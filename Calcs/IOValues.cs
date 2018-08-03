using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Calcs
{
    public class IOValues : ViewModelBase
    {
        CalcCore.CalcValueBase calcValue;
        CalcCore.ICalc calc;
        CalculationViewModel calcVM;

        public string Value
        {
            get
            {
                return calcValue.ValueAsString;
            }
            set
            {
                calcValue.ValueAsString = value;
                calc.UpdateCalc();
                MessageBox.Show(calc.GetOutputs()[0].ValueAsString);
                RaisePropertyChanged(nameof(Value));
                calcVM.UpdateOutputs();
            }
        }

        public string Symbol
        {
            get
            {
                return calcValue.Symbol;
            }
        }

        public string Units
        {
            get
            {
                return calcValue.Unit;
            }
        }

        public IOValues(CalcCore.CalcValueBase calcValue, CalcCore.ICalc calc, CalculationViewModel calcVM)
        {
            this.calcValue = calcValue;
            this.calc = calc;
            this.calcVM = calcVM;
        }
    }
}
