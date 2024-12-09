using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.Abstract;

public abstract class CalcQuantity<T> : ICalcQuantity<T> where T : IQuantity
{
    public T Quantity { get; set; }
    public string Unit => Quantity.ToString().Split(' ')[1];
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
    public static implicit operator double(CalcQuantity<T> value) => value.Value;

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

    public string ValueAsString() => ToString();
    public override string ToString() => Quantity.ToString().Replace(" ", string.Empty);

    internal static (string name, string symbol, U unit) OperatorMetadataHelper<U>(
        CalcQuantity<T> x, CalcQuantity<T> y, char operation) where U : Enum
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}{operation}{y.DisplayName}";
        string symbol = x.Symbol == y.Symbol ? x.Symbol : string.Empty;
        U unit = (U)x.Quantity.Unit;
        return (name, symbol, unit);
    }
}
