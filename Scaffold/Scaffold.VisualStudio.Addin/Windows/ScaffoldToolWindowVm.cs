using Microsoft.VisualStudio.Extensibility.UI;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.Extensibility;

namespace Scaffold.VisualStudio.Addin.Windows
{
    [DataContract]
    internal class ScaffoldToolWindowVm : NotifyPropertyChangedObject
    {
        private bool _hasActiveProject;
        private string _activeProjectPath;

        private IClientContext Context { get; }

        public ScaffoldToolWindowVm(VisualStudioExtensibility extensibility)
        {
            //Context = context;

            //var project = context.GetActiveProjectAsync(CancellationToken.None).Result;
            //ActiveProjectPath = project?.Path;

            HelloCommand = new AsyncCommand((parameter, clientContext, cancellationToken) =>
            {
                //Text = $"Hello {parameter as string}!";
                return Task.CompletedTask;
            });
        }

        [DataMember]
        public bool HasActiveProject
        {
            get => _hasActiveProject;
            set => SetProperty(ref _hasActiveProject, value);
        }

        [DataMember]
        public string ActiveProjectPath
        {
            get => _activeProjectPath;
            set
            {
                SetProperty(ref _activeProjectPath, value);
                HasActiveProject = value != null;
            }
        }


        [DataMember]
        public AsyncCommand HelloCommand { get; }
    }
}
