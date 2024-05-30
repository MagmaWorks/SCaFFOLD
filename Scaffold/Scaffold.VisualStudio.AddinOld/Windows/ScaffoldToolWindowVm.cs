using Microsoft.VisualStudio.Extensibility.UI;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.RpcContracts.Documents;

namespace Scaffold.VisualStudio.AddinOld.Windows
{
    [DataContract]
    internal class ScaffoldToolWindowVm : NotifyPropertyChangedObject, IDocumentEventsListener
    {
        private bool _hasActiveProject;
        private string _activeProjectPath;


        public ScaffoldToolWindowVm()
        {
            //Context = context;

            //var project = context.GetActiveProjectAsync(CancellationToken.None).Result;
            //ActiveProjectPath = project?.Path;

            //HelloCommand = new AsyncCommand((parameter, clientContext, cancellationToken) =>
            //{
            //    //Text = $"Hello {parameter as string}!";
            //    return Task.CompletedTask;
            //});
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


        //[DataMember]
        //public AsyncCommand HelloCommand { get; }

        public void Dispose()
        {
            // Not required, the tool window is the only access point to this view model.
        }

        public async Task OpenedAsync(DocumentEventArgs e, CancellationToken token)
        {
            ;
        }
        
        public Task ClosedAsync(DocumentEventArgs e, CancellationToken token) => Task.CompletedTask;
        public Task SavingAsync(DocumentEventArgs e, CancellationToken token) => Task.CompletedTask;
        public Task SavedAsync(DocumentEventArgs e, CancellationToken token) => Task.CompletedTask;
        public Task RenamedAsync(RenamedDocumentEventArgs e, CancellationToken token) => Task.CompletedTask;
        public Task ShownAsync(DocumentEventArgs e, CancellationToken token) => Task.CompletedTask;
        public Task HiddenAsync(DocumentEventArgs e, CancellationToken token) => Task.CompletedTask;
    }
}
