namespace Scaffold.Core.Interfaces;

public interface ICalcQuantity : ICalcValue
{
    string Unit { get; }
    double Value { get; set; }
}