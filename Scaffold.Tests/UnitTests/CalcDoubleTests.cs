using Scaffold.Core.CalcValues;

namespace Scaffold.Tests.UnitTests
{
    public class CalcDoubleTests
    {
        [Fact]
        public void ImplicitOperatorTest()
        {
            // Assemble
            var calcDouble1 = new CalcDouble(4.5);
            var calcDouble2 = new CalcDouble(5.5);

            // Act
            double result = calcDouble1 + calcDouble2;

            // Assert
            Assert.Equal(10.0, result);
        }
    }
}
