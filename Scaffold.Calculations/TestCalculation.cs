using System.Collections.Generic;
using System.Dynamic;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Calculations
{
    public class TestCalculation : ICalculation
    {
        public string ReferenceName { get; set; } = "";
        public string CalculationName { get; set; } = "Test Calculation";
        public CalcStatus Status { get; set; } = CalcStatus.None;

        [InputCalcValue("Multiplier", @"D")]
        public double Multiplier { get; set; } = 0;

        [InputCalcValue("Force", @"F1")]
        public CalcForce Force { get; set; } = new CalcForce(10, "Force", "F2");

        [OutputCalcValue("Result", @"R")]
        public double Result { get; private set; } = 0;

        public IList<IFormula> GetFormulae()
        {
            return new List<IFormula>();
        }

        public void Calculate()
        {
            Result = Force.Value * Multiplier;
        }
    }
}
