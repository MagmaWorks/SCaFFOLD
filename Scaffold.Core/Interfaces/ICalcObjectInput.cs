namespace Scaffold.Core.Interfaces;

public interface ICalcObjectInput<T> : ICalculation
{
    T Output { get; }
}
