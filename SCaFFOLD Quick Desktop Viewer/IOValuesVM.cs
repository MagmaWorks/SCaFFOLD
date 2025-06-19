using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Interfaces;

namespace SCaFFOLD_Quick_Desktop_Viewer
{
    public class IOValues : ViewModelBase
    {
        ICalcValue calcValue;
        ICalculation calc;
        ICalcViewParent calcVM;

        string _type = "IQuanity";
        public string Type
        {
            get
            {
                //return calcValue.Type.ToString();
                return _type;
            }
        }

        //bool _sliderEntry = false;
        //public bool SliderEntry
        //{
        //    get
        //    {
        //        return _sliderEntry;
        //    }
        //    set
        //    {
        //        _sliderEntry = value;
        //        RaisePropertyChanged(nameof(SliderEntry));
        //        RaisePropertyChanged(nameof(MinVal));
        //        RaisePropertyChanged(nameof(MaxVal));
        //    }
        //}

        //double _maxVal;
        //public double MaxVal { get { return _maxVal; } set { _maxVal = value; RaisePropertyChanged(nameof(MaxVal)); } }
        //double _minVal;
        //public double MinVal { get { return _minVal; } set { _minVal = value; RaisePropertyChanged(nameof(MinVal)); } }

        public string Value
        {
            get
            {
                return calcValue.ValueAsString();
            }
            set
            {
                calcValue.TryParse(value);
                calc.Calculate();
                //RaisePropertyChanged(nameof(Value));
                //RaisePropertyChanged(nameof(ValueRounded));
                calcVM.UpdateOutputs();
            }
        }

        //public bool BoolValue
        //{
        //    get
        //    {
        //        return (calcValue as CalcCore.CalcBool).Value;
        //    }
        //    set
        //    {
        //        (calcValue as CalcCore.CalcBool).Value = value;
        //        calc.UpdateCalc();
        //        RaisePropertyChanged(nameof(BoolValue));
        //        RaisePropertyChanged(nameof(ValueRounded));
        //        calcVM.UpdateOutputs();
        //    }
        //}

        //public string ValueRounded
        //{
        //    get
        //    {
        //        if (calcValue.Type == CalcCore.CalcValueType.DOUBLE)
        //        {
        //            double val = (calcValue as CalcCore.CalcDouble).Value;
        //            if (val >= 1000)
        //                return val.ToString("N0");
        //            else
        //                return val.ToString("G3");
        //        }
        //        return calcValue.ValueAsString;
        //    }
        //}

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

        //public string Units
        //{
        //    get
        //    {
        //        return calcValue.Unit;
        //    }
        //}

        //public string Name
        //{
        //    get
        //    {
        //        return calcValue.Name;
        //    }
        //}

        //public CalcCore.CalcStatus Status
        //{
        //    get
        //    {
        //        return calcValue.Status;
        //    }
        //}

        public IOValues(ICalcValue calcValue, ICalculation calc, ICalcViewParent calcVM)
        {
            this.calcValue = calcValue;
            this.calc = calc;
            this.calcVM = calcVM;

            switch (calcValue)
            {
                case ICalcQuantity:
                    break;
                case CalcSelectionList:
                    _selectionList = (calcValue as CalcSelectionList).SelectionList;
                    this._type = "SELECTIONLIST";
                    break;
                default:
                    break;
            }
        }
    }
}
