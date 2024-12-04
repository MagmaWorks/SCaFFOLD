using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.Abstract;

public abstract class CalcValue<T> : ICalcValue
{
    public string DisplayName { get; set; }
    public string Symbol { get; set; }
    public CalcStatus Status { get; set; }
    public T Value { get; set; }
    public string Unit { get; set; } = string.Empty;
    public abstract bool TryParse(string strValue);
    public string ValueAsString() => $"{Value}{Unit}";

    protected CalcValue(string name)
    {
        DisplayName = name;
    }
}
