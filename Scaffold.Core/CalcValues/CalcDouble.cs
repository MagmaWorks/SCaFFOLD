using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcValues;

public sealed class CalcDouble : CalcValue<double>
{
    public CalcDouble(double value) : this(null, value) { }

    public CalcDouble(string name, double value) : base(name)
        => Value = value;

    public CalcDouble(string name, string symbol, double value, string unit = "")
        : this(name, value)
    {
        Symbol = symbol?.Trim();
        Unit = unit?.Trim();
    }

    public override bool TryParse(string strValue)
    {
        if (double.TryParse(strValue, out double result))
        {
            Value = result;
            return true;
        }

        return false;
    }

    public static implicit operator double(CalcDouble value) => value.Value;

    public static CalcDouble operator +(CalcDouble x, CalcDouble y)
    {
        (string name, string symbol, string unit) = OperatorMetadataHelper(x, y, '+');
        return new CalcDouble(name, symbol, x.Value + y.Value, unit);
    }

    public static CalcDouble operator -(CalcDouble x, CalcDouble y)
    {
        (string name, string symbol, string unit) = OperatorMetadataHelper(x, y, '-');
        return new CalcDouble(name, symbol, x.Value - y.Value, unit);
    }

    public static CalcDouble operator *(CalcDouble x, CalcDouble y)
    {
        (string name, string symbol, string unit) = OperatorMetadataHelper(x, y, '*');
        if (!string.IsNullOrEmpty(unit))
        {
            unit = $"{unit}²";
        }

        return new CalcDouble(name, symbol, x.Value * y.Value, unit);
    }

    public static CalcDouble operator /(CalcDouble x, CalcDouble y)
    {
        (string name, string symbol, string unit) = OperatorMetadataHelper(x, y, '/');
        return new CalcDouble(name, symbol, x.Value / y.Value);
    }

    private static (string name, string symbol, string unit) OperatorMetadataHelper(CalcDouble x, CalcDouble y, char operation)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName} {operation} {y.DisplayName}";
        string symbol = x.Symbol == y.Symbol ? x.Symbol : string.Empty;
        string unit = x.Unit == y.Unit ? x.Unit : string.Empty;
        return (name, symbol, unit);
    }
}
