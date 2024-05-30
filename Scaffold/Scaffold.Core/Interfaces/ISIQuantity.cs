namespace Scaffold.Core.Interfaces
{
    public interface ICalcSIQuantity : ICalcQuantity
    {
        public OasysUnits.IQuantity Quantity { get;  } 

    }
}