﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using CalcCore;
using HelixToolkit.Wpf;
using MWGeometry;
using Newtonsoft;
using SkiaSharp.Views.WPF;

namespace Calcs
{
    public class CalculationViewModel : ViewModelBase, ICalcViewParent
    {
        CalcCore.ICalc calc;
        public CalcCore.ICalc Calc { get => calc; }
        public TableVM Table { get; set; }
        public ChartVM Chart { get; set; }
        public CrossRefVM CrossRef {get;set;}

        Model3D _model;
        public Model3D Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
                RaisePropertyChanged(nameof(Model));
            }
        }

        string filepath = "";
        public string Filepath
        {
            get
            {
                return filepath;
            }
            set
            {
                filepath = value;
                RaisePropertyChanged(nameof(Filepath));
            }
        }


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

        public ObservableCollection<DXFDrawData> TestGroup
        {
            get
            {
                var test = calc.GetDrawings();
                if (test != null)
                {
                    var rettest = DXFDisplay.ReadDXF(test[0]);
                    return new ObservableCollection<DXFDrawData>(rettest);
                }
                else
                    return new ObservableCollection<DXFDrawData> { };
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

        public CalcCore.CalcStatus Status
        {
            get
            {
                return calc.Status;
            }
        }


        public CalculationViewModel(CalcCore.ICalc calc)
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
            this.Table = new TableVM(calc);
            this.Chart = new ChartVM(calc);
            this.CrossRef = new CrossRefVM(calc);
            _formulae = new ObservableCollection<FormulaeVM>();
            foreach (var item in calc.GetFormulae())
            {
                BitmapSource im = null;
                if (item.Image != null)
                {
                    im = item.Image.ToWriteableBitmap();
                }
                _formulae.Add(new FormulaeVM() { Expression = item.Expression, Ref = item.Ref, Conclusion = item.Conclusion, Narrative = item.Narrative, Status = item.Status, Image = im });
            }
            if (this.calc.Get3DModels().Count>0)
            {
                makeModel(this.calc.Get3DModels()[0]);
            }
        }

        private void makeModel(MW3DModel calcCore3DModel)
        {
            var m = new Model3DGroup();
            foreach (var mesh in calcCore3DModel.Meshes)
            {
                MWColor col = mesh.Brush.Color;
                Brush myBrush = new SolidColorBrush(Color.FromArgb(col.Alpha, col.Red, col.Green, col.Blue));
                myBrush.Opacity = mesh.Opacity;
                Material mat = new DiffuseMaterial(myBrush);

                MeshBuilder meshBuilder = new MeshBuilder(false, true);

                foreach (var pos in mesh.Nodes)
                {
                    meshBuilder.Positions.Add(new Point3D(pos.Point.X, pos.Point.Y, pos.Point.Z));
                    meshBuilder.TextureCoordinates.Add(new Point(0.5, 0.5));
                }
                foreach (var triangle in mesh.MeshIndices)
                {
                    meshBuilder.AddTriangle(triangle);
                }

                var meshOutput = meshBuilder.ToMesh(true);

                m.Children.Add(new GeometryModel3D(meshOutput, mat));
            }
            foreach (var item in calcCore3DModel.Text)
            {
                MWPoint3D p = item.Position;
                MWVector3D v = item.Direction;
                MWVector3D u = item.Up;
                var text = HelixToolkit.Wpf.TextCreator.CreateTextLabelModel3D(item.Text, Brushes.Black, item.IsDoubleSided, item.Height, new Point3D(p.X, p.Y, p.Z), new Vector3D(v.X, v.Y, v.Z), new Vector3D(u.X, u.Y, u.Z));
                m.Children.Add(text);
            }
            Model = m;
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
                BitmapSource im = null;
                if (item.Image != null)
                {
                    im = item.Image.ToWriteableBitmap();
                }
                _formulae.Add(new FormulaeVM() { Expression = item.Expression, Ref = item.Ref, Conclusion = item.Conclusion, Narrative = item.Narrative, Status = item.Status, Image = im });
            }
            RaisePropertyChanged(nameof(Formulae));
            RaisePropertyChanged(nameof(TestGroup));
            if (this.calc.Get3DModels().Count > 0)
            {
                makeModel(this.calc.Get3DModels()[0]);
                RaisePropertyChanged(nameof(Model));
            }
            RaisePropertyChanged(nameof(Status));
        }
    }
}