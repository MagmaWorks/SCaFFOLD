using Scaffold.Core.Images.Interfaces;

namespace Scaffold.Core.Interfaces;

public interface IFormula : ICalculationStatus
{
    string Markdown { get; }
    ICalcImage Image { get; }
}
