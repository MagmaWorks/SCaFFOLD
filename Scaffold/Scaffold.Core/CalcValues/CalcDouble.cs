using Scaffold.Core.CalcValues.Abstract;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.CalcValues
{
    public class CalcDouble : CalcValue<double>
    {
        public CalcDouble(IoDirection group, string name, string symbol, OasysUnits.IQuantity unit, double value) 
            : base(group, name, symbol, unit)
        {
            Value = value;
        }

        public override CalcValueType GetCalcType() => CalcValueType.DOUBLE;
        public override void SetValueFromString(string strValue)
        {
            var parsedSuccessfully = double.TryParse(strValue, out var result);
            if (parsedSuccessfully == false)
                throw new ArgumentException($"Failed to parse {strValue} to a double");

            Value = result;
        }
    }
}