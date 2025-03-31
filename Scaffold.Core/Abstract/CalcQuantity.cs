using System.Globalization;
using OasysUnits;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.Abstract;

public abstract class CalcQuantity<T> : ICalcQuantity<T>, IEquatable<CalcQuantity<T>> where T : IQuantity
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

    public static bool operator ==(CalcQuantity<T> value, CalcQuantity<T> other)
    {
        return other.Quantity.As(value.Quantity.Unit).Equals(value.Value);
    }

    public static bool operator !=(CalcQuantity<T> value, CalcQuantity<T> other)
    {
        return !other.Quantity.As(value.Quantity.Unit).Equals(value.Value);
    }

    public static bool operator >(CalcQuantity<T> value, CalcQuantity<T> other)
    {
        return value.Value > other.Quantity.As(value.Quantity.Unit);
    }

    public static bool operator <(CalcQuantity<T> value, CalcQuantity<T> other)
    {
        return value.Value < other.Quantity.As(value.Quantity.Unit);
    }

    public static bool operator >=(CalcQuantity<T> value, CalcQuantity<T> other)
    {
        return value.Value >= other.Quantity.As(value.Quantity.Unit);
    }

    public static bool operator <=(CalcQuantity<T> value, CalcQuantity<T> other)
    {
        return value.Value <= other.Quantity.As(value.Quantity.Unit);
    }

    public bool TryParse(string strValue)
    {
        try
        {
            IQuantity quantity = OasysUnits.Quantity.Parse(CultureInfo.InvariantCulture, Quantity.QuantityInfo.ValueType, strValue);
            Quantity = (T)quantity;
            return true;
        }
        catch
        {
            return false;
        }
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (ReferenceEquals(obj, null))
        {
            return false;
        }

        if (obj is CalcQuantity<T> other)
        {
            return Equals(other);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return DisplayName.GetHashCode() ^ Symbol.GetHashCode() ^ Status.GetHashCode()
            ^ Value.GetHashCode() ^ Unit.GetHashCode();
    }

    public bool Equals(CalcQuantity<T> other) => Value.Equals(other?.Value) && Unit == other.Unit;
    public string ValueAsString() => ToString();
    public override string ToString() => Quantity.ToString(CultureInfo.InvariantCulture).Replace(" ", "\u2009");

    internal static (string name, string symbol, U unit) OperatorMetadataHelper<U>(
        CalcQuantity<T> x, CalcQuantity<T> y, char operation) where U : Enum
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}\u2009{operation}\u2009{y.DisplayName}";
        string symbol = x.Symbol == y.Symbol ? x.Symbol : string.Empty;
        U unit = (U)x.Quantity.Unit;
        return (name, symbol, unit);
    }
}
