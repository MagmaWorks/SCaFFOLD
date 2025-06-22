// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagmaWorks.Geometry;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Geometry.Abstract;

namespace Scaffold.Core.Models
{
    public class CalcGraphic : ICalcGraphic
    {
        public List<IGeometryBase> Geometry { get; }
        public List<CalcDoubleMultiArray> ControlPoints { get; }

        public CalcGraphic(List<IGeometryBase> geometry, List<CalcDoubleMultiArray> controlPoints)
        {
            Geometry = geometry;
            ControlPoints = controlPoints;
        }
    }
}
