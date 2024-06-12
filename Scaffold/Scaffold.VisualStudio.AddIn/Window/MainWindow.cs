using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.ToolWindows;
using Microsoft.VisualStudio.RpcContracts.Documents;
using Microsoft.VisualStudio.RpcContracts.RemoteUI;

namespace Scaffold.VisualStudio.AddIn.Window;

/// <summary>
/// A sample tool window.
/// </summary>
[VisualStudioContribution]
public class MainWindow : ToolWindow
{
    private MainWindowContent Content { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow" /> class.
    /// </summary>
    public MainWindow()
    {
        Title = "SCaFFOLD Explorer";
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
        Content = new MainWindowContent();

        var listener = (IDocumentEventsListener)Content.DataContext;
        if (listener == null)
            throw new ArgumentException("The data context should be an IDocumentEventsListener.");

        await Extensibility.Documents().SubscribeAsync(listener, null, cancellationToken);
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