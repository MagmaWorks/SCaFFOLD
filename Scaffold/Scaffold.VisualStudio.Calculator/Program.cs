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
    private static string RootPath { get; set; }
    private static string BinariesPath { get; set; }
        
    private static void Main(string[] args)
    {
        if (args.Length != 2)
            throw new ArgumentException(
                "Project root and assembly (bin) root are required parameters.");

        RootPath = args[0];
        BinariesPath = args[1];
            
        var result = ReadCalculations();
        Console.Write(JsonSerializer.Serialize(result));
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
        

    private static List<DisplayFormula> WriteDisplayFormulae(IEnumerable<Formula> formulae)
        => formulae.Select(formula => new DisplayFormula(formula.Expression)
        {
            Ref = formula.Ref, 
            Narrative = formula.Narrative, 
            Conclusion = formula.Conclusion, 
            Status = formula.Status.ToString()
        }).ToList();
        
    private static CalculationAssemblyResult ReadCalculations()
    {
        var result = new CalculationAssemblyResult {Results = [] };

        try
        {
            if (Directory.Exists(RootPath) == false)
            {
                result.RunError = new ErrorDetail {Message = "The supplied path does not exist - ensure you have a tab open within the project you want to read." };
                return result;
            }

            var dotnetBuild = new DotnetBuild();
            var buildResult = dotnetBuild.Run(RootPath);
            
            if (buildResult.ExitCode != 0)
            {
                result.RunError = new ErrorDetail
                {
                    Source = "dotnet build",
                    Message = "Failed to build project - see error detail.", 
                    InnerException = string.Join(",", buildResult.Output)
                };
                return result;
            }
            
            var reader = new BinariesAssemblyReader(BinariesPath);
            var assembly = reader.GetAssembly();

            foreach (var calculation in assembly.GetCalculationTypes())
            {
                if (calculation.FullName == null)
                    continue;

                var calculationResult = new CalculationResult();
                var instance = (CalculationBase)assembly.CreateInstance(calculation.FullName);

                instance?.LoadIoCollections();
                ReadInstance(instance, calculationResult);
            }
        }
        catch (Exception ex)
        {
            result.RunError = new ErrorDetail
            {
                Message = ex.Message, 
                InnerException = ex.InnerException?.Message, 
                Source = ex.Source,
                StackTrace = ex.StackTrace
            };
        }

        return result;
    }
}