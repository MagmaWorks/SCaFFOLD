using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public abstract class CalcValueBase
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Unit { get; set; }
        public abstract string ValueAsString { get; set; }
        public CalcStatus Status { get; set; }
        public abstract CalcValueType Type { get; }
        // description
    }

    public enum CalcValueType
    {
        DOUBLE,
        SELECTIONLIST,
        FILEPATH,
        DOUBLELIST
    }

    public enum CalcStatus
    {
        NONE,
        PASS,
        JUSTPASS,
        JUSTFAIL,
        FAIL
    }
}
