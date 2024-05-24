using OasysUnits;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.Core.CalcValues;

public class CalcSiQuantity : ICalcSIQuantity
{
    private IQuantity _quantity;

    IQuantity ICalcSIQuantity.Quantity => _quantity;
    string ICalcQuantity.Unit => _quantity.Unit.ToString(); 

    double ICalcQuantity.Value
    {
        get => _quantity.Value;
        set => _quantity = Quantity.From(value, _quantity.Unit);
    }

    CalcStatus ICalcValue.Status { get; set; }
    IoDirection ICalcValue.Direction { get; }

    string _displayName;
    string ICalcValue.DisplayName => _displayName;
    string _symbol;
    string ICalcValue.Symbol => _symbol;

    public CalcSiQuantity(IQuantity quantity, string name, string symbol)
    {
        _quantity = quantity;
        _displayName = name;
        _symbol = symbol;
    }

    string ICalcValue.GetValue(string format)
        => _quantity.Value.ToString(CultureInfo.InvariantCulture);

    void ICalcValue.SetValue(string strValue)
    {
        _quantity = Quantity.From(double.TryParse(strValue, out var convertedValue) 
            ? convertedValue 
            : double.NaN, _quantity.Unit); //discuss expected behaviour here. is it better to throw an exception?
    }
}