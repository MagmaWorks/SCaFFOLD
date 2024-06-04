using System.Reflection;
using Microsoft.VisualStudio.Extensibility.UI;
using System.Runtime.Serialization;
using System.Security;
using System.Windows;
using System.Xml;
using Microsoft.VisualStudio.RpcContracts.Documents;
using Scaffold.VisualStudio.AddIn.Classes;
using Scaffold.VisualStudio.Models;
using FileInfo = System.IO.FileInfo;

// TODO: Catch and display all exceptions in any of this code.

//
// Notes
// WPF converters are unfortunately not applying on the Extensibility project from XAML, so I've added them to code behind.
//
namespace Scaffold.VisualStudio.AddIn.Window
{
    [DataContract]
    internal class MainWindowViewModel : NotifyPropertyChangedObject, IDocumentEventsListener
    {
        private Visibility _onLoadVisibility = Visibility.Visible;
        private Visibility _waitingForTabVisibility = Visibility.Visible;
        private Visibility _hasActiveProjectVisibility = Visibility.Collapsed;
        private Visibility _isWatchingVisibility = Visibility.Collapsed;
        private string _activeProjectPath;
        private bool _watcherIsRunning;

        private string LastShownDocumentRead { get; set; }

        public MainWindowViewModel()
        {
            // HelloCommand = new AsyncCommand((parameter, clientContext, cancellationToken) =>
            // {
            //     Text = $"Hello {parameter as string}!";
            //     return Task.CompletedTask;
            // });
            var wait = "";
        }

        [DataMember]
        public Visibility OnLoadVisibility
        {
            get => _onLoadVisibility;
            set => SetProperty(ref _onLoadVisibility, value);
        }

        [DataMember]
        public Visibility WaitingForTabVisibility
        {
            get => _waitingForTabVisibility;
            set => SetProperty(ref _waitingForTabVisibility, value);
        }

        [DataMember]
        public Visibility HasActiveProjectVisibility
        {
            get => _hasActiveProjectVisibility;
            set => SetProperty(ref _hasActiveProjectVisibility, value);
        }

        [DataMember]
        public Visibility IsWatchingVisibility
        {
            get => _isWatchingVisibility;
            set => SetProperty(ref _isWatchingVisibility, value);
        }
        
        [DataMember]
        public List<TreeItem> Watching { get; }

        [DataMember]
        public string ActiveProjectPath
        {
            get => _activeProjectPath;
            set
            {
                SetProperty(ref _activeProjectPath, value);
                HasActiveProjectVisibility = string.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        
        [DataMember]
        public bool WatcherIsRunning
        {
            get => _watcherIsRunning;
            set => SetProperty(ref _watcherIsRunning, value);
        }

        // [DataMember]
        // public AsyncCommand HelloCommand { get; }

        public Task OpenedAsync(DocumentEventArgs e, CancellationToken token) => Task.CompletedTask;
        public Task ClosedAsync(DocumentEventArgs e, CancellationToken token) => Task.CompletedTask;
        public Task SavingAsync(DocumentEventArgs e, CancellationToken token) => Task.CompletedTask;
        public Task SavedAsync(DocumentEventArgs e, CancellationToken token) => Task.CompletedTask;
        public Task RenamedAsync(RenamedDocumentEventArgs e, CancellationToken token) => Task.CompletedTask;
        public Task HiddenAsync(DocumentEventArgs e, CancellationToken token) => Task.CompletedTask;

        /// <summary>
        /// Executing assembly from VS appears to read \ and . as final characters even after last index of.
        /// This adds additional sanitization to reduce the string further to only textual characters without another directory break.
        /// </summary>
        private string SanitizeBasePath(string path)
        {
            while (true)
            {
                if (path.Last() == '.' || path.Last() == '\\')
                {
                    path = path[..^1];
                    continue;
                }

                return path;
            }
        }

        private static ProjectDetails GetProjectDetails(DirectoryInfo directory)
        {
            var files = directory.GetFiles("*.csproj");

            if (directory.Parent == null)
                throw new ArgumentException("Directory or parent directory could not be found for current tab location.");

            if (files.Length == 0)
                return GetProjectDetails(directory.Parent);

            var csProj = files[0];
            
            var doc = new XmlDocument();
            doc.Load(csProj.FullName);

            const string mainGroup = "/Project/PropertyGroup";
            var targetFramework = doc.DocumentElement?.SelectSingleNode($"{mainGroup}/TargetFramework");
            var assemblyName = doc.DocumentElement?.SelectSingleNode($"{mainGroup}/AssemblyName");
            var outputType = doc.DocumentElement?.SelectSingleNode($"{mainGroup}/OutputType");

            if (targetFramework == null)
                throw new ArgumentException(
                    "SCaFFOLD explorer failed to obtain the target framework from the csproj file.");
            
            return new ProjectDetails
            {
                TargetFramework = targetFramework.InnerText,
                AssemblyName = assemblyName?.InnerText ?? csProj.Name.Replace(".csproj", ""),
                IsExecutable = outputType != null && outputType.InnerText.ToLower() == "exe",
                ProjectFilePath = directory.FullName
            };
        }

        private bool HasCalculations(ProjectDetails projectDetails)
        {
            if (File.Exists(projectDetails.AssemblyPath()) == false)
            {
                var dotnetBuild = new DotnetBuild();
                var result = dotnetBuild.Run(projectDetails.ProjectFilePath);

                if (result.ExitCode != 0)
                    throw new ArgumentException(string.Join(',', result.Output));
            }

            // Note: This is run a second time on purpose - if it still doesn't exist, it is exceptional.
            if (File.Exists(projectDetails.AssemblyPath()) == false)
                throw new ArgumentException(
                    $"Failed to load the assembly after dotnet build - it could not be found under path {projectDetails.AssemblyPath()}");
            
            var assembly = Assembly.LoadFrom(projectDetails.AssemblyPath());

            var matchingTypes = assembly.GetTypes().Where(x => x.GetInterface("Scaffold.Core.Interfaces.ICalculation") != null);
            return matchingTypes.Any();
        }

        public Task ShownAsync(DocumentEventArgs e, CancellationToken token)
        {
            if (WatcherIsRunning)
                return Task.CompletedTask;
            
            if (string.IsNullOrEmpty(LastShownDocumentRead) == false && e.Moniker.AbsolutePath == LastShownDocumentRead)
                return Task.CompletedTask;

            //LastShownDocumentRead = e.Moniker.AbsolutePath; // TODO: Put back

            var fileInfo = new FileInfo(e.Moniker.AbsolutePath);
            var projectDetails = GetProjectDetails(fileInfo.Directory);

            if (HasCalculations(projectDetails) == false)
                return Task.CompletedTask;

            ActiveProjectPath = projectDetails.ProjectFilePath;
            OnLoadVisibility = Visibility.Visible;
            WaitingForTabVisibility = Visibility.Collapsed;
            HasActiveProjectVisibility = Visibility.Visible;

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            // Resources disposed by parent.
        }
    }
}
