using Scaffold.Core.CalcValues;

namespace Scaffold.Tests.UnitTests.CalcValues
{
    public class CalcDoubleTests
    {
        [Fact]
        public void Constructor_WithValueAndDisplayName_SetsProperties()
        {
            // Arrange & Act
            var calcDouble = new CalcDouble(4.5, "test value");

            // Assert
            Assert.Equal(4.5, calcDouble.Value);
            Assert.Equal("test value", calcDouble.DisplayName);
            Assert.True(string.IsNullOrEmpty(calcDouble.Symbol));
            Assert.True(string.IsNullOrEmpty(calcDouble.Unit));
        }

        [Fact]
        public void Constructor_WithValueDisplayNameAndSymbol_SetsProperties()
        {
            // Arrange & Act
            var calcDouble = new CalcDouble(4.5, "test value", "α");

            // Assert
            Assert.Equal(4.5, calcDouble.Value);
            Assert.Equal("test value", calcDouble.DisplayName);
            Assert.Equal("α", calcDouble.Symbol);
            Assert.True(string.IsNullOrEmpty(calcDouble.Unit));
        }

        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            var calcDouble = new CalcDouble(4.5);

            // Act & Assert
            Assert.False(calcDouble.TryParse("invalid"));
            Assert.True(calcDouble.TryParse("5.5"));
            Assert.Equal(5.5, calcDouble.Value);
        }

        [Theory]
        [InlineData(4.3, 4.3, true)]
        [InlineData(4.31, 4.3, false)]
        public void EqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcDouble1 = new CalcDouble(val1);
            var calcDouble2 = new CalcDouble(val2);

            // Act
            bool result = calcDouble1 == calcDouble2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void NotEqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcDouble1 = new CalcDouble(val1);
            var calcDouble2 = new CalcDouble(val2);

            // Act
            bool result = calcDouble1 != calcDouble2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.5, 2.0, true)] // 4.5 > 2.0
        [InlineData(2.0, 4.5, false)] // 2.0 > 4.5
        [InlineData(4.5, 4.5, false)] // 4.5 > 4.5
        public void GreaterThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcDouble1 = new CalcDouble(val1);
            var calcDouble2 = new CalcDouble(val2);

            // Act
            bool result = calcDouble1 > calcDouble2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.5, 2.0, false)] // 4.5 < 2.0
        [InlineData(2.0, 4.5, true)] // 2.0 < 4.5
        [InlineData(4.5, 4.5, false)] // 4.5 < 4.5
        public void LessThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcDouble1 = new CalcDouble(val1);
            var calcDouble2 = new CalcDouble(val2);

            // Act
            bool result = calcDouble1 < calcDouble2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.5, 2.0, true)] // 4.5 >= 2.0
        [InlineData(2.0, 4.5, false)] // 2.0 >= 4.5
        [InlineData(4.5, 4.5, true)] // 4.5 >= 4.5
        public void GreaterThanOrEqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcDouble1 = new CalcDouble(val1);
            var calcDouble2 = new CalcDouble(val2);

            // Act
            bool result = calcDouble1 >= calcDouble2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.5, 2.0, false)] // 4.5 <= 2.0
        [InlineData(2.0, 4.5, true)] // 2.0 <= 4.5
        [InlineData(4.5, 4.5, true)] // 4.5 <= 4.5
        public void LessThanOrEqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcDouble1 = new CalcDouble(val1);
            var calcDouble2 = new CalcDouble(val2);

            // Act
            bool result = calcDouble1 <= calcDouble2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AdditionSameUnitOperatorTest()
        {
            // Arrange
            var calcDouble1 = new CalcDouble(4.5, "l1", "L", "m");
            var calcDouble2 = new CalcDouble(5.5, "l2", "L", "m");

            // Act
            CalcDouble result = calcDouble1 + calcDouble2;

            // Assert
            Assert.Equal(10.0, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("m", result.Unit);
            Assert.Equal("l1 + l2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Arrange
            var calcDouble1 = new CalcDouble(4.5);
            var calcDouble2 = new CalcDouble(5.5);

            // Act
            CalcDouble result = calcDouble1 - calcDouble2;

            // Assert
            Assert.Equal(-1.0, result.Value);
        }

        [Fact]
        public void SubtractionSameUnitOperatorTest()
        {
            // Arrange
            var calcDouble1 = new CalcDouble(4.5, "l1", "L", "m");
            var calcDouble2 = new CalcDouble(5.5, "l2", "L", "m");

            // Act
            CalcDouble result = calcDouble1 - calcDouble2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("m", result.Unit);
            Assert.Equal("l1 - l2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void MultiplicationOperatorTest()
        {
            // Arrange
            var calcDouble1 = new CalcDouble(4.5);
            var calcDouble2 = new CalcDouble(5.5);

            // Act
            CalcDouble result = calcDouble1 * calcDouble2;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
        }

        [Fact]
        public void MultiplicationSameUnitOperatorTest()
        {
            // Arrange
            var calcDouble1 = new CalcDouble(4.5, "l1", "L", "m");
            var calcDouble2 = new CalcDouble(5.5, "l2", "L", "m");

            // Act
            CalcDouble result = calcDouble1 * calcDouble2;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("m²", result.Unit);
            Assert.Equal("l1 · l2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Arrange
            var calcDouble1 = new CalcDouble(4.5);
            var calcDouble2 = new CalcDouble(5.5);

            // Act
            CalcDouble result = calcDouble1 / calcDouble2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value);
        }

        [Fact]
        public void DivisionSameUnitOperatorTest()
        {
            // Arrange
            var calcDouble1 = new CalcDouble(4.5, "l1", "L", "m");
            var calcDouble2 = new CalcDouble(5.5, "l2", "L", "m");

            // Act
            CalcDouble result = calcDouble1 / calcDouble2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.True(string.IsNullOrEmpty(result.Unit));
            Assert.Equal("l1 / l2", result.DisplayName); // note: using Thin Space \u2009
        }
    }
}
