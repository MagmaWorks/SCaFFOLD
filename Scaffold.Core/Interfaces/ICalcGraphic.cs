// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagmaWorks.Geometry;
using Scaffold.Core.CalcValues;

namespace Scaffold.Core.Interfaces
{
    public interface ICalcGraphic
    {
        List<IGeometryBase> Geometry { get; }
        List<CalcDoubleMultiArray> ControlPoints { get; }
    }
}
