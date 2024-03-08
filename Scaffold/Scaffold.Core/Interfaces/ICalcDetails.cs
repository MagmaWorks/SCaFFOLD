using Scaffold.Core.Enums;

namespace Scaffold.Core.Interfaces
{
    public interface ICalcDetails
    {
        string Name { get; set; }
        string Symbol { get; set; }
        OasysUnits.IQuantity Unit { get; set; }
        IoDirection Group { get; set; }
        CalcStatus Status { get; set; }
        
        CalcValueType GetCalcType();
        void SetValueFromString(string strValue);
    }
}