﻿using System.Diagnostics;
using Scaffold.VisualStudio.Models.Results;

namespace Scaffold.VisualStudio.Models.Main;

public class DotnetBuild
{
    /// <summary>
    /// Wrapper for running dotnet build with the same settings in both targets, Returns stream output lines as individual list items.
    /// </summary>
    public ProcessResult Run(string workingDirectory, bool withNoRestore)
    {
        var buildFlags = withNoRestore ? "--no-restore" : "";
        var result = new ProcessResult { Output = new List<string>() };
        var process = new Process
        {
            StartInfo = new ProcessStartInfo 
            {
                FileName = "dotnet",
                Arguments = $"build {buildFlags}",
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                WorkingDirectory = workingDirectory
            }
        };
        
        process.Start();
        
        while (process.StandardOutput.EndOfStream == false)
            result.Output.Add(process.StandardOutput.ReadLine());

        result.ExitCode = process.ExitCode;
        process.Dispose();

        return result;
    }
}