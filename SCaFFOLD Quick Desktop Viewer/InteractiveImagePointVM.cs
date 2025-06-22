// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scaffold.Core.Interfaces;
using UnitsNet;

namespace SCaFFOLD_Quick_Desktop_Viewer
{

    public class InteractiveImagePointVM : ViewModelBase
    {
        double x, y;

        ICalcViewParent parent;

        public double X
        {
            get
            {
                return _xValue.Value;
            }
            set
            {
                _xValue.TryParse(value.ToString());
                parent.UpdateOutputs();
                RaisePropertyChanged(nameof(X));
            }
        }

        public double Y
        {
            get
            {
                return _yValue.Value;
            }
            set
            {
                _yValue.TryParse(value.ToString());
                parent.UpdateOutputs();
                RaisePropertyChanged(nameof(Y));
            }
        }

        public System.Windows.Point XY
        {
            get
            {
                var pt = new System.Windows.Point((int)X, (int)Y);
                return pt;
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }

        }



        ICalcQuantity _xValue;
        ICalcQuantity _yValue;

        public InteractiveImagePointVM(ICalcQuantity xValue, ICalcQuantity yValue, ICalcViewParent parent)
        {
            this._xValue = xValue;
            this._yValue = yValue;
            this.parent = parent;
        }
    }
}
