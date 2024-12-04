using OasysUnits;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.CalcValues;

public abstract class CalcQuantity<TQuantity> : ICalcQuantity<TQuantity> where TQuantity : IQuantity
{
    public TQuantity Quantity { get; set; }
    public string Unit => Quantity.ToString().Replace(Value.ToString(), string.Empty).Trim();
    public string DisplayName { get; set; }
    public string Symbol { get; set; }
    public CalcStatus Status { get; set; }
    public double Value => (double)Quantity.Value;

    public CalcQuantity(TQuantity quantity, string name, string symbol)
    {
        Quantity = quantity;
        DisplayName = name;
        Symbol = symbol;
    }

    public bool TryParse(string strValue)
    {
        if (OasysUnits.Quantity.TryParse(
            Quantity.QuantityInfo.ValueType, strValue, out IQuantity quantity))
        {
            Quantity = (TQuantity)quantity;
            return true;
        }

        return false;
    }

    public string ValueAsString() => Quantity.ToString().Replace(" ", string.Empty);
}
