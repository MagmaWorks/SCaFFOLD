using System.Text.Json;
using Scaffold.Core.Abstract;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;
using Scaffold.VisualStudio.Models;
using Scaffold.VisualStudio.Models.Results;
using Scaffold.VisualStudio.Models.Scaffold;

namespace Scaffold.VisualStudio.Calculator;

internal static class Program
{
    private static ProjectDetails ProjectDetails { get; set; }
        
    private static void Main(string[] args)
    {
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
    
    private static void ReadCalculations(CalculationAssemblyResult assemblyResult)
    {
        try
        {
            if (Directory.Exists(ProjectDetails.ProjectFilePath) == false)
            {
                assemblyResult.RunError = new ErrorDetail {Message = "The supplied path does not exist - ensure you have a tab open within the project you want to read." };
                return;
            }

            // var dotnetBuild = new DotnetBuild();
            // var buildResult = dotnetBuild.Run(ProjectDetails.ProjectFilePath);
            //
            // if (buildResult.ExitCode != 0)
            // {
            //     assemblyResult.RunError = new ErrorDetail
            //     {
            //         Source = "dotnet build",
            //         Message = "Failed to build project - see error detail.", 
            //         InnerException = string.Join(",", buildResult.Output)
            //     };
            //     return;
            // }
            
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
            
                var calculationResult = new CalculationResult();
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