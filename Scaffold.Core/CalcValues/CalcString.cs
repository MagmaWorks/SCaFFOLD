using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcValues;

public class CalcString : CalcValue<string>
{
    public CalcString(string value)
        : base(value, null, string.Empty, string.Empty) { }

    public CalcString(string value, string name)
        : base(value, name, string.Empty, string.Empty) { }

    public CalcString(string value, string name, string symbol, string unit = "")
        : base(value, name, symbol, unit) { }

    public static implicit operator CalcString(string value) => new CalcString(value, string.Empty);

    public static CalcString operator +(CalcString x, CalcString y)
    {
        (string name, string symbol, string unit) = OperatorMetadataHelper(x, y, '+');
        return new CalcString(x.Value + y.Value, name, symbol, unit);
    }
}
