using OasysUnits;

namespace Scaffold.Core.Interfaces;

public interface ICalcQuantity<TQuantity> : ICalcValue where TQuantity : IQuantity
{
    string Unit { get; }
    double Value { get; }
    public TQuantity Quantity { get; }
}
