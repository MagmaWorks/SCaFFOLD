using System.Diagnostics;
using Scaffold.Core.Abstract;
using Scaffold.Core.Interfaces;

namespace Scaffold.Console
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
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("FAILED: Could not read instance.");
                System.Console.ResetColor();
                return;
            }
            
            WriteCalcValueList(instance.GetInputs(), "INPUTS");
            WriteCalcValueList(instance.GetOutputs(), "OUTPUTS");
        }

        private static void WriteCalcValueList(IEnumerable<ICalcValue> values, string name)
        {
            System.Console.WriteLine($"----- {name} -----");
            foreach (var value in values)
            {
                System.Console.WriteLine($"{value.DisplayName}: {value.GetValue()}");
            }
            
            System.Console.WriteLine("-----");
        }
        


        private static void WaitForExit()
        {
            System.Console.WriteLine("Type 'exit' to close the application.");
            var result = System.Console.ReadLine();
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
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine("The supplied path does not exist - ensure you have a tab open within the project you want to read.");
                    System.Console.ReadKey();
                    return;
                }
            
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo 
                    {
                        FileName = "dotnet",
                        Arguments = "build --no-restore",
                        RedirectStandardOutput = true,
                        CreateNoWindow = false,
                        WorkingDirectory = RootPath
                    }
                };
            
                System.Console.ForegroundColor = ConsoleColor.Yellow;
                System.Console.WriteLine("Building project...");
                System.Console.ResetColor();
            
                process.Start();
                while (process.StandardOutput.EndOfStream == false)
                {
                    System.Console.WriteLine(process.StandardOutput.ReadLine());
                }
            
                if (process.ExitCode != 0)
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine("Failed to build project - see error detail above.");
                    System.Console.ReadKey();
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
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine(ex.Message);
            }
            finally
            {
                CreateTimer();
            }
        }
    }
}
