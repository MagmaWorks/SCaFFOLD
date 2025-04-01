using OasysUnits;

namespace Scaffold.Core.Interfaces;

public interface ICalcQuantity<T> : ICalcValue where T : IQuantity
{
    string Unit { get; }
    double Value { get;  }
    public T Quantity { get; set; }
}
