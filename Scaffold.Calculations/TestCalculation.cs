using System.Collections.Generic;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;

namespace Scaffold.Calculations
{
    public class TestCalculation : ICalculation
    {
        public string ReferenceName { get; set; } = "";
        public string CalculationName { get; set; } = "Test Calculation";
        public CalcStatus Status { get; set; } = CalcStatus.None;

        [InputCalcValue(@"D", "Multiplier")]
        public double Multiplier { get; set; } = 0;

        [InputCalcValue(@"F", "Force")]
        public CalcForce Force { get; set; } = new CalcForce(10, "Force", "F");

        [OutputCalcValue(@"R", "Result")]
        public double Result { get; private set; } = 0;

        IList<IFormula> _formulaList = new List<IFormula>();

        public IList<IFormula> GetFormulae()
        {
            return _formulaList;
        }

        public void Calculate()
        {
            Result = Force.Value * Multiplier;
            _formulaList.Clear();
            _formulaList.Add(new Formula("BS EN 1992", "This is a test calc in SCaFFOLD 2.0", "OK", @"\zeta = \eta / \kappa", CalcStatus.Pass));
        }
    }
}
