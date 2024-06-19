using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using CSharpMath.SkiaSharp;
using Scaffold.Core.Abstract;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;
using Scaffold.VisualStudio.Models.Main;
using Scaffold.VisualStudio.Models.Results;
using Scaffold.VisualStudio.Models.Scaffold;
using SkiaSharp;

namespace Scaffold.VisualStudio.Calculator;

internal static class Program
{
    private static string RunId { get; set; }
    private static ProjectDetails ProjectDetails { get; set; }
        
    private static void Main(string[] args)
    {
        RunId = Guid.NewGuid().ToString();
        
        DeletePreviousRuns();
        
        var result = new CalculationAssemblyResult<FormulaDetail> {Results = [] };

        if (ArgumentsAreValid(args, result))
            ReadCalculations(result);
        
        Console.Write(JsonSerializer.Serialize(result));
    }

    private static bool ArgumentsAreValid(string[] args, CalculationAssemblyResult<FormulaDetail> result)
    {
        if (args.Length != 1)
            throw new ArgumentException("Project details model must be passed to the calculator.");

        try
        {
            ProjectDetails = JsonSerializer.Deserialize<ProjectDetails>(args[0]);
        }
        catch (Exception ex)
        {
            AssignRunErrorFromException(result, ex);
            return false;
        }
        
        return true;
    }
        
    private static void ReadInstance(CalculationBase instance, CalculationResult<FormulaDetail> result)
    {
        if (instance == null)
        {
            result.Failure = new ErrorDetail { Message = "FAILED: Could not read instance." };
            return;
        }

        result.CalculationDetail = new CalculationDetail<FormulaDetail>
        {
            Title = instance.Title,
            Type = instance.Type,
            Inputs = WriteCalcValueList(instance.GetInputs(), "input"),
            Outputs = WriteCalcValueList(instance.GetOutputs(), "output"),
            Formulae = WriteDisplayFormulae(instance.GetFormulae())
        };
    }

    private static List<CalcValueDetail> WriteCalcValueList(IEnumerable<ICalcValue> calcValues, string imageHeading)
    {
        var output = new List<CalcValueDetail>();

        var i = 0;
        foreach (var value in calcValues)
        {
            var flatObj = new CalcValueDetail
            {
                DisplayName = value.DisplayName,
                Status = value.Status.ToString(),
                Value = value.GetValue()
            };
            
            if (string.IsNullOrEmpty(value.Symbol) == false)
            {
                var directory = $@"{RunDirectory()}\TemporaryImages";
                    
                if (Directory.Exists(directory) == false)
                    Directory.CreateDirectory(directory);

                using var memoryStream = new MemoryStream();
                var painter = new MathPainter { LaTeX = value.Symbol };
                using var png = painter.DrawAsStream();

                png?.CopyTo(memoryStream);

                if (memoryStream.Length > 0)
                {
                    var file = @$"{directory}\{imageHeading}-{i}.png";
                    File.WriteAllBytes(file, memoryStream.ToArray());

                    flatObj.Symbol = file;
                }
            }
            
            output.Add(flatObj);
            i++;
        }

        return output;
    }


    private static List<FormulaDetail> WriteDisplayFormulae(IEnumerable<Formula> formulae)
    {
        var list = new List<FormulaDetail>();
        foreach (var formula in formulae)
        {
            list.Add(new FormulaDetail
            {
                Ref = formula.Ref,
                Narrative = formula.Narrative,
                Conclusion = formula.Conclusion,
                Status = formula.Status.ToString(),
                Expressions = formula.Expression
            });
        }

        return list;
    }

    private static void AssignRunErrorFromException(CalculationAssemblyResult<FormulaDetail> result, Exception ex)
    {
        result.RunError = new ErrorDetail
        {
            Message = ex.Message, 
            InnerException = ex.InnerException?.Message, 
            Source = ex.Source,
            StackTrace = ex.StackTrace
        };
    }

    /// <summary>
    /// Added to delete previous, rather than closing of a single run at a time to reduce the chance
    /// the process fails due to any files being locked.
    /// </summary>
    private static void DeletePreviousRuns()
    {
        var runDirectoryPath = RunDirectoryRoot();
        var runDirectory = new DirectoryInfo(runDirectoryPath);

        if (runDirectory.Exists == false)
            Directory.CreateDirectory(runDirectoryPath);

        foreach (var subDirectory in runDirectory.GetDirectories())
        {
            if (subDirectory.Name == RunId)
                continue;
            
            subDirectory.Delete(true);
        }
    }

    private static string RunDirectoryRoot()
    {
        var assemblyLocation = Globals.GetWorkingDirectory();
        return @$"{assemblyLocation}\ScaffoldRuns";
    }

    private static string RunDirectory() 
        => @$"{RunDirectoryRoot()}\{RunId}";
    
