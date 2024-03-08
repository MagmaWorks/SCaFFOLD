using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.CalcValues.Abstract
{
    public abstract class CalcValue<T> : ICalcDetails
    {
        protected CalcValue(IoDirection group, string name, string symbol, OasysUnits.IQuantity unit)
        {
            Group = group;
            Name = name;
            Symbol = symbol;
            Unit = unit;
            Status = CalcStatus.NONE;
        }

        public string Name { get; set; }
        public string Symbol { get; set; }
        public OasysUnits.IQuantity Unit { get; set; }
        public T Value { get; set; }
        public IoDirection Group { get; set; }
        public CalcStatus Status { get; set; }

        public abstract CalcValueType GetCalcType();
        public abstract void SetValueFromString(string strValue);

        public override string ToString() => Value.ToString();
    }
}