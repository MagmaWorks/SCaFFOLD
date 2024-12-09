using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcValues;

public sealed class CalcDouble : CalcValue<double>
{
    public CalcDouble(double value)
        : base(value, null, string.Empty, string.Empty) { }

    public CalcDouble(double value, string name)
        : base(value, name, string.Empty, string.Empty) { }

    public CalcDouble(double value, string name, string symbol, string unit = "")
        : base(value, name, symbol, unit) { }

    public static CalcDouble operator +(CalcDouble x, CalcDouble y)
    {
        (string name, string symbol, string unit) = OperatorMetadataHelper(x, y, '+');
        return new CalcDouble(x.Value + y.Value, name, symbol, unit);
    }

    public static CalcDouble operator -(CalcDouble x, CalcDouble y)
    {
        (string name, string symbol, string unit) = OperatorMetadataHelper(x, y, '-');
        return new CalcDouble(x.Value - y.Value, name, symbol, unit);
    }

    public static CalcDouble operator *(CalcDouble x, CalcDouble y)
    {
        (string name, string symbol, string unit) = OperatorMetadataHelper(x, y, '·');
        if (!string.IsNullOrEmpty(unit))
        {
            unit = $"{unit}²";
        }

        return new CalcDouble(x.Value * y.Value, name, symbol, unit);
    }

    public static CalcDouble operator /(CalcDouble x, CalcDouble y)
    {
        (string name, string symbol, string _) = OperatorMetadataHelper(x, y, '/');
        return new CalcDouble(x.Value / y.Value, name, symbol);
    }
}
