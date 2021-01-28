using System;
using System.Collections.Generic;
using System.Text;

namespace CalcCore
{
    public class CalcString : CalcCore.CalcValueBase
    {
        public string Value { get; set; }

        public override string ValueAsString { get => Value; set => Value = value; }

        public override CalcValueType Type { get { return CalcValueType.STRING; } }

        public CalcString(string name, string symbol, string unit, string value)
        {
            this.Name = name;
            this.Symbol = symbol;
            this.Unit = unit;
            this.Value = value;
        }
    }
}
