using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public abstract class CalcBase : ICalc
    {
        protected CalcValueFactory inputValues = new CalcValueFactory();

        protected CalcValueFactory outputValues = new CalcValueFactory();

        public string InstanceName{ get; set; }

        public string TypeName { get { return ""; } }

        public List<CalcValueBase> GetInputs()
        {
            return inputValues.GetValues();
        }

        public List<CalcValueBase> GetOutputs()
        {
            return outputValues.GetValues();
        }

        public abstract void UpdateCalc();
    }
}
