using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.ToolWindows;
using Microsoft.VisualStudio.RpcContracts.RemoteUI;
using System.Threading;
using System.Threading.Tasks;

namespace Scaffold.VisualStudio.Windows
{
    /// <summary>
    /// A sample tool window.
    /// </summary>
    [VisualStudioContribution]
    public class ScaffoldToolWindow : ToolWindow
    {
        private ScaffoldToolWindowContent Content { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScaffoldToolWindow" /> class.
        /// </summary>
        public ScaffoldToolWindow()
        {
            Title = "SCaFFOLD explorer";
            
        }

        /// <inheritdoc />
        public override ToolWindowConfiguration ToolWindowConfiguration => new()
        {
            // Use this object initializer to set optional parameters for the tool window.
            Placement = ToolWindowPlacement.Floating,
        };

        /// <inheritdoc />
        public override async Task InitializeAsync(CancellationToken cancellationToken)
        {
            var result = await Extensibility.Documents().GetOpenDocumentsAsync(cancellationToken);
            Content = new ScaffoldToolWindowContent(Extensibility);
            // Use InitializeAsync for any one-time setup or initialization.
            ;
        }

        /// <inheritdoc />
        public override Task<IRemoteUserControl> GetContentAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<IRemoteUserControl>(Content);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Content.Dispose();

            base.Dispose(disposing);
        }
    }
}
