using OasysUnits;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.Abstract;

public abstract class CalcQuantity<T> : ICalcQuantity<T> where T : IQuantity
{
    public T Quantity { get; set; }
    public string Unit => Quantity.ToString().Replace(Value.ToString(), string.Empty).Trim();
    public string DisplayName { get; set; }
    public string Symbol { get; set; }
    public CalcStatus Status { get; set; }
    public double Value => (double)Quantity.Value;

    public CalcQuantity(T quantity, string name, string symbol)
    {
        Quantity = quantity;
        DisplayName = name;
        Symbol = symbol;
    }

    public static implicit operator T(CalcQuantity<T> value) => value.Quantity;

    public bool TryParse(string strValue)
    {
        if (OasysUnits.Quantity.TryParse(
            Quantity.QuantityInfo.ValueType, strValue, out IQuantity quantity))
        {
            Quantity = (T)quantity;
            return true;
        }

        return false;
    }

    public string ValueAsString() => Quantity.ToString().Replace(" ", string.Empty);

}
