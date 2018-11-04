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
        public string Author
        {
            get
            {
                return Environment.UserName;
            }
        }
        ObservableCollection<FormulaeVM> _formulae;
        public ObservableCollection<FormulaeVM> Formulae
        {
            get { return _formulae ; }
            set
            {
                _formulae = value;
            }
        }

        public string CalcTypeName
        {
            get
            {
                return calc.TypeName;
            }
        }

        public string CalcInstanceName
        {
            get
            {
                return calc.InstanceName;
            }
            set
            {
                calc.InstanceName = value;
                RaisePropertyChanged(nameof(CalcInstanceName));
            }
        }

        ICommand toWord;

        public ICommand ToWord
        {
            get
            {
                return toWord ?? (toWord = new CommandHandler(() => outputToWord(), true));
            }
        }

        private void outputToWord()
        {
            CalcCore.OutputToWord.WriteToWord(calc);
        }

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
            _formulae = new ObservableCollection<FormulaeVM>();
            foreach (var item in calc.GetFormulae())
            {
                _formulae.Add(new FormulaeVM() { Expression = item.Expression, Ref = item.Ref, Conclusion=item.Conclusion, Narrative=item.Narrative, Status=item.Status });
            }
        }

        public void UpdateOutputs()
        {
            Outputs = new ObservableCollection<IOValues>();
            foreach (var item in calc.GetOutputs())
            {
                Outputs.Add(new IOValues(item, calc, this));
            }
            _formulae = new ObservableCollection<FormulaeVM>();
            foreach (var item in calc.GetFormulae())
            {
                _formulae.Add(new FormulaeVM() { Expression = item.Expression, Ref = item.Ref, Conclusion = item.Conclusion, Narrative = item.Narrative, Status = item.Status });
            }
            RaisePropertyChanged(nameof(Formulae));


        }
    }
}