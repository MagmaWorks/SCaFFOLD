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
}
