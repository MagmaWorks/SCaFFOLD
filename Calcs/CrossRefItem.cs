using CalcCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calcs
{
    public class CrossRefItem : ViewModelBase
    {
        ICalc calc;

        public int InputIndex { get; }

        public string Name { get { return calc.GetInputs()[InputIndex].Name; } }

        bool _isSelected = true;
        private double _minVal;
        private double _maxVal;
        private int _steps;
        private double _step;
        private bool _fixedStepSize;
        private bool _fixedSteps;

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
            }
        }

        public bool FixedStepSize { get { return _fixedStepSize; } set { _fixedStepSize = value; RaisePropertyChanged(nameof(_fixedStepSize)); RaisePropertyChanged(nameof(StepsFullyFixed)); } }

        public bool FixedSteps { get { return _fixedSteps; } set { _fixedSteps = value; RaisePropertyChanged(nameof(_fixedSteps)); RaisePropertyChanged(nameof(StepsFullyFixed)); } }

        public bool StepsFullyFixed
        {
            get
            {
                return !(FixedStepSize && FixedSteps);
            }  
        }

        public double MinVal { get => _minVal; set { _minVal = value; RaisePropertyChanged(nameof(MinVal)); } }

        public double MaxVal { get => _maxVal; set
            {
                if (FixedSteps)
                {
                    _maxVal = value;
                }
                
                RaisePropertyChanged(nameof(MaxVal));
                RaisePropertyChanged(nameof(Steps));
                RaisePropertyChanged(nameof(Step));
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
            }

        }


        public CrossRefItem(ICalc calc, int inputIndex)
        {
            this.calc = calc;
            this.InputIndex = inputIndex;
            MinVal = 0;
            MaxVal = 100;
            Steps = 20;
        }
    }
}