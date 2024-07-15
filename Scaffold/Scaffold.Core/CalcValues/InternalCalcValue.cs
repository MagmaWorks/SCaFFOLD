using System.ComponentModel;
using OasysUnits;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.CalcValues;

internal sealed class InternalCalcValue(ICalculation calculation, Type valueType, string memberName) : ICalcValue
{
    private ICalculation Calculation { get; } = calculation;
    private Type ValueType { get; } = valueType;
    private string MemberName { get; } = memberName;

    public string DisplayName { get; set; }
    public string Symbol { get; set; }
    public string UnitName => Unit?.QuantityInfo.Name ?? "";
    public CalcStatus Status { get; set; }
    
    public object Value
    {
        get
        {
            var member = Calculation.GetType().GetProperty(MemberName);
            return member?.GetValue(Calculation);
        }
    }

    public IQuantity Unit { get; set; }
    
    private object Convert(string input)
    {
        var converter = TypeDescriptor.GetConverter(ValueType);
        return converter.ConvertFromString(input);
    }
    
    public void SetValue(string strValue)
    {
        var newValue = Convert(strValue);
        var member = Calculation.GetType().GetProperty(MemberName);
        
        member?.SetValue(Calculation, newValue);
    }

    public string GetValue(string format = "") => Value.ToString();
}