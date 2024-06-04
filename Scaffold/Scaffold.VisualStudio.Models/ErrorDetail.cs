namespace Scaffold.VisualStudio.Models;

public class ErrorDetail
{
    public string Source { get; set; }
    public string Message { get; set; }
    public string InnerException { get; set; }
    public string StackTrace { get; set; }
}