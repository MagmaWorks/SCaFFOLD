using Scaffold.Core.Images.Interfaces;

namespace Scaffold.Core.Interfaces;

public interface IFormula : ICalculationStatus
{
    List<string> Expression { get; }
    string Reference { get; }
    string Narrative { get; }
    string Conclusion { get; }
    ICalcImage Image { get; }
}
