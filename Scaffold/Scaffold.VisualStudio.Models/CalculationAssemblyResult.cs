namespace Scaffold.VisualStudio.Models;

public class CalculationAssemblyResult
{
    public List<CalculationResult> Results { get; set; }
    public ErrorDetail RunError { get; set; }
}