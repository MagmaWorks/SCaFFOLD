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
        Symbol = symbol;
        Unit = unit;
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

    public static CalcDouble operator +(CalcDouble c1, CalcDouble c2)
    {
        return new CalcDouble(c1.Value + c2.Value);
    }

    public static CalcDouble operator -(CalcDouble c1, CalcDouble c2)
    {
        return new CalcDouble(c1.Value - c2.Value);
    }

    public static CalcDouble operator *(CalcDouble c1, CalcDouble c2)
    {
        return new CalcDouble(c1.Value * c2.Value);
    }

    public static CalcDouble operator /(CalcDouble c1, CalcDouble c2)
    {
        return new CalcDouble(c1.Value / c2.Value);
    }
}
