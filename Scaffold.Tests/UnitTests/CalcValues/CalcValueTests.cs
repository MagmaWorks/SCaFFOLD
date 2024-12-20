using OasysUnits.Units;
using Scaffold.Core.CalcValues;

namespace Scaffold.Tests.UnitTests.CalcValues
{
    public class CalcValueTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Assemble
            var calcDouble = new CalcDouble(4.5);

            // Act
            // Assert
            Assert.False(calcDouble.TryParse("invalid"));
        }

        [Theory]
        [InlineData(4.3, 4.3, true)]
        [InlineData(4.31, 4.3, false)]
        public void EqualOperatorTest(double val1, double val2, bool expected)
        {
            // Assemble
            var calcArea1 = new CalcDouble(val1);
            var calcArea2 = new CalcDouble(val2);

            // Act
            bool result = calcArea1 == calcArea2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void NotEqualOperatorTest(double val1, double val2, bool expected)
        {
            // Assemble
            var calcArea1 = new CalcDouble(val1);
            var calcArea2 = new CalcDouble(val2);

            // Act
            bool result = calcArea1 != calcArea2;

            // Assert
            Assert.Equal(expected, result);
        }
    }
} 
