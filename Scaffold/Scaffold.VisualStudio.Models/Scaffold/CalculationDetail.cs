namespace Scaffold.VisualStudio.Models.Scaffold;

public class CalculationDetail<T> where T : IFormula
{
    public string Title { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public List<CalcValueDetail> Inputs { get; set; }
    public List<CalcValueDetail> Outputs { get; set; }
    public List<T> Formulae { get; set; }
}