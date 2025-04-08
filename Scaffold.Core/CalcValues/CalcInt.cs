using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcValues;

public sealed class CalcInt : CalcValue<int>
{
    public CalcInt(int value)
        : base(value, null, string.Empty, string.Empty) { }

    public CalcInt(int value, string name)
        : base(value, name, string.Empty, string.Empty) { }

    public CalcInt(int value, string name, string symbol, string unit = "")
        : base(value, name, symbol, unit) { }

    public static bool operator >(CalcInt value, CalcInt other)
    {
        value.Unit = other.Unit;
        return value.Value > other.Value;
    }

    public static bool operator <(CalcInt value, CalcInt other)
    {
        return value.Value < other.Value;
    }

    public static bool operator >(CalcInt value, CalcDouble other)
    {
        return value.Value > other.Value;
    }

    public static bool operator >(CalcInt value, double other)
    {
        return value.Value > other;
    }

    public static bool operator <(CalcInt value, CalcDouble other)
    {
        return value.Value < other.Value;
    }

    public static bool operator <(CalcInt value, double other)
    {
        return value.Value < other;
    }

    public static bool operator >=(CalcInt value, CalcInt other)
    {
        return value.Value >= other.Value;
    }

    public static bool operator >=(CalcInt value, int other)
    {
        return value.Value >= other;
    }

    public static bool operator <=(CalcInt value, CalcInt other)
    {
        return value.Value <= other.Value;
    }

    public static bool operator <=(CalcInt value, int other)
    {
        return value.Value <= other;
    }

    public static bool operator >=(CalcInt value, CalcDouble other)
    {
        return value.Value >= other.Value;
    }

    public static bool operator >=(CalcInt value, double other)
    {
        return value.Value >= other;
    }

    public static bool operator <=(CalcInt value, CalcDouble other)
    {
        return value.Value <= other.Value;
    }

    public static bool operator <=(CalcInt value, double other)
    {
        return value.Value <= other;
    }

    public static CalcInt operator +(CalcInt x, CalcInt y)
    {
        (string name, string symbol, string unit) = OperatorMetadataHelper(x, y, '+');
        return new CalcInt(x.Value + y.Value, name, symbol, unit);
    }

    public static CalcInt operator -(CalcInt x, CalcInt y)
    {
        (string name, string symbol, string unit) = OperatorMetadataHelper(x, y, '-');
        return new CalcInt(x.Value - y.Value, name, symbol, unit);
    }

    public static CalcInt operator *(CalcInt x, CalcInt y)
    {
        (string name, string symbol, string unit) = OperatorMetadataHelper(x, y, '·');
        if (!string.IsNullOrEmpty(unit))
        {
            unit = $"{unit}²";
        }

        return new CalcInt(x.Value * y.Value, name, symbol, unit);
    }

    public static CalcInt operator /(CalcInt x, CalcInt y)
    {
        (string name, string symbol, string _) = OperatorMetadataHelper(x, y, '/');
        return new CalcInt(x.Value / y.Value, name, symbol);
    }

    public static CalcInt operator *(CalcInt x, int y)
    {
        return new CalcInt(x.Value * y, x.DisplayName, x.Symbol, x.Unit);
    }

    public static CalcDouble operator /(CalcInt x, int y)
    {
        return new CalcDouble(x.Value / y, x.DisplayName, x.Symbol, x.Unit);
    }
}
