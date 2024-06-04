namespace Scaffold.VisualStudio.Models;

public class CalculationResult
{
    public CalculationDetail CalculationDetail { get; set; }
    public ErrorDetail Failure { get; set; }
    public bool IsSuccess => Failure == null;
}