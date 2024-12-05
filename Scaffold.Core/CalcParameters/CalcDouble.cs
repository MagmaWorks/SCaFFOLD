using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcValues;

public sealed class CalcDouble : ACalculationParameter<double>
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
}
