using OasysUnits;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.Core.CalcValues
{
    public class CalcSIQuantity : ICalcSIQuantity
    {
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

        string ICalcValue.ToString() // redundant!
        {
            return this.ToString(); 
        }
    }
}
