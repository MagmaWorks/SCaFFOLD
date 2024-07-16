using OasysUnits;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.Abstract;

public abstract class CalcValue<T> : ICalcValue
{
    protected CalcValue(string name)
    {
        DisplayName = name;
    }
    
    public string DisplayName { get; set; }
    public string Symbol { get; set; }
    public string UnitName => Unit?.QuantityInfo.Name ?? "";
    public CalcStatus Status { get; set; }
    public T Value { get; set; }
    public IQuantity Unit { get; set; }
        
    public abstract void SetValue(string strValue);
    public virtual string GetValue(string format = "") => Value.ToString();
    public abstract override string ToString();
}