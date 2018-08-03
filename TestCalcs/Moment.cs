using CalcCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestCalcs
{
    public class Moment : CalcBase
    {
        CalcDouble span;
        CalcDouble udl;
        CalcDouble moment;
        CalcDouble shear;

        public Moment()
        {
            span = this.inputValues.CreateDoubleCalcValue("span", "l", "m", 10);
            udl = this.inputValues.CreateDoubleCalcValue("udl", "w", "kN/m", 5);
            moment = this.outputValues.CreateDoubleCalcValue("moment", "M", "kNm", 0);
            shear = this.outputValues.CreateDoubleCalcValue("Shear", "V", "kN", 0);
            UpdateCalc();
        }

        public Moment(double span, double udl)
        {
            this.span = this.inputValues.CreateDoubleCalcValue("span", "l", "m", span);
            this.udl = this.inputValues.CreateDoubleCalcValue("udl", "w", "kN/m", udl);
            moment = this.outputValues.CreateDoubleCalcValue("moment", "M", "kNm", 0);
            shear = this.outputValues.CreateDoubleCalcValue("Shear", "V", "kN", 0);
            UpdateCalc();
        }

        public override void UpdateCalc()
        {
            moment.Value = (udl.Value * Math.Pow(span.Value, 2)) / 8;
            shear.Value = (udl.Value * span.Value) / 2;
        }

    }
}
