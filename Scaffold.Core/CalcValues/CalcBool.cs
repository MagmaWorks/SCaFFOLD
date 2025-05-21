﻿using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcValues;

public class CalcBool : CalcValue<bool>
{
    public CalcBool(bool value = false)
        : base(value, null, string.Empty, string.Empty) { }

    public CalcBool(bool value, string name)
        : base(value, name, string.Empty, string.Empty) { }

    public CalcBool(bool value, string name, string symbol, string unit = "")
        : base(value, name, symbol, unit) { }

    public static implicit operator CalcBool(bool value) => new CalcBool(value, string.Empty);
    public static CalcBool operator &(CalcBool x, CalcBool y)
    {
        (string name, string symbol, string _) = OperatorMetadataHelper(x, y, '&');
        return new CalcBool(x.Value & y.Value, name, symbol);
    }

    public static CalcBool operator |(CalcBool x, CalcBool y)
    {
        (string name, string symbol, string _) = OperatorMetadataHelper(x, y, '∨');
        return new CalcBool(x.Value | y.Value, name, symbol);
    }

    public static CalcBool operator ==(CalcBool x, CalcBool y)
    {
        (string name, string symbol, string _) = OperatorMetadataHelper(x, y, '=');
        return new CalcBool(x.Value == y.Value, name, symbol);
    }

    public static CalcBool operator !=(CalcBool x, CalcBool y)
    {
        (string name, string symbol, string _) = OperatorMetadataHelper(x, y, '≠');
        return new CalcBool(x.Value != y.Value, name, symbol);
    }

    public override bool Equals(object obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();
}
