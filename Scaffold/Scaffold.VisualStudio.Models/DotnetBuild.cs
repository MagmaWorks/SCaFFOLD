using System.Diagnostics;
using Scaffold.VisualStudio.Models.Results;

namespace Scaffold.VisualStudio.Models;

public class DotnetBuild
{
    /// <summary>
    /// Wrapper for running dotnet build with the same settings in both targets, Returns stream output lines as individual list items.
    /// </summary>
    public ProcessResult Run(string workingDirectory)
    {
        var result = new ProcessResult { Output = new List<string>() };
        var process = new Process
        {
            StartInfo = new ProcessStartInfo 
            {
                FileName = "dotnet",
                Arguments = "build --no-restore",
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                WorkingDirectory = workingDirectory
            }
        };
        
        process.Start();
        
        while (process.StandardOutput.EndOfStream == false)
            result.Output.Add(process.StandardOutput.ReadLine());

        result.ExitCode = process.ExitCode;
        return result;
    }
}