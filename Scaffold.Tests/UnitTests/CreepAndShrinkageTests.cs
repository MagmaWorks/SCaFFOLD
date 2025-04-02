using Scaffold.Calculations;
using Scaffold.Core.Abstract;

namespace Scaffold.Tests.UnitTests
{
    public class CreepAndShrinkageTests
    {
        [Fact]
        public void CreepAndShrinkageTest1()
        {
            var reader = new CalculationReader();
            var calculation = new TestCalculation();
            // Do stuff here
            calculation.Calculate();

            var metadata = reader.GetMetadata(calculation);
            var inputs = reader.GetInputs(calculation);
            var outputs = reader.GetOutputs(calculation);
            var formulae = reader.GetFormulae(calculation);

            Console.WriteLine(inputs[0]);

            // Some assertions here
        }
    }
}
