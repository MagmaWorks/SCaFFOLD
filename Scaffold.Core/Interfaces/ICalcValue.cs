using Scaffold.Core.Enums;

namespace Scaffold.Core.Interfaces;

public interface ICalcValue
{
    string DisplayName { get; set; }
    string Symbol { get; set; }
    CalcStatus Status { get; set; }
    void SetValue(string strValue);
    string GetValue(string format = "");
}
