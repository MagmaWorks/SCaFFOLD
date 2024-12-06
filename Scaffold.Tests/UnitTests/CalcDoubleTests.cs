using Scaffold.Core.CalcValues;

namespace Scaffold.Tests.UnitTests
{
    public class CalcDoubleTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Assemble
            var calcDouble = new CalcDouble(4.5);

            // Act
            // Assert
            Assert.True(calcDouble.TryParse("5.5"));
            Assert.Equal(5.5, calcDouble.Value);
        }

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
        public void AdditionSameUnitOperatorTest()
        {
            // Assemble
            var calcDouble1 = new CalcDouble("l1", "L", 4.5, "m");
            var calcDouble2 = new CalcDouble("l2", "L", 5.5, "m");

            // Act
            CalcDouble result = calcDouble1 + calcDouble2;

            // Assert
            Assert.Equal(10.0, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("m", result.Unit);
            Assert.Equal("l1 + l2", result.DisplayName);
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
        public void SubtractionSameUnitOperatorTest()
        {
            // Assemble
            var calcDouble1 = new CalcDouble("l1", "L", 4.5, "m");
            var calcDouble2 = new CalcDouble("l2", "L", 5.5, "m");

            // Act
            CalcDouble result = calcDouble1 - calcDouble2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("m", result.Unit);
            Assert.Equal("l1 - l2", result.DisplayName);
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
        public void MultiplicationSameUnitOperatorTest()
        {
            // Assemble
            var calcDouble1 = new CalcDouble("l1", "L", 4.5, "m");
            var calcDouble2 = new CalcDouble("l2", "L", 5.5, "m");

            // Act
            CalcDouble result = calcDouble1 * calcDouble2;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("m²", result.Unit);
            Assert.Equal("l1 * l2", result.DisplayName);
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

        [Fact]
        public void DivisionSameUnitOperatorTest()
        {
            // Assemble
            var calcDouble1 = new CalcDouble("l1", "L", 4.5, "m");
            var calcDouble2 = new CalcDouble("l2", "L", 5.5, "m");

            // Act
            CalcDouble result = calcDouble1 / calcDouble2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.True(string.IsNullOrEmpty(result.Unit));
            Assert.Equal("l1 / l2", result.DisplayName);
        }
    }
}
