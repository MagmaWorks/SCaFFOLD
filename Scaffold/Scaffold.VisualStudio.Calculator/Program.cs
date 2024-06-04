using System.Diagnostics;
using Scaffold.Core.Abstract;
using Scaffold.Core.Interfaces;
using Scaffold.VisualStudio.Models;
using Scaffold.VsCalculator;

namespace Scaffold.VisualStudio.Calculator
{
    internal static class Program
    {
        private static List<FileChangeDetail> ProjectFiles { get; set; }
        private static string RootPath { get; set; }
        private static Timer Timer { get; set; }
        
        private static void Main(string[] args)
        {
            // TODO Remove test value. Get .NET version from project for binary reader.
            RootPath = args?.Length > 0 ? args[0] : @"C:\Users\d.growns\Documents\Repos\ScaffoldForVsTesting\VsTesting";
            
            CreateTimer();
            WaitForExit();
        }

        private static void CreateTimer()
            => Timer = new Timer(CheckForChanges, null, 0, 1000);
        
        private static void ReadInstance(CalculationBase instance)
        {
            if (instance == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("FAILED: Could not read instance.");
                Console.ResetColor();
                return;
            }
            
            WriteCalcValueList(instance.GetInputs(), "INPUTS");
            WriteCalcValueList(instance.GetOutputs(), "OUTPUTS");
        }

        private static void WriteCalcValueList(IEnumerable<ICalcValue> values, string name)
        {
            Console.WriteLine($"----- {name} -----");
            foreach (var value in values)
            {
                Console.WriteLine($"{value.DisplayName}: {value.GetValue()}");
            }
            
            Console.WriteLine("-----");
        }
        


        private static void WaitForExit()
        {
            Console.WriteLine("Type 'exit' to close the application.");
            var result = Console.ReadLine();
            if (result?.ToLower() == "exit")
            {
                Timer.Dispose();
                Environment.Exit(0);
            }
            else
            {
                // ReSharper disable once TailRecursiveCall
                WaitForExit();
            }
        }

        private static void SetChangeDetail(string filePath, bool isFirstRun)
        {
            var info = new FileInfo(filePath);
            var existingFile = ProjectFiles.FirstOrDefault(x => x.FullPath == filePath);
            if (existingFile == null)
            {
                ProjectFiles.Add(new FileChangeDetail
                {
                    FullPath = filePath,
                    LastModified = info.LastWriteTime,
                    HasChanged = !isFirstRun
                });
            }
            else
            {
                if (existingFile.LastModified == info.LastWriteTime)
                {
                    existingFile.HasChanged = false;
                    return;
                }
                    
                existingFile.LastModified = info.LastWriteTime;
                existingFile.HasChanged = true;
            }
        }
        
        private static void CheckForChanges(object state)
        {
            var isFirstRun = ProjectFiles == null;
            ProjectFiles ??= [];
            
            var binFolder = Path.Combine(RootPath, "bin");
            var objFolder = Path.Combine(RootPath, "obj");
            
            var files = Directory.GetFiles(RootPath, "", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (file.StartsWith(binFolder) || file.StartsWith(objFolder))
                    continue;
                
                SetChangeDetail(file, isFirstRun);
            }

            if (ProjectFiles.Any(x => x.HasChanged))
            {
                Timer.Dispose();
                Timer = null;
                ReadCalculation();
            }
        }

        private static void ReadCalculation()
        {
            try
            {
                if (Directory.Exists(RootPath) == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The supplied path does not exist - ensure you have a tab open within the project you want to read.");
                    Console.ReadKey();
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Building project...");
                Console.ResetColor();
                
                var dotnetBuild = new DotnetBuild();
                var result = dotnetBuild.Run(RootPath);

                foreach (var line in result.Output)
                    Console.WriteLine(line);
            
                if (result.ExitCode != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed to build project - see error detail above.");
                    Console.ReadKey();
                    return;
                }
            
                var reader = new BinariesAssemblyReader(RootPath);
                var assembly = reader.GetAssembly();
                
                var instance = (CalculationBase)assembly.CreateInstance("VsTesting.Core.AdditionCalculation");
                instance?.LoadIoCollections();
                ReadInstance(instance);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            finally
            {
                CreateTimer();
            }
        }
    }
}
