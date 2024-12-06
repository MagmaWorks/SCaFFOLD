using System.Collections.Generic;
using Scaffold.Core.Attributes;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Calculations
{
    public class ConcreteCreepAndShrinkage : ICalculation
    {
        public string ReferenceName { get; set; } = "";
        public string CalculationName { get; set; } = "Concrete Creep and Shrinkage";
        public CalcStatus Status { get; set; } = CalcStatus.None;

        [InputCalcValue(@"\varphi(t,t_0)", "Notional Creep Coefficient")]
        public double NotionalCreepCoefficient { get; } = 0;

        public IList<IFormula> GetFormulae()
        {
            return new List<IFormula>();
        }

        public void Calculate()
        {

        }
    }
}
