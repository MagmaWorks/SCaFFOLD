namespace Scaffold.VisualStudio.Models;

public class CalculationDetail
{
    public string Title { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public List<CalcValueDetail> Inputs { get; set; }
    public List<CalcValueDetail> Outputs { get; set; }
    public List<DisplayFormula> Formulae { get; set; }
}