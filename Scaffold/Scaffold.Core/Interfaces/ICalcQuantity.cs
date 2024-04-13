using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.Core.Interfaces
{
    public interface ICalcQuantity : ICalcValue
    {
        string Unit { get; }
        double Value { get; set; }
    }
}
