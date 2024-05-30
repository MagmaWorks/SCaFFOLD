using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.ToolWindows;
using Microsoft.VisualStudio.RpcContracts.Documents;
using Microsoft.VisualStudio.RpcContracts.RemoteUI;

namespace Scaffold.VisualStudio.AddinOld.Windows
{
    [VisualStudioContribution]
    public class ScaffoldToolWindow : ToolWindow
    {
        private ScaffoldToolWindowContent Content { get; set; }
        
        public ScaffoldToolWindow()
        {
            Title = "SCaFFOLD explorer";
        }
        
        public override ToolWindowConfiguration ToolWindowConfiguration => new()
        {
            Placement = ToolWindowPlacement.Floating
        };
        
        public override async Task InitializeAsync(CancellationToken cancellationToken)
        {
            Content = new ScaffoldToolWindowContent();

            var listener = (IDocumentEventsListener)Content.DataContext;
            if (listener == null)
                throw new ArgumentException("The data context should be an IDocumentEventsListener.");
            
            await Extensibility.Documents().SubscribeAsync(listener, null, cancellationToken);
        }
        
        public override Task<IRemoteUserControl> GetContentAsync(CancellationToken cancellationToken)
            => Task.FromResult<IRemoteUserControl>(Content);
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Content.Dispose();

            base.Dispose(disposing);
        }
    }
}
