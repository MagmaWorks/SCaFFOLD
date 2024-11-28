using Scaffold.Calculations;
using Scaffold.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.XUnitTests.UnitTests
{
    public class CreepAndShrinkageTests
    {

        [Fact]
        public void CreepAndShrinkageTest1()
        {
            var reader = new CalculationReader();
            var calculation = new ConcCreepAndShrinkage();
            // Do stuff here
            calculation.Update();

            var metadata = reader.GetMetadata(calculation);
            var inputs = reader.GetInputs(calculation);
            var outputs = reader.GetOutputs(calculation);
            var formulae = reader.GetFormulae(calculation);

            Console.WriteLine(inputs[0]);

            // Some assertions here
        }
    }
}

