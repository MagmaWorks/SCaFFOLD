using CalcCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calcs
{
    public class CalcsToPrintVM : ViewModelBase
    {
        public ObservableCollection<CalcToPrint> Calcs { get; set; }

        public CalcsToPrintVM(List<ICalc> calcs)
        {
            Calcs = new ObservableCollection<CalcToPrint>();
            foreach (var calc in calcs)
            {
                Calcs.Add(new CalcToPrint(calc, true));
            }
        }

        public bool Print { get; set; } = false;
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
