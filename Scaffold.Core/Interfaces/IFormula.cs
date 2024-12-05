using Scaffold.Core.Enums;
using Scaffold.Core.Images.Interfaces;

namespace Scaffold.Core.Interfaces;

public interface IFormula
{
    List<string> Expression { get; }
    string Reference { get; }
    string Narrative { get; }
    string Conclusion { get; }
    CalcStatus Status { get; }
    ICalcImage Image { get; }
}
