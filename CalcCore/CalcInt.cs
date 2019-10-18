using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public class CalcInt : CalcValueBase
    {
        public int Value { get; set; }

        public override string ValueAsString
        {
            get
            {
                return Value.ToString();
            }
            set
            {
                if (int.TryParse(value, out int result))
                    Value = result;
            }
        }

        public override CalcValueType Type { get { return CalcValueType.INT; } }

        public CalcInt(string name, string symbol, string unit, int value)
        {
            this.Name = name;
            this.Symbol = symbol;
            this.Unit = unit;
            this.Value = value;
        }

    }
}
