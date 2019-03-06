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

        protected List<Formula> formulae;

        public string InstanceName { get; set; } = "";

        public string ClassName
        {
            get
            {
                return this.GetType().ToString();
            }
        }

        protected string _typeName;
        public string TypeName
        {
            get
            {
                return _typeName ?? (_typeName = this.GetType().ToString()); 
            }
        }

        public List<Formula> GetFormulae()
        {
            return formulae ?? (formulae = GenerateFormulae());
        }

        public abstract List<Formula> GenerateFormulae();

        public List<CalcValueBase> GetInputs()
        {
            return inputValues.GetValues();
        }

        public List<CalcValueBase> Inputs
        {
            get
            {
                return inputValues.GetValues();
            }
        }

        public List<CalcValueBase> GetOutputs()
        {
            return outputValues.GetValues();
        }

        public abstract void UpdateCalc();
    }
}
