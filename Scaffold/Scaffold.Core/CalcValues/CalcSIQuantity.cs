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

        string Unit => quantity.Unit.ToString(); // need to add implementation that converts this to LaTeX

        double ICalcQuantity.Value
        {
            get
            {
                return quantity.Value;
            }
            set
            {
                quantity.Value = value; //need to create new quantity with same units etc as current but with new value
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
                quantity.Value = convertedValue;    //need to create new quantity with same units etc as current but with new value
            else
                quantity.Value = double.NaN; //discuss expected behaviour here. is it better to throw an exception?
            ;
        }

        string ICalcValue.ToString() // redundant!
        {
            return this.ToString(); 
        }
    }
}
