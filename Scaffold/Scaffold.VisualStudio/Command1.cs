﻿using System.Collections;
using Microsoft;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Extensibility.Shell;
using System.Diagnostics;
using System.Text;
using Scaffold.Core.Abstract;

namespace Scaffold.VisualStudio
{
    /// <summary>
    /// Command1 handler.
    /// </summary>
    [VisualStudioContribution]
    internal class Command1 : Command
    {
        private readonly TraceSource logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command1"/> class.
        /// </summary>
        /// <param name="traceSource">Trace source instance to utilize.</param>
        public Command1(TraceSource traceSource)
        {
            // This optional TraceSource can be used for logging in the command. You can use dependency injection to access
            // other services here as well.
            this.logger = Requires.NotNull(traceSource, nameof(traceSource));
        }

        /// <inheritdoc />
        public override CommandConfiguration CommandConfiguration => new("%Scaffold.VisualStudio.Command1.DisplayName%")
        {
            // Use this object initializer to set optional parameters for the command. The required parameter,
            // displayName, is set above. DisplayName is localized and references an entry in .vsextension\string-resources.json.
            Icon = new(ImageMoniker.KnownValues.Extension, IconSettings.IconAndText),
            Placements = [CommandPlacement.KnownPlacements.ExtensionsMenu],
        };

        /// <inheritdoc />
        public override Task InitializeAsync(CancellationToken cancellationToken)
        {
            // Use InitializeAsync for any one-time setup or initialization.
            return base.InitializeAsync(cancellationToken);
        }

        /// <inheritdoc />
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

            var process = new Process
            {
                StartInfo = new ProcessStartInfo 
                {
                    FileName = "dotnet",
                    Arguments = "build --no-restore",
                    RedirectStandardOutput = true,
                    CreateNoWindow = false,
                    WorkingDirectory = path
                }
            };

            process.Start();
            while (process.StandardOutput.EndOfStream == false)
            {
                response.Append(await process.StandardOutput.ReadLineAsync(cancellationToken)).Append(Environment.NewLine);
            }

            var assemblyReader = new BinariesAssemblyReader(path);
            var assembly = assemblyReader.GetAssembly();

            if (assembly == null)
            {
                await Extensibility.Shell()
                    .ShowPromptAsync(
                        "Could not read the project binaries folder to obtain calculation assembly.",
                                PromptOptions.OK, cancellationToken);
                return;
            }

            try
            {
                var types = assembly.GetTypes();
                var calculation = types.FirstOrDefault(x => x.FullName.Contains("AdditionCalculation"));
                var instance = assembly.CreateInstance(calculation.FullName);
                //instance.GetType().GetMethod("LoadIoCollections").Invoke(instance, null);
                var castInstance = (CalculationBase)instance;
                castInstance.LoadIoCollections();
            }
            catch (Exception ex)
            {
                response.Append(Environment.NewLine)
                    .Append("ERROR GETTING TYPE")
                    .Append(Environment.NewLine)
                    .Append(ex.Message);
            }

            // if (instance != null)
            // {
            //     response.Append(Environment.NewLine).Append("Inputs").Append(Environment.NewLine);
            //     foreach (var value in 
            //              (IEnumerable)instance.GetType().GetMethod("GetInputs").Invoke(instance, null))
            //     {
            //         var displayName = value.GetType().GetProperty("DisplayName").GetValue(value);
            //         var propertyValue = value.GetType().GetProperty("Value").GetValue(value);
            //         response.Append($"{displayName}: {propertyValue}").Append(Environment.NewLine);
            //     }
            //
            //     response.Append(Environment.NewLine).Append("Outputs").Append(Environment.NewLine);
            //     foreach (var value in
            //              (IEnumerable)instance.GetType().GetMethod("GetOutputs").Invoke(instance, null))
            //     {
            //         var displayName = value.GetType().GetProperty("DisplayName").GetValue(value);
            //         var propertyValue = value.GetType().GetProperty("Value").GetValue(value);
            //         response.Append($"{displayName}: {propertyValue}").Append(Environment.NewLine);
            //     }
            // }

            await Extensibility.Shell().ShowPromptAsync(response.ToString(), PromptOptions.OK, cancellationToken);
        }
    }
}