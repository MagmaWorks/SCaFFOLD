namespace Scaffold.VisualStudio.Models.Scaffold;

public interface IFormula
{
    public List<string> Expressions { get; }
    public string Ref { get; set; }
    public string Narrative { get; set; }
    public string Conclusion { get; set; }
    public string Status { get; set; }
}