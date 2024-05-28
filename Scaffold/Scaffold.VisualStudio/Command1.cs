using System.Collections;
using Microsoft;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Extensibility.Shell;
using System.Diagnostics;
using System.Text;
using Scaffold.Core.Abstract;
using System.IO.Compression;

namespace Scaffold.VisualStudio
{
    [VisualStudioContribution]
    internal class Command1 : Command
    {
        private readonly TraceSource _logger;
        
        public Command1(TraceSource traceSource)
        {
            _logger = Requires.NotNull(traceSource, nameof(traceSource));
        }
        
        public override CommandConfiguration CommandConfiguration => new("%Scaffold.VisualStudio.Command1.DisplayName%")
        {
            // Use this object initializer to set optional parameters for the command. The required parameter,
            // displayName, is set above. DisplayName is localized and references an entry in .vsextension\string-resources.json.
            Icon = new(ImageMoniker.KnownValues.Extension, IconSettings.IconAndText),
            Placements = [CommandPlacement.KnownPlacements.ExtensionsMenu],
        };
        
        public override Task InitializeAsync(CancellationToken cancellationToken)
        {
            // Use InitializeAsync for any one-time setup or initialization.
            return base.InitializeAsync(cancellationToken);
        }
        
        public override async Task ExecuteCommandAsync(IClientContext context, CancellationToken cancellationToken)
        {
            var response = new StringBuilder();

            var project = await context.GetActiveProjectAsync(CancellationToken.None);
            if (project?.Path == null)
            {
                response.Append("You must open a solution in visual studio before the project can be inspected.");
                await Extensibility.Shell().ShowPromptAsync(response.ToString(), PromptOptions.OK, cancellationToken);
                return;
            }

            var lastIndex = project.Path.LastIndexOf(@"\", StringComparison.Ordinal);
            var path = project.Path[..lastIndex];

            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "Scaffold.Console.exe",
                        Arguments = path,
                        RedirectStandardOutput = true,
                        CreateNoWindow = false,
                        // TODO We need to unfix this from specific path and instead have it installed some other way.
                        WorkingDirectory = @"C:\Users\d.growns\Documents\Repos\WPF\SCaFFOLD\Scaffold\Scaffold.Console\bin\Debug\net8.0"
                    }
                };

                process.Start();
                while (process.StandardOutput.EndOfStream == false)
                {
                    response.Append(await process.StandardOutput.ReadLineAsync(cancellationToken)).Append(Environment.NewLine);
                }

                await Extensibility.Shell().ShowPromptAsync(response.ToString(), PromptOptions.OK, cancellationToken);
            }
            catch (Exception ex)
            {
                ;
            }

        }
    }
}
