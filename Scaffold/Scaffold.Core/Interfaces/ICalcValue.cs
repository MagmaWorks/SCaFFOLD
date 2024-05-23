using OasysUnits;
using Scaffold.Core.Enums;

namespace Scaffold.Core.Interfaces;

public interface ICalcValue
{
    IoDirection Direction { get; }
    string DisplayName { get; }
    string Symbol { get; }
<<<<<<< HEAD
    CalcStatus Status { get; set; }        
    void SetValueAsString(string strValue);
    string GetValueAsString(string format = "");
=======
    string UnitName { get; }
    CalcStatus Status { get; set; }    
    void SetValue(string strValue);
    string GetValue(string format = "");
    string ToString();
>>>>>>> 083b9edfd7d25d0ebe5c643348a414e7642378e3
}