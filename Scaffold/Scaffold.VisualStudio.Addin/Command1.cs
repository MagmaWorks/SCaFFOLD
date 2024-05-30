//using Microsoft;
//using Microsoft.VisualStudio.Extensibility;
//using Microsoft.VisualStudio.Extensibility.Commands;
//using Microsoft.VisualStudio.Extensibility.Shell;
//using System.Diagnostics;
//using System.Text;

//namespace Scaffold.VisualStudio.AddIn
//{
//    [VisualStudioContribution]
//    internal class Command1 : Command
//    {
//        private readonly TraceSource _logger;
        
//        public Command1(TraceSource traceSource)
//        {
//            _logger = Requires.NotNull(traceSource, nameof(traceSource));
//        }

//        /// <summary>
//        /// Executing assembly from VS appears to read \ and . as final characters even after last index of.
//        /// This adds additional sanitization to reduce the string further to only textual characters without another directory break.
//        /// </summary>
//        private string SanitizeBasePath(string path)
//        {
//            while (true)
//            {
//                if (path.Last() == '.' || path.Last() == '\\')
//                {
//                    path = path[..^1];
//                    continue;
//                }

//                return path;
//            }
//        }

//        private string GetWorkingDirectory()
//        {
//            var executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
//            var lastIndex = executingAssembly.Location.LastIndexOf(@"\", StringComparison.Ordinal);
//            var workingDirectory = executingAssembly.Location[..lastIndex];

//            return SanitizeBasePath(workingDirectory);
//        }

//        public override CommandConfiguration CommandConfiguration => new("%Scaffold.VisualStudio.Addin.Command1.DisplayName%")
//        {
//            // Use this object initializer to set optional parameters for the command. The required parameter,
//            // displayName, is set above. DisplayName is localized and references an entry in .vsextension\string-resources.json.
//            Icon = new(ImageMoniker.KnownValues.Extension, IconSettings.IconAndText),
//            Placements = [CommandPlacement.KnownPlacements.ExtensionsMenu],
//        };
        
//        public override Task InitializeAsync(CancellationToken cancellationToken)
//        {
//            // Use InitializeAsync for any one-time setup or initialization.
//            return base.InitializeAsync(cancellationToken);
//        }
        
//        public override async Task ExecuteCommandAsync(IClientContext context, CancellationToken cancellationToken)
//        {
//            var response = new StringBuilder();

//            var project = await context.GetActiveProjectAsync(CancellationToken.None);
//            var workingDirectory = GetWorkingDirectory();
//            var consolePath = $@"{workingDirectory}\Scaffold.Console.exe";

//            var exists = File.Exists(consolePath);
//            if (project?.Path == null)
//            {
//                response.Append("You must open a solution in visual studio before the project can be inspected.");
//                await Extensibility.Shell().ShowPromptAsync(response.ToString(), PromptOptions.OK, cancellationToken);
//                return;
//            }

//            try
//            {
//                var process = new Process
//                {
//                    StartInfo = new ProcessStartInfo
//                    {
//                        FileName = "Scaffold.Console.exe",
//                        Arguments = project.Path,
//                        RedirectStandardOutput = true,
//                        CreateNoWindow = false,
//                        WorkingDirectory = workingDirectory
//                    }
//                };

//                process.Start();
//                while (process.StandardOutput.EndOfStream == false)
//                {
//                    response.Append(await process.StandardOutput.ReadLineAsync(cancellationToken)).Append(Environment.NewLine);
//                }

//                if (response.Length == 0)
//                    response.Append("Nothing to add");

//                await Extensibility.Shell().ShowPromptAsync(response.ToString(), PromptOptions.OK, cancellationToken);
//            }
//            catch (Exception ex)
//            {
//                await Extensibility.Shell().ShowPromptAsync(ex.Message, PromptOptions.OK, cancellationToken);
//                ;
//            }

//        }
//    }
//}
