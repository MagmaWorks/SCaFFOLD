using OasysUnits;

namespace Scaffold.Core.Interfaces;

public interface ICalculationQuantityParameter<Q> : ICalculationParameter<Q> where Q : IQuantity
{
}
