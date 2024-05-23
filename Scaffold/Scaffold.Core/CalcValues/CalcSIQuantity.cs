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
<<<<<<< HEAD
        IQuantity quantity;
        IQuantity ICalcSIQuantity.Quantity => quantity;

        string ICalcQuantity.Unit => quantity.Unit.ToString(); 

        double ICalcQuantity.Value
        {
            get
            {
                return quantity.Value;
            }
            set
            {
                quantity = Quantity.From(value, quantity.Unit);
            }
        }

        IoDirection direction;
        IoDirection ICalcValue.Direction => direction;

        string displayName;
        string ICalcValue.DisplayName => displayName;

        string symbol;
        string ICalcValue.Symbol => symbol;

        CalcStatus status;
        CalcStatus ICalcValue.Status { get => status; set { status = value; } }

        public CalcSIQuantity(IQuantity quantity, string name,  CalcStatus status)
        {
            this.quantity = quantity;
            this.displayName = name;
            this.status = status;
        }

        string ICalcValue.GetValueAsString(string format)
        {
            {
                return quantity.Value.ToString();
            };
        }

        void ICalcValue.SetValueAsString(string strValue)
        {
            double convertedValue;
            if (double.TryParse(strValue, out convertedValue))
                quantity = Quantity.From(convertedValue, quantity.Unit);  
            else
                quantity = Quantity.From(double.NaN, quantity.Unit); //discuss expected behaviour here. is it better to throw an exception?
            ;
        }
=======
        get => _quantity.Value;
        set => _quantity = Quantity.From(value, _quantity.Unit);
>>>>>>> 083b9edfd7d25d0ebe5c643348a414e7642378e3
    }

    public string UnitName { get; private set; }
    CalcStatus ICalcValue.Status { get; set; }
    IoDirection ICalcValue.Direction { get; }
        
    string ICalcValue.DisplayName { get; }
    string ICalcValue.Symbol { get; }
        
    string ICalcValue.GetValue(string format)
        => _quantity.Value.ToString(CultureInfo.InvariantCulture);

    void ICalcValue.SetValue(string strValue)
    {
        _quantity = Quantity.From(double.TryParse(strValue, out var convertedValue) 
            ? convertedValue 
            : double.NaN, _quantity.Unit); //discuss expected behaviour here. is it better to throw an exception?
    }
}