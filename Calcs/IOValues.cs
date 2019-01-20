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
        ICalcViewParent calcVM;

        public string Type
        {
            get
            {
                return calcValue.Type.ToString();
            }
        }

        bool _sliderEntry = false;
            public bool SliderEntry
        {
            get
            {
                return _sliderEntry;
            }
            set
            {
                _sliderEntry = value;
                RaisePropertyChanged(nameof(SliderEntry));
                RaisePropertyChanged(nameof(MinVal));
                RaisePropertyChanged(nameof(MaxVal));
            }
        }

        double _maxVal;
        public double MaxVal { get { return _maxVal; } set { _maxVal = value; RaisePropertyChanged(nameof(MaxVal)); } }
        double _minVal;
        public double MinVal { get { return _minVal; } set { _minVal = value; RaisePropertyChanged(nameof(MinVal)); } }

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
                RaisePropertyChanged(nameof(Value));
                RaisePropertyChanged(nameof(ValueRounded));
                calcVM.UpdateOutputs();
            }
        }

        public string ValueRounded
        {
            get
            {
                if (calcValue.Type == CalcCore.CalcValueType.DOUBLE)
                {
                    double val = (calcValue as CalcCore.CalcDouble).Value;
                    if (val >= 1000)
                        return val.ToString("N0");
                    else
                        return val.ToString("G3");
                }
                return calcValue.ValueAsString;
            }
        }

        public List<string> _selectionList;

        public List<string> SelectionList
        {
            get
            {
                return _selectionList;
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

        public string Name
        {
            get
            {
                return calcValue.Name;
            }
        }

        public IOValues(CalcCore.CalcValueBase calcValue, CalcCore.ICalc calc, ICalcViewParent calcVM)
        {
            this.calcValue = calcValue;
            this.calc = calc;
            this.calcVM = calcVM;
            switch (calcValue.Type)
            {
                case CalcCore.CalcValueType.DOUBLE:
                    MaxVal = Math.Max((calcValue as CalcCore.CalcDouble).Value * 5,0);
                    MinVal = Math.Min(0, (calcValue as CalcCore.CalcDouble).Value) ;
                    break;
                case CalcCore.CalcValueType.SELECTIONLIST:
                    _selectionList = (calcValue as CalcCore.CalcSelectionList).SelectionList;
                    break;
                case CalcCore.CalcValueType.FOLDERPATH:
                    break;
                default:
                    break;
            }
        }
    }
}
