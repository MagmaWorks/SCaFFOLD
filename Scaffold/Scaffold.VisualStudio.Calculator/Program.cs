using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Scaffold.Core.Abstract;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;
using Scaffold.VisualStudio.Models;
using Scaffold.VisualStudio.Models.Results;
using Scaffold.VisualStudio.Models.Scaffold;

namespace Scaffold.VisualStudio.Calculator;

internal static class Program
{
    private static string RunId { get; set; }
    private static ProjectDetails ProjectDetails { get; set; }
        
    private static void Main(string[] args)
    {
        RunId = Guid.NewGuid().ToString();
        
        DeletePreviousRuns();
        
        var result = new CalculationAssemblyResult {Results = [] };

        if (ArgumentsAreValid(args, result))
            ReadCalculations(result);
        
        Console.Write(JsonSerializer.Serialize(result));
    }

    private static bool ArgumentsAreValid(string[] args, CalculationAssemblyResult result)
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
        
    private static void ReadInstance(CalculationBase instance, CalculationResult result)
    {
        if (instance == null)
        {
            result.Failure = new ErrorDetail { Message = "FAILED: Could not read instance." };
            return;
        }

        result.CalculationDetail = new CalculationDetail
        {
            Title = instance.Title,
            Type = instance.Type,
            Inputs = WriteCalcValueList(instance.GetInputs()),
            Outputs = WriteCalcValueList(instance.GetOutputs()),
            Formulae = WriteDisplayFormulae(instance.GetFormulae())
        };
    }

    private static List<CalcValueDetail> WriteCalcValueList(IEnumerable<ICalcValue> calcValues)
        => calcValues.Select(value => new CalcValueDetail
        {
            DisplayName = value.DisplayName, 
            Symbol = value.Symbol, 
            Status = value.Status.ToString(), 
            Value = value.GetValue()
        }).ToList();
        

    private static List<IFormula> WriteDisplayFormulae(IEnumerable<Formula> formulae)
    {
        var list = new List<IFormula>();
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

    private static void AssignRunErrorFromException(CalculationAssemblyResult result, Exception ex)
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
        var runDirectoryPath = RunDirectory();
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

    private static string RunDirectory()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyLocation = assembly.Location[..assembly.Location.LastIndexOf("\\", StringComparison.Ordinal)];
        return @$"{assemblyLocation}\ScaffoldRuns";
    }
    
    private static string CopyProjectToLocal()
    {
        var localDirectory = @$"{RunDirectory()}\{RunId}";
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
    
    private static void ReadCalculations(CalculationAssemblyResult assemblyResult)
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
            
            var dotnetBuild = new DotnetBuild();
            var buildResult = dotnetBuild.Run(ProjectDetails.ProjectFilePath);

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
            
                var calculationResult = new CalculationResult {AssemblyQualifiedTypeName = calculation.FullName};
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