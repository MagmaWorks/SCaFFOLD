using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;

namespace Scaffold.VisualStudio.AddIn.Windows
{
    /// <summary>
    /// A command for showing a tool window.
    /// </summary>
    [VisualStudioContribution]
    public class ScaffoldToolWindowCommand : Command
    {
        /// <inheritdoc />
        public override CommandConfiguration CommandConfiguration => new(displayName: "Open SCaFFOLD Explorer")
        {
            // Use this object initializer to set optional parameters for the command. The required parameter,
            // displayName, is set above. To localize the displayName, add an entry in .vsextension\string-resources.json
            // and reference it here by passing "%Scaffold.VisualStudio.Addin.ScaffoldToolWindowCommand.DisplayName%" as a constructor parameter.
            Placements = [CommandPlacement.KnownPlacements.ToolsMenu],
            Icon = new(ImageMoniker.KnownValues.Extension, IconSettings.IconAndText),
        };

        /// <inheritdoc />
        public override async Task ExecuteCommandAsync(IClientContext context, CancellationToken cancellationToken)
        {
            await Extensibility.Shell().ShowToolWindowAsync<ScaffoldToolWindow>(activate: true, cancellationToken);
        }
    }
}
