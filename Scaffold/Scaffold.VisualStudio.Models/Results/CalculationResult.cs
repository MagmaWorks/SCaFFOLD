using Scaffold.VisualStudio.Models.Scaffold;

namespace Scaffold.VisualStudio.Models.Results;

public class CalculationResult
{
    public string AssemblyQualifiedTypeName { get; set; }
    public CalculationDetail CalculationDetail { get; set; }
    public ErrorDetail Failure { get; set; }
    public bool IsSuccess => Failure == null;
}