namespace Scaffold.VisualStudio.Models.Results;

public class CalculationAssemblyResult
{
    public List<CalculationResult> Results { get; set; }
    public ErrorDetail RunError { get; set; }
}