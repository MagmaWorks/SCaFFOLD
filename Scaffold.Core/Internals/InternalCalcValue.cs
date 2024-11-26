using System.ComponentModel;
using OasysUnits;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.Internals;

internal sealed class InternalCalcValue : ICalcValue
{
    private ICalculation Calculation { get; }
    private Type ValueType { get; }
    private string MemberName { get; }

    public string DisplayName { get; set; }
    public string Symbol { get; set; }
    public string UnitName => Unit?.QuantityInfo.Name ?? "";
    public CalcStatus Status { get; set; }

    public InternalCalcValue(ICalculation calculation, Type valueType, string memberName)
    {
        Calculation = calculation;
        ValueType = valueType;
        MemberName = memberName;
    }


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
