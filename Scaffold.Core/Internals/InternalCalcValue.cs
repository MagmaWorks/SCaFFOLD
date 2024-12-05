using System.ComponentModel;
using System.Reflection;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.Internals;

internal sealed class InternalCalcValue : ICalculationParameter<object>
{
    private ICalculation Calculation { get; }
    private Type ValueType { get; }
    private string MemberName { get; }
    public string Unit { get; }
    public string DisplayName { get; set; }
    public string Symbol { get; set; }
    public object Value
    {
        get
        {
            PropertyInfo member = Calculation.GetType().GetProperty(MemberName);
            return member?.GetValue(Calculation);
        }
    }

    public InternalCalcValue(ICalculation calculation, Type valueType, string memberName)
    {
        Calculation = calculation;
        ValueType = valueType;
        MemberName = memberName;
    }

    private object Convert(string input)
    {
        var converter = TypeDescriptor.GetConverter(ValueType);
        return converter.ConvertFromString(input);
    }

    public bool TryParse(string strValue)
    {
        object newValue = Convert(strValue);
        PropertyInfo member = Calculation.GetType().GetProperty(MemberName);
        if (member != null)
        {
            member?.SetValue(Calculation, newValue);
        }

        return member != null;
    }

    public string ValueAsString() => Value.ToString();
}
