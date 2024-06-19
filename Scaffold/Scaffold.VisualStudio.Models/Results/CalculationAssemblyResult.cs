using Scaffold.VisualStudio.Models.Scaffold;

namespace Scaffold.VisualStudio.Models.Results;

public class CalculationAssemblyResult<T> where T : IFormula
{
    public List<CalculationResult<T>> Results { get; set; }
    public ErrorDetail RunError { get; set; }
}