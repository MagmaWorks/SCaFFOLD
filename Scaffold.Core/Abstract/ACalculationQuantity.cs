using OasysUnits;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.CalcValues;

public abstract class ACalculationQuantity<T> : ICalculationQuantityParameter<T> where T : IQuantity
{
    public string Unit => Value.ToString().Replace(Value.Value.ToString(), string.Empty).Trim();
    public string DisplayName { get; set; }
    public string Symbol { get; set; }
    public T Value { get; private set; }

    public ACalculationQuantity(T quantity, string name, string symbol)
    {
        Value = quantity;
        DisplayName = name;
        Symbol = symbol;
    }

    public bool TryParse(string strValue)
    {
        if (Quantity.TryParse(
            Value.QuantityInfo.ValueType, strValue, out IQuantity quantity))
        {
            Value = (T)quantity;
            return true;
        }

        return false;
    }

    public string ValueAsString() => Value.ToString().Replace(" ", string.Empty);
}
