using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scaffold.Core.Attributes;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;

namespace Scaffold.Calculations
{
    public class ConcCreepAndShrinkage : ICalculation
    {
        public string Title { get; set; } = "";
        public string Type { get; set; } = "Concrete Creep and Shrinkage";

        public CalcStatus Status { get; set; } = CalcStatus.None;

        [InputCalcValue(@"\varphi(t,t_0)", "Notional Creep Coefficient")]
        public double notionalCreepCoeff { get; } = 0;



        public IEnumerable<Formula> GetFormulae()
        {
            return new List<Formula>();
        }

        public void Update()
        {

        }
    }
}
