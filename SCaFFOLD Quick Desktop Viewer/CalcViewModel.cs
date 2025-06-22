// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using MagmaWorks.Geometry;
using Scaffold.Core;
using Scaffold.Core.Abstract;
using Scaffold.Core.Interfaces;

namespace SCaFFOLD_Quick_Desktop_Viewer
{
    public class CalcViewModel : ViewModelBase, ICalcViewParent
    {
        private ICalculation calculation { get; set; }
        private CalculationReader reader { get; set; } = new CalculationReader();

        public string TestName
        {
            get
            {
                return "TEST";
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

        // testing interactive image idea
        List<InteractiveImagePointVM> _interactivePoints;
        public List<InteractiveImagePointVM> InteractivePoints
        {
            get => _interactivePoints;
            set { _interactivePoints = value; }
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

        public CalcViewModel(ICalculation calculation)
        {
            this.calculation = calculation;
            reader.GetFormulae(calculation);
            var inputsList = reader.GetInputs(calculation);

            Inputs = new List<IOValues>();
            foreach (var item in inputsList)
            {
                Inputs.Add(new IOValues(item, calculation, this));
            }

            // testing interactive image logic
            _interactivePoints = new List<InteractiveImagePointVM>();
            var graphics = calculation.GetGraphic();
            foreach (var item in graphics[0].ControlPoints)
            {
                _interactivePoints.Add(new InteractiveImagePointVM(item, this));
            }
            foreach (var item in graphics[0].Geometry)
            {
                switch (item)
                {
                    case Line2d:
                        _interactivePoints.Add(new InteractiveImagePointVM(item as Line2d, this));
                        break;
                    default:
                        break;

                }


                UpdateOutputs();
            }

        }
        public void UpdateOutputs()
        {
            //Outputs = new ObservableCollection<IOValues>();
            //foreach (var item in ICalculation.)
            //{
            //    Outputs.Add(new IOValues(item, calc, this));
            //}
            _formulae = new ObservableCollection<FormulaeVM>();
            var outputs = calculation.GetFormulae();
            foreach (var item in outputs)
            {
                BitmapSource im = null;
                if (item.Image != null)
                {
                    //im = item.Image.ToWriteableBitmap();
                }
                _formulae.Add(new FormulaeVM() { Expression = item.Expression, Ref = item.Reference, Conclusion = item.Conclusion, Narrative = item.Narrative, Status = item.Status, Image = im });
            }
            RaisePropertyChanged(nameof(Formulae));

            foreach (var item in inputs)
            {
                item.Refresh();
            }
            foreach(var item in _interactivePoints)
            {
                item.Refresh();
            }

            //RaisePropertyChanged(nameof(TestGroup));
            //if (this.calc.Get3DModels().Count > 0)
            //{
            //    makeModel(this.calc.Get3DModels()[0]);
            //    RaisePropertyChanged(nameof(Model));
        }
        //RaisePropertyChanged(nameof(Status));

        //foreach (var item in MainViews)
        //{
        //    if (item.ViewType == MainViewTypes.PLUGIN)
        //    {
        //        item.Plugin.Update();
        //    }
        //}
    }

}
