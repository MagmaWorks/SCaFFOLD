using CalcCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Calcs
{
    public abstract class CalcPluginBase
    {
        protected ICalc _calc;
        public CalcCore.ICalc Calc { get => _calc; }
        public abstract UserControl Control { get;  }

        public void CopyValuesBack(ICalc calc)
        {
            var calcInputs = calc.GetInputs();
            var sourceInputs = Calc.GetInputs();
            foreach (var input in calcInputs)
            {
                var inputToCopy = sourceInputs.Where(a => a.Name == input.Name).First();
                input.ValueAsString = inputToCopy.ValueAsString;
            }
        }

        public void CopyValuesFrom(ICalc calc)
        {
            var calcInputs = calc.GetInputs();
            var destinationInputs = Calc.GetInputs();
            foreach (var input in destinationInputs)
            {
                var inputToCopy = calcInputs.Where(a => a.Name == input.Name).First();
                input.ValueAsString = inputToCopy.ValueAsString;
            }
        }

        public abstract void Initialise(ICalc calc);

        public abstract void Update();
    }
}
