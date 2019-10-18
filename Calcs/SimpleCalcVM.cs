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
    public class SimpleCalcVM : ViewModelBase, ICalcViewParent
    {
        CalcCore.ICalc calc;
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
            get { return _formulae; }
            set
            {
                _formulae = value;
            }
        }

        bool _includeInputsInWord = true;
        public bool IncludeInputsInWord
        {
            get
            {
                return _includeInputsInWord;
            }
            set
            {
                _includeInputsInWord = value;
                RaisePropertyChanged(nameof(IncludeInputsInWord));
            }
        }

        bool _includeOutputsInWord = true;
        public bool IncludeOutputsInWord
        {
            get
            {
                return _includeOutputsInWord;
            }
            set
            {
                _includeOutputsInWord = value;
                RaisePropertyChanged(nameof(IncludeOutputsInWord));
            }
        }

        bool _includeBodyInWord = true;
        public bool IncludeBodyInWord
        {
            get
            {
                return _includeBodyInWord;
            }
            set
            {
                _includeBodyInWord = value;
                RaisePropertyChanged(nameof(IncludeBodyInWord));
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
            CalcCore.OutputToODT.WriteToODT(calc, _includeInputsInWord, _includeBodyInWord, _includeOutputsInWord);
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

        public SimpleCalcVM(CalcCore.ICalc calc)
        {
            this.calc = calc;

            Inputs = new List<IOValues>();
            foreach (var item in calc.GetInputs())
            {
                Inputs.Add(new IOValues(item, calc, this));
            }
            Outputs = new ObservableCollection<IOValues>();
            foreach (var item in calc.GetOutputs())
            {
                Outputs.Add(new IOValues(item, calc, this));
            }

            //this.calc = calculation;
            _formulae = new ObservableCollection<FormulaeVM>();
            foreach (var item in calc.GetFormulae())
            {
                _formulae.Add(new FormulaeVM() { Expression = item.Expression, Ref = item.Ref, Conclusion = item.Conclusion, Narrative = item.Narrative, Status = item.Status/*, Image=item.Image */});
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
                _formulae.Add(new FormulaeVM() { Expression = item.Expression, Ref = item.Ref, Conclusion = item.Conclusion, Narrative = item.Narrative, Status = item.Status/*, Image=item.Image */});
            }
            RaisePropertyChanged(nameof(Formulae));
        }
    }
}