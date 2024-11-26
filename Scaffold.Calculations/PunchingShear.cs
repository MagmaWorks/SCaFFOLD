using System.Collections.Generic;
using Scaffold.Core.Attributes;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;

namespace Scaffold.Calculations
{
    [CalculationMetadata("Punching Shear", "SCaFFOLD Lib")]
    public class PunchingShear : ICalculation
    {
        public string Title { get; set; } = "Example";
        public string Type { get; set; } = "Punching Shear to EC2";

        [InputCalcValue("F", "Punching Force")]
        public double _force { get; set; } = 1005;

        [OutputCalcValue("R", "Resistance")]
        public double _resistance { get; set; } = 0;

        [InputCalcValue("G", "grade")]
        public double _grade { get; set; } = 40;

        public CalcStatus Status => CalcStatus.None;

        public IEnumerable<Formula> GetFormulae()
        {
            return new List<Formula>();
        }
        public PunchingShear()
        {

        }

        public void Update()
        {
            _resistance = _force * 2;
        }
    }
}
