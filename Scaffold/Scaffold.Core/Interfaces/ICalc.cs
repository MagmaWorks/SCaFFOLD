using Scaffold.Core.Enums;
using Scaffold.Core.Models;

namespace Scaffold.Core.Interfaces
{
    public interface ICalc
    {
        string InstanceName { get; set; }
        string TypeName { get; }
        CalcStatus Status { get; }
        List<Formula> GetFormulae();
        void Recalculate();
        List<netDxf.DxfDocument> GetDrawings();
        List<MW3DModel> Get3DModels();
    }
}