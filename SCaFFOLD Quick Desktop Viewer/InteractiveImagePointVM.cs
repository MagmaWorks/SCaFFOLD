// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Interfaces;
using UnitsNet;

namespace SCaFFOLD_Quick_Desktop_Viewer
{

    public class InteractiveImagePointVM : ViewModelBase
    {
        public string GraphicType { get; private set; } = "POINT";

        CalcDoubleMultiArray array;

        ICalcViewParent parent;

        MagmaWorks.Geometry.Line2d line;

        public void Refresh()
        {
            RaisePropertyChanged(nameof(XY));
            RaisePropertyChanged(nameof(LineStart));
            RaisePropertyChanged(nameof(LineEnd));
        }

        public System.Windows.Point XY
        {
            get
            {
                var pt = new System.Windows.Point((int)array.Value[0][0], (int)array.Value[0][1]);
                return pt;
            }
            set
            {
                array.Value[0][0] = value.X;
                array.Value[0][1] = value.Y;
                parent.UpdateOutputs();
            }
        }

        public System.Windows.Point LineStart
        {
            get
            {
                var pt = new System.Windows.Point(line.Start.U.Value, line.Start.V.Value);
                return pt;
            }
        }
        public System.Windows.Point LineEnd
        {
            get
            {
                var pt = new System.Windows.Point(line.End.U.Value, line.End.V.Value);
                return pt;
            }
        }

        public InteractiveImagePointVM(CalcDoubleMultiArray array, ICalcViewParent parent)
        {
            this.GraphicType = "POINT";
            this.array = array;
            this.parent = parent;
        }

        public InteractiveImagePointVM(MagmaWorks.Geometry.Line2d line, ICalcViewParent parent)
        {
            this.GraphicType = "LINE";
            this.parent = parent;
            this.line = line;
        }
    }
}
