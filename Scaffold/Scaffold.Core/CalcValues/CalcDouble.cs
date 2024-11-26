﻿using System.Globalization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcValues;

public sealed class CalcDouble : CalcValue<double> 
{
    public CalcDouble(double value) : this(null, value) {}
    
    public CalcDouble(string name, double value) : base(name)
        => Value = value;
    
    public CalcDouble(string name, string symbol, OasysUnits.IQuantity unit, double value) 
        : this(name, value)
    {
        Symbol = symbol;
        Unit = unit;
    }
        
    public override void SetValue(string strValue)
    {
        var parsedSuccessfully = double.TryParse(strValue, out var result);
        if (parsedSuccessfully == false)
            throw new ArgumentException($"Failed to parse {strValue} to a double");

        Value = result;
    }

    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}