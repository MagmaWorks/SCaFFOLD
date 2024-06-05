namespace Scaffold.VisualStudio.Models.Results;

public class ProcessResult
{
    public int ExitCode { get; set; }
    public List<string> Output { get; set; }
}