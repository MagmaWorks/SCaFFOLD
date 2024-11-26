using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scaffold.Calculations;
using Scaffold.Core.Abstract;
using Scaffold.XUnitTests.Core;

namespace Scaffold.XUnitTests.UnitTests
{
    public class PunchingShearTests
    {
        [Fact]
        public void PunchingTest1()
        {
            var reader = new CalculationReader();
            var calculation = new PunchingShear();
            // Do stuff here
            calculation.Update();

            var metadata = reader.GetMetadata(calculation);
            var inputs = reader.GetInputs(calculation);
            var outputs = reader.GetOutputs(calculation);
            var formulae = reader.GetFormulae(calculation);

            // Some assertions here
        }
    }
}
