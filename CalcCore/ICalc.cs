using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netDxf;
using MWGeometry;

namespace CalcCore
{
    public interface ICalc
    {
        string InstanceName { get; set; }
        string TypeName { get; }
        CalcStatus Status { get; }
        List<CalcValueBase> Inputs { get; }
        List<CalcValueBase> GetInputs();
        List<CalcValueBase> GetOutputs();
        List<Formula> GetFormulae();
        void UpdateCalc();
        List<netDxf.DxfDocument> GetDrawings();
        List<MW3DModel> Get3DModels();
    }
}
