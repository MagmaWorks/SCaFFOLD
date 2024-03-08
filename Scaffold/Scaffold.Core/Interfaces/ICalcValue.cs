using Scaffold.Core.Enums;

namespace Scaffold.Core.Interfaces;

public interface ICalcValue
{
    IoDirection Direction { get; }
    string Name { get; }
    string Symbol { get; }
    OasysUnits.IQuantity Unit { get; set; }
    CalcStatus Status { get; set; }
        
    void SetValue(string strValue);
    string ToString();
}