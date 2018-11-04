using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public interface ICalc
    {
        string InstanceName { get; set; }
        string TypeName { get; }
        List<CalcValueBase> GetInputs();
        List<CalcValueBase> GetOutputs();
        List<Formula> GetFormulae();
        void UpdateCalc();
    }
}