    private static string CopyProjectToLocal()
    {
        var localDirectory = RunDirectory();
        Directory.CreateDirectory(localDirectory);

        CopyDirectory(ProjectDetails.ProjectFilePath, localDirectory);
        
        return localDirectory;
    }
    
    private static void CopyDirectory(string sourceDirectory, string destinationDirectory)
    {
        var sourceInfo = new DirectoryInfo(sourceDirectory);
        var destinationInfo = new DirectoryInfo(destinationDirectory);

        if (sourceInfo.Name is "bin")
            return;

        if (destinationInfo.Exists == false)
            destinationInfo.Create();

        foreach (var file in sourceInfo.GetFiles())
            file.CopyTo(Path.Combine(destinationInfo.FullName, file.Name));

        foreach (var directory in sourceInfo.GetDirectories())
            CopyDirectory(directory.FullName, Path.Combine(destinationInfo.FullName, directory.Name));
    }
    
    private static List<HintPathReplacement> GetHintPathReplacements(string projectFilePath)
    {
        var list = new List<HintPathReplacement>();
        var projectFile = File.ReadAllText(projectFilePath);
        var projectFileInfo = new FileInfo(projectFilePath);
        
        var hintPathRegex = new Regex("<HintPath>.*</HintPath>");
        var hintPaths = hintPathRegex.Matches(projectFile);

        if (hintPaths.Count == 0)
            return list;
        
        foreach (Match hintPath in hintPaths)
        {
            var path = hintPath.Value.Replace("<HintPath>", "").Replace("</HintPath>", "");
            var hintMatches = Regex.Matches(path, Regex.Escape("..\\")).Count;
            var hintStr = new StringBuilder();
            
            DirectoryInfo parentDirectory = null;
            
            for (var i = 0; i < hintMatches; i++)
            {
                hintStr.Append(@"..\");
                parentDirectory = projectFileInfo.Directory?.Parent;
            }

            parentDirectory = parentDirectory?.Parent; // 1 additional step required for the path but not for the hint str.

            if (parentDirectory == null)
                throw new ArgumentException("Failed to convert hint path to a realised path.");
            
            list.Add(new HintPathReplacement{HintPath = hintStr.ToString(), ReplacementPath = $@"{parentDirectory.FullName}\"});
        }

        return list;
    }

    private static void WriteHintReplacements(string originalCsProjFile)
    {
        var replacements = GetHintPathReplacements(originalCsProjFile);
        if (replacements.Count == 0) 
            return;
        
        var projectFile = File.ReadAllText(ProjectDetails.CsProjPath());
        
        foreach (var replacement in replacements)
            projectFile = projectFile.Replace(replacement.HintPath, replacement.ReplacementPath);
                
        File.WriteAllText(ProjectDetails.CsProjPath(), projectFile);
    }
    
    private static void ReadCalculations(CalculationAssemblyResult<FormulaDetail> assemblyResult)
    {
        try
        {
            if (Directory.Exists(ProjectDetails.ProjectFilePath) == false)
            {
                assemblyResult.RunError = new ErrorDetail {Message = "The supplied path does not exist - ensure you have a tab open within the project you want to read." };
                return;
            }

            var originalCsProjFile = ProjectDetails.CsProjPath();
            ProjectDetails.ProjectFilePath = CopyProjectToLocal();
            
            WriteHintReplacements(originalCsProjFile);

            var settings = Globals.GetSettings<Settings>();
            var dotnetBuild = new DotnetBuild();
            var buildResult = dotnetBuild.Run(ProjectDetails.ProjectFilePath, settings.DotnetBuildNoRestore);

            if (buildResult.ExitCode != 0)
            {
                assemblyResult.RunError = new ErrorDetail
                {
                    Source = "dotnet build",
                    Message = "Failed to build project.",
                    InnerException = string.Join(",", buildResult.Output)
                };
                return;
            }

            var reader = new BinariesAssemblyReader(ProjectDetails.BinariesPath(), ProjectDetails.PackageName());
            var assembly = reader.GetAssembly();

            if (assembly == null)
            {
                assemblyResult.RunError = new ErrorDetail
                {
                    Source = "Assembly reader", 
                    Message = "Failed to load assembly", 
                    InnerException = ProjectDetails.AssemblyPath()
                };
                
                return;
            }
            
            foreach (var calculation in assembly.GetCalculationTypes())
            {
                if (calculation.FullName == null)
                    continue;
            
                var calculationResult = new CalculationResult<FormulaDetail> {AssemblyQualifiedTypeName = calculation.FullName};
                var instance = (CalculationBase)assembly.CreateInstance(calculation.FullName);
            
                instance?.LoadIoCollections();
                ReadInstance(instance, calculationResult);

                assemblyResult.Results.Add(calculationResult);
            }
        }
        catch (Exception ex)
        {
            AssignRunErrorFromException(assemblyResult, ex);
        }
    }
}