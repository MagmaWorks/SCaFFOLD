namespace Scaffold.VisualStudio.Models;

public class CalculationResult
{
    public CalculationDetail CalculationDetail { get; set; }
    public ExceptionDetail Failure { get; set; }
    public bool IsSuccess => Failure == null;
}