using System;
using System.Collections.Generic;
using System.Text;

namespace CalcCore
{
    public class CalcBool : CalcCore.CalcValueBase
    {
        public bool Value { get; set; }

        public override string ValueAsString
        {
            get
            {
                if (Value)
                    return "True";
                else
                    return "False";
            }
            set
            {
                if (value.ToLower() == "true")
                {
                    Value = true;
                }
                else if (value.ToLower() == "false")
                {
                    Value = false;
                }
            }
        }

        public override CalcValueType Type { get { return CalcValueType.BOOL; } }

        public CalcBool(string name, string symbol, string unit, bool value)
        {
            this.Name = name;
            this.Symbol = symbol;
            this.Unit = unit;
            this.Value = value;
        }
    }
}
