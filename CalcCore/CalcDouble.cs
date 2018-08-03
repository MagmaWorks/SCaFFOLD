using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public class CalcDouble : CalcValueBase
    {
        public double Value { get; set; }

        public override string ValueAsString
        {
            get
            {
                return Value.ToString();
            }
            set
            {
                if (Double.TryParse(value, out double result))
                    Value = result;
            }
        }
        
        public CalcDouble(string name, string symbol, string unit, double value)
        {
            this.Name = name;
            this.Symbol = symbol;
            this.Unit = unit;
            this.Value = value;
        }

    }
}
