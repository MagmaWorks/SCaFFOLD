using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Calcs
{
    public class CalculationViewModel : ViewModelBase
    {
        CalcCore.ICalc calc;
        public TableVM Table { get; set; }
        public ChartVM Chart { get; set; }
        public CrossRefVM CrossRef {get;set;}

        List<IOValues> inputs;
        public List<IOValues> Inputs
        {
            get
            {
                return inputs;
            }
            set
            {
                inputs = value;
            }
        }
        ObservableCollection<IOValues> outputs;
        public ObservableCollection<IOValues> Outputs
        {
            get
            {
                return outputs;
            }
            set
            {
                outputs = value;
                RaisePropertyChanged(nameof(Outputs));
            }
        }

        public CalculationViewModel(CalcCore.ICalc calculation)
        {
            Inputs = new List<IOValues>();
            foreach (var item in calculation.GetInputs())
            {
                Inputs.Add(new IOValues(item, calculation, this));
            }
            Outputs = new ObservableCollection<IOValues>();
            foreach (var item in calculation.GetOutputs())
            {
                Outputs.Add(new IOValues(item, calculation, this));
            }

            this.calc = calculation;
            this.Table = new TableVM(calc);
            this.Chart = new ChartVM(calc);
            this.CrossRef = new CrossRefVM(calc);
        }

        public void UpdateOutputs()
        {
            Outputs = new ObservableCollection<IOValues>();
            foreach (var item in calc.GetOutputs())
            {
                Outputs.Add(new IOValues(item, calc, this));
            }
        }
    }
}