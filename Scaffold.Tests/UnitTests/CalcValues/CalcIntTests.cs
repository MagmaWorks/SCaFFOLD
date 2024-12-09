using Scaffold.Core.CalcValues;

namespace Scaffold.Tests.UnitTests.CalcValues
{
    public class CalcIntTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Assemble
            var calcInt = new CalcInt(4);

            // Act
            // Assert
            Assert.True(calcInt.TryParse("5"));
            Assert.Equal(5, calcInt.Value);
        }

        [Fact]
        public void ImplicitOperatorTest()
        {
            // Assemble
            var calcInt1 = new CalcInt(4);

            // Act
            int result = calcInt1;

            // Assert
            Assert.Equal(4, result);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Assemble
            var calcInt1 = new CalcInt(4);
            var calcInt2 = new CalcInt(5);

            // Act
            CalcInt result = calcInt1 + calcInt2;

            // Assert
            Assert.Equal(9, result.Value);
        }

        [Fact]
        public void AdditionSameUnitOperatorTest()
        {
            // Assemble
            var calcInt1 = new CalcInt(4, "l1", "L", "m");
            var calcInt2 = new CalcInt(5, "l2", "L", "m");

            // Act
            CalcInt result = calcInt1 + calcInt2;

            // Assert
            Assert.Equal(9, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("m", result.Unit);
            Assert.Equal("l1+l2", result.DisplayName);
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Assemble
            var calcInt1 = new CalcInt(4);
            var calcInt2 = new CalcInt(5);

            // Act
            CalcInt result = calcInt1 - calcInt2;

            // Assert
            Assert.Equal(-1.0, result.Value);
        }

        [Fact]
        public void SubtractionSameUnitOperatorTest()
        {
            // Assemble
            var calcInt1 = new CalcInt(4, "l1", "L", "m");
            var calcInt2 = new CalcInt(5, "l2", "L", "m");

            // Act
            CalcInt result = calcInt1 - calcInt2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("m", result.Unit);
            Assert.Equal("l1-l2", result.DisplayName);
        }

        [Fact]
        public void MultiplicationOperatorTest()
        {
            // Assemble
            var calcInt1 = new CalcInt(4);
            var calcInt2 = new CalcInt(5);

            // Act
            CalcInt result = calcInt1 * calcInt2;

            // Assert
            Assert.Equal(4 * 5, result.Value);
        }

        [Fact]
        public void MultiplicationSameUnitOperatorTest()
        {
            // Assemble
            var calcInt1 = new CalcInt(4, "l1", "L", "m");
            var calcInt2 = new CalcInt(5, "l2", "L", "m");

            // Act
            CalcInt result = calcInt1 * calcInt2;

            // Assert
            Assert.Equal(4 * 5, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("m²", result.Unit);
            Assert.Equal("l1·l2", result.DisplayName);
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Assemble
            var calcInt1 = new CalcInt(4);
            var calcInt2 = new CalcInt(5);

            // Act
            CalcInt result = calcInt1 / calcInt2;

            // Assert
            Assert.Equal(4 / 5, result.Value);
        }

        [Fact]
        public void DivisionSameUnitOperatorTest()
        {
            // Assemble
            var calcInt1 = new CalcInt(4, "l1", "L", "m");
            var calcInt2 = new CalcInt(5, "l2", "L", "m");

            // Act
            CalcInt result = calcInt1 / calcInt2;

            // Assert
            Assert.Equal(4 / 5, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.True(string.IsNullOrEmpty(result.Unit));
            Assert.Equal("l1/l2", result.DisplayName);
        }
    }
}
