using OasysUnits;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using System.Globalization;

namespace Scaffold.Core.CalcValues;

public class CalcSiQuantity : ICalcSIQuantity
{
    private IQuantity _quantity;
    private string _displayName;
    private string _symbol;

    public CalcSiQuantity(IQuantity quantity, string name, string symbol)
    {
        _quantity = quantity;
        _displayName = name;
        _symbol = symbol;
    }

    CalcStatus ICalcValue.Status { get; set; }
    
    string ICalcValue.DisplayName
    {
        get => _displayName;
        set => _displayName = value;
    }

    string ICalcValue.Symbol
    {
        get => _symbol;
        set => _symbol = value;
    }

    IQuantity ICalcSIQuantity.Quantity => _quantity;
    string ICalcQuantity.Unit => _quantity.Unit.ToString(); 
    
    double ICalcQuantity.Value
    {
        get => _quantity.Value;
        set => _quantity = Quantity.From(value, _quantity.Unit);
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