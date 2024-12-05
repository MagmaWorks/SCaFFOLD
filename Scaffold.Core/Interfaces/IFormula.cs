using Scaffold.Core.Images.Interfaces;
using Scaffold.Core.Enums;

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
