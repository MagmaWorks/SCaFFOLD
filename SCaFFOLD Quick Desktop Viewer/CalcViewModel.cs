// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scaffold.Core;

using Scaffold.Core.Interfaces;

namespace SCaFFOLD_Quick_Desktop_Viewer
{
    internal class CalcViewModel
    {
        private ICalculation calculation { get; set; }

        CalcViewModel(ICalculation calculation)
        {
            this.calculation = calculation;
        }

    }
}
