using CalcCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Calcs
{
    public class CalcsToPrintVM : ViewModelBase
    {
        public ObservableCollection<CalcToPrint> Calcs { get; set; }

        bool _includeInputs = true;
        public bool IncludeInputs
        {
            get
            {
                return _includeInputs;
            }
            set
            {
                _includeInputs = value;
                RaisePropertyChanged(nameof(IncludeInputs));
            }
        }
        bool _includeBody = true;
        public bool IncludeBody
        {
            get
            {
                return _includeBody;
            }
            set
            {
                _includeBody = value;
                RaisePropertyChanged(nameof(IncludeBody));
            }
        }
        bool _includeOutputs = true;
        public bool IncludeOutputs
        {
            get
            {
                return _includeOutputs;
            }
            set
            {
                _includeOutputs = value;
                RaisePropertyChanged(nameof(IncludeOutputs));
            }
        }

        public CalcsToPrintVM(List<ICalc> calcs, int selectedCalc)
        {
            Calcs = new ObservableCollection<CalcToPrint>();
            for (int i = 0; i < calcs.Count; i++)
            {
                var calc = calcs[i];
                
                if (i == selectedCalc)
                    Calcs.Add(new CalcToPrint(calc, true));
                else
                    Calcs.Add(new CalcToPrint(calc, false));
            }
        }

        public bool Print { get; set; } = false;

        ICommand selectAllCommand;

        public ICommand SelectAllCommand
        {
            get
            {
                return selectAllCommand ?? (selectAllCommand = new CommandHandler(() => selectAll(), true));
            }
        }

        void selectAll()
        {
            bool valueToSet = false;
            if (Calcs[0].Print == true)
            {
                valueToSet = false;
            }
            else
            {
                valueToSet = true;
            }
            foreach (var item in Calcs)
            {
                item.Print = valueToSet;
            }
        }
    }

    public class CalcToPrint : ViewModelBase
    {
        public ICalc Calc { get; set; }
        bool _print = true;
        public bool Print
        {
            get
            {
                return _print;
            }
            set
            {
                _print = value;
                RaisePropertyChanged(nameof(Print));
            }
        }
        public string TypeName
        {
            get
            {
                return Calc.TypeName;
            }
        }
        public string InstanceName
        {
            get
            {
                return Calc.InstanceName;
            }
        }

        public CalcToPrint(ICalc calc, bool toPrint)
        {
            _print = toPrint;
            Calc = calc;
        }
    }
}
