using CalcCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Calcs;

namespace WindowsAppPlugins
{
    public class CrossRefItem : ViewModelBase
    {
        ICalc calc;
        CrossRefVM crossRefVM;

        public int InputIndex { get; }

        public string Name { get { return calc.GetInputs()[InputIndex].Name; } }
        public CalcCore.CalcValueType CalcType { get { return calc.GetInputs()[InputIndex].Type; } }
        public string Unit { get { return calc.GetInputs()[InputIndex].Unit; } }
        public string Symbol { get { return calc.GetInputs()[InputIndex].Symbol; } }

        bool _isSelected = false;
        private double _minVal;
        private double _maxVal;
        private int _steps;
        private double _step;
        private bool _fixedStepSize;
        private bool _fixedSteps;
        private int startIndex;
        private int endIndex;

        public Visibility Visi
        {
            get
            {
                if (IsSelected)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
                RaisePropertyChanged(nameof(Visi));
                crossRefVM.calcCrossRefInputs();

            }
        }

        public bool FixedStepSize { get { return _fixedStepSize; } set { _fixedStepSize = value; RaisePropertyChanged(nameof(_fixedStepSize)); RaisePropertyChanged(nameof(StepsFullyFixed)); crossRefVM.calcCrossRefInputs(); ; } }

        public bool FixedSteps { get { return _fixedSteps; } set { _fixedSteps = value; RaisePropertyChanged(nameof(_fixedSteps)); RaisePropertyChanged(nameof(StepsFullyFixed)); crossRefVM.calcCrossRefInputs(); } }

        public bool StepsFullyFixed
        {
            get
            {
                return !(FixedStepSize && FixedSteps);
            }  
        }

        public double MinVal { get => _minVal; set
            {
                if (FixedStepSize)
                {
                    _minVal = value;
                    _steps = (int)(Math.Round((_maxVal - _minVal) / _step, 0));
                }
                else
                {
                    _minVal = value;
                    _step = (_maxVal - _minVal) / _steps;
                }

                RaisePropertyChanged(nameof(MaxVal));
                RaisePropertyChanged(nameof(Steps));
                RaisePropertyChanged(nameof(Step));
                crossRefVM.calcCrossRefInputs();
            }
        }

        public double MaxVal { get => _maxVal; set
            {
                if (FixedStepSize)
                {
                    _maxVal = value;
                    _steps = (int)(Math.Round((_maxVal - _minVal) / _step,0));
                }
                else
                {
                    _maxVal = value;
                    _step = (_maxVal - _minVal) / _steps;
                }

                RaisePropertyChanged(nameof(MaxVal));
                RaisePropertyChanged(nameof(Steps));
                RaisePropertyChanged(nameof(Step));
                crossRefVM.calcCrossRefInputs();
            }
        }

        public int Steps {
            get => _steps;
            set {
                if (FixedStepSize)
                {
                    double temp = Step;
                    _steps = value;
                    _maxVal = _minVal + temp * _steps;
                }
                else
                {
                    _steps = value;
                }
                RaisePropertyChanged(nameof(Steps));
                RaisePropertyChanged(nameof(Step));
                RaisePropertyChanged(nameof(MaxVal));
                crossRefVM.calcCrossRefInputs();
            }
        }

        public double Step {
            get
            {
                return (MaxVal - MinVal) / Steps;
            }
            set
            {
                if (FixedSteps)
                {
                    _maxVal = _minVal + (value * _steps);
                }
                else
                {
                    _steps = (int)((MaxVal - MinVal) / value);
                    _maxVal = MinVal + (value * _steps);
                }
                RaisePropertyChanged(nameof(MaxVal));
                RaisePropertyChanged(nameof(Steps));
                RaisePropertyChanged(nameof(Step));
                crossRefVM.calcCrossRefInputs();
            }
        }

        public int StartIndex { get { return startIndex; } set { startIndex = value; RaisePropertyChanged(nameof(StartIndex)); crossRefVM.calcCrossRefInputs(); } }
        public int EndIndex { get { return endIndex; } set { endIndex = value; RaisePropertyChanged(nameof(EndIndex)); crossRefVM.calcCrossRefInputs(); } }
        public List<string> SelectionList { get { return ((calc.GetInputs()[InputIndex]) as CalcCore.CalcSelectionList).SelectionList; } }
        IOValues _inputValue;
        public IOValues InputValue
        {
            get
            {
                return _inputValue;
            }
        }

        public CrossRefItem(ICalc calc, int inputIndex, CrossRefVM crossRefVM)
        {
            this.crossRefVM = crossRefVM;
            this.calc = calc;
            this.InputIndex = inputIndex;
            MinVal = 0;
            MaxVal = 100;
            Steps = 20;
            _inputValue = new IOValues(calc.GetInputs()[inputIndex], calc, crossRefVM);
        }
    }
}