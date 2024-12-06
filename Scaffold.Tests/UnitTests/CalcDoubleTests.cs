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

        [Fact]
        public void AdditionOperatorTest()
        {
            // Assemble
            var calcDouble1 = new CalcDouble(4.5);
            var calcDouble2 = new CalcDouble(5.5);

            // Act
            CalcDouble result = calcDouble1 + calcDouble2;

            // Assert
            Assert.Equal(10.0, result.Value);
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Assemble
            var calcDouble1 = new CalcDouble(4.5);
            var calcDouble2 = new CalcDouble(5.5);

            // Act
            CalcDouble result = calcDouble1 - calcDouble2;

            // Assert
            Assert.Equal(-1.0, result.Value);
        }

        [Fact]
        public void MultiplicationOperatorTest()
        {
            // Assemble
            var calcDouble1 = new CalcDouble(4.5);
            var calcDouble2 = new CalcDouble(5.5);

            // Act
            CalcDouble result = calcDouble1 * calcDouble2;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Assemble
            var calcDouble1 = new CalcDouble(4.5);
            var calcDouble2 = new CalcDouble(5.5);

            // Act
            CalcDouble result = calcDouble1 / calcDouble2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value);
        }
    }
}
