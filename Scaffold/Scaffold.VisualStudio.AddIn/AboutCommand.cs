using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Extensibility.Shell;

namespace Scaffold.VisualStudio.AddIn;

[VisualStudioContribution]
internal class AboutCommand : Command
{
    public override CommandConfiguration CommandConfiguration => new("%Scaffold.VisualStudio.AddIn.AboutCommand.DisplayName%")
    {
        Icon = new(ImageMoniker.KnownValues.Extension, IconSettings.IconAndText),
        Placements = [CommandPlacement.KnownPlacements.ExtensionsMenu],
    };

    public override async Task ExecuteCommandAsync(IClientContext context, CancellationToken cancellationToken)
    {
        await Extensibility
            .Shell()
            .ShowPromptAsync(
                "For more information on SCaFFOLD, please go to the website: https://scaffold.magmaworks.co.uk", 
                        PromptOptions.OK, cancellationToken);
    }
}