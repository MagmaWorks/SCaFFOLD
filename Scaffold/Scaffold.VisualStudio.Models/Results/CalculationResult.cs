using Scaffold.VisualStudio.Models.Scaffold;

namespace Scaffold.VisualStudio.Models.Results;

public class CalculationResult<T> where T : IFormula
{
    public string AssemblyQualifiedTypeName { get; set; }
    public CalculationDetail<T> CalculationDetail { get; set; }
    public ErrorDetail Failure { get; set; }
    public bool IsSuccess => Failure == null;
}