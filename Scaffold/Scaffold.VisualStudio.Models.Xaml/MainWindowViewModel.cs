﻿using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Web;
using System.Windows;
using System.Xml;
using Microsoft.VisualStudio.Extensibility.UI;
using Microsoft.VisualStudio.RpcContracts.Documents;
using Scaffold.VisualStudio.Models.Main;
using Scaffold.VisualStudio.Models.Results;
using FileInfo = System.IO.FileInfo;

// TODO: Formula text colour based on VS theme, remove border.

//
// Notes
// WPF converters are unfortunately not applying on the Extensibility project from XAML, so I've added them to code behind.
//
namespace Scaffold.VisualStudio.Models.Xaml;

[DataContract]
public class MainWindowViewModel : NotifyPropertyChangedObject, IDocumentEventsListener
{
    private string _activeProjectPath;
    private string _runTime;
    private string _runInformation;
    private double _successRatio;
    private bool _watcherIsRunning;

    private ProjectDetails ProjectDetails { get; set; }
    private string LastShownDocumentRead { get; set; }

    public MainWindowViewModel()
    {
        Settings = Globals.GetSettings<DisplaySettings>();
        Watching = [];
        RunTime = "No run information available.";
        ActiveProjectPath = null;
        
        StartWatcherCommand = new AsyncCommand((_, _, _) =>
        {
            WatcherIsRunning = true;
            return Task.CompletedTask;
        });

        StopWatcherCommand = new AsyncCommand((_, _, _) =>
        {
            WatcherIsRunning = false;
            return Task.CompletedTask;
        });
    }

    [DataMember]
    public string ActiveProjectPath
    {
        get => _activeProjectPath;
        set => SetProperty(ref _activeProjectPath, value);
    }

    [DataMember]
    public string RunTime
    {
        get => _runTime;
        set => SetProperty(ref _runTime, value);
    }

    [DataMember]
    public string RunInformation
    {
        get => _runInformation;
        set => SetProperty(ref _runInformation, value);
    }
    
    [DataMember]
    public double SuccessRatio
    {
        get => _successRatio;
        set => SetProperty(ref _successRatio, value);
    }

    [DataMember]
    public bool WatcherIsRunning
    {
        get => _watcherIsRunning;
        set => SetProperty(ref _watcherIsRunning, value);
    }

    [DataMember] public DisplaySettings Settings { get; set; }
    [DataMember] public ObservableList<TreeItem> Watching { get; }
    [DataMember] public AsyncCommand StartWatcherCommand { get; set; }
    [DataMember] public AsyncCommand StopWatcherCommand { get; set; }

    public Task OpenedAsync(DocumentEventArgs e, CancellationToken token) => Task.CompletedTask;
    public Task ClosedAsync(DocumentEventArgs e, CancellationToken token) => Task.CompletedTask;
    public Task SavingAsync(DocumentEventArgs e, CancellationToken token) => Task.CompletedTask;
    public Task RenamedAsync(RenamedDocumentEventArgs e, CancellationToken token) => Task.CompletedTask;
    public Task HiddenAsync(DocumentEventArgs e, CancellationToken token) => Task.CompletedTask;
    
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
            ProjectFilePath = directory.FullName,
            CsProjFile = csProj.Name
        };
    }

    private bool HasCalculations(ProjectDetails projectDetails)
    {
        if (File.Exists(projectDetails.AssemblyPath()) == false)
        {
            var dotnetBuild = new DotnetBuild();
            var result = dotnetBuild.Run(projectDetails.ProjectFilePath, Settings.DotnetBuildNoRestore);

            if (result.ExitCode != 0)
                throw new ArgumentException(string.Join(',', result.Output));
        }

        // Note: This is run a second time on purpose - if it still doesn't exist, it is exceptional.
        if (File.Exists(projectDetails.AssemblyPath()) == false)
            throw new ArgumentException(
                $"Failed to load the assembly after dotnet build - it could not be found under path {projectDetails.AssemblyPath()}");
            
        var assembly = Assembly.LoadFrom(projectDetails.AssemblyPath());
        var matchingTypes = assembly.GetCalculationTypes();
        
        return matchingTypes.Count > 0;
    }

    public Task ShownAsync(DocumentEventArgs e, CancellationToken token)
    {
        if (WatcherIsRunning)
            return Task.CompletedTask;
            
        if (string.IsNullOrEmpty(LastShownDocumentRead) == false && e.Moniker.AbsolutePath == LastShownDocumentRead)
            return Task.CompletedTask;

        LastShownDocumentRead = e.Moniker.AbsolutePath; 

        var fileInfo = new FileInfo(e.Moniker.AbsolutePath);
        var projectDetails = GetProjectDetails(fileInfo.Directory);

        if (HasCalculations(projectDetails) == false)
            return Task.CompletedTask;

        ProjectDetails = projectDetails;
        ActiveProjectPath = projectDetails.ProjectFilePath;

        return Task.CompletedTask;
    }

    public async Task SavedAsync(DocumentEventArgs e, CancellationToken token)
    {
        var runStartTime = DateTime.Now;
        var workingDirectory = Globals.GetWorkingDirectory();

        var detailsJson = JsonSerializer.Serialize(ProjectDetails); 
        detailsJson = HttpUtility.JavaScriptStringEncode(detailsJson);

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = $"{workingDirectory}\\Scaffold.VisualStudio.Calculator.exe",
                Arguments = detailsJson,
                RedirectStandardOutput = true,
                CreateNoWindow = false
            }
        };

        process.Start();

        var jsonString = await process.StandardOutput.ReadToEndAsync(token);
        var fullResult = JsonSerializer.Deserialize<CalculationAssemblyResult>(jsonString);

        process.Dispose();

        UpdateWatchingList(fullResult, runStartTime);
    }
    
    private void UpdateWatchingList(CalculationAssemblyResult fullResult, DateTime runStartTime)
    {
        var successfulResults = 0;
        var updated = new List<TreeItem>();

        if (fullResult.RunError == null)
        {
            foreach (var calculationResult in fullResult.Results)
            {
                var existing =
                    Watching.FirstOrDefault(x => x.AssemblyQualifiedTypeName == calculationResult.AssemblyQualifiedTypeName);

                var treeItem = new TreeItem(calculationResult);
                treeItem.SetExpanderState(Settings.AlwaysExpandCalculations, existing);
                
                if (calculationResult.IsSuccess)
                    successfulResults++;

                updated.Add(treeItem);
            }
        }
        else
        {
            updated.Add(new TreeItem(fullResult.RunError) { IsExpanded = true });
        }

        Watching.Clear();
        Watching.AddRange(updated);

        var runEndTime = DateTime.Now;
        var runTime = (runEndTime - runStartTime).TotalSeconds;
        
        SuccessRatio = successfulResults / (double)fullResult.Results.Count;
        RunTime = $"Ran for {Math.Round(runTime, 2)} seconds, finished at {runEndTime:HH:mm:ss}";
        RunInformation = $"{successfulResults} of {fullResult.Results.Count} successful";
    }

    public void Dispose()
    {
        // Resources disposed by parent.
    }
}