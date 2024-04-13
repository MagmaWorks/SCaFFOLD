using Scaffold.Core.Enums;

namespace Scaffold.Core.Interfaces;

public interface ICalcValue
{
    IoDirection Direction { get; }
    string DisplayName { get; }
    string Symbol { get; }
    CalcStatus Status { get; set; }        
    void SetValueAsString(string strValue);
    string GetValueAsString(string format = "");
    string ToString();
}