namespace Scaffold.VisualStudio.Models.Scaffold;

public class FormulaDetail : IFormula
{
    public List<string> Expressions { get; set; }
    public string Ref { get; set; }
    public string Narrative { get; set; }
    public string Conclusion { get; set; }
    public string Status { get; set; }
}