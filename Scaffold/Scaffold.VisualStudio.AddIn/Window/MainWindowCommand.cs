using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;

namespace Scaffold.VisualStudio.AddIn.Window;

[VisualStudioContribution]
public class MainWindowCommand : Command
{
    public override CommandConfiguration CommandConfiguration => new(displayName: "Show SCaFFOLD explorer")
    {
        Placements = [CommandPlacement.KnownPlacements.ExtensionsMenu],
        Icon = new(ImageMoniker.KnownValues.Extension, IconSettings.IconAndText),
    };

    public override async Task ExecuteCommandAsync(IClientContext context, CancellationToken cancellationToken)
    {
        await Extensibility.Shell().ShowToolWindowAsync<MainWindow>(activate: true, cancellationToken);
    }
}