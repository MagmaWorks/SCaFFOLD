using Scaffold.Core.Enums;
using Scaffold.Core.Models;

namespace Scaffold.VisualStudio.Models;

public class CalculationDetail
{
    public string Title { get; set; }
    public string Type { get; set; }
    public CalcStatus Status { get; set; }
    public List<CalcValueDetail> Inputs { get; set; }
    public List<CalcValueDetail> Outputs { get; set; }
    public List<Formula> Formulae { get; set; }
}