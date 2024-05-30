using Scaffold.Core.Enums;

namespace Scaffold.VisualStudio.Models;

public class CalcValueDetail
{
    public string DisplayName { get; set; }
    public string Symbol { get; set; }
    public string Value { get; set; }
    public CalcStatus Status { get; set; }
}