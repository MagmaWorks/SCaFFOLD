using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcCore;

namespace TestCalcs
{
    class NewCalc : CalcBase
    {
        CalcDouble test1;
        CalcDouble test2;
        CalcDouble test3;

        public NewCalc()
        {
            initialise();
        }

        void initialise()
        {
            this.test1 = this.inputValues.CreateDoubleCalcValue("toto","l","m",4);
            this.test2 = this.inputValues.CreateDoubleCalcValue("tata", "k", "f", 6);
            this.test3 = this.outputValues.CreateDoubleCalcValue("tata", "k", "f", 6);
            UpdateCalc();
        }

        public override void UpdateCalc()
        {
            formulae = null;
            test3.Value = test1.Value + test2.Value;
        }

        public override List<Formula> GenerateFormulae()
        {
            return new List<Formula>();
        }

    }
}
