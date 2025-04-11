using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core.CalcQuantities;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcLengthTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Meter, "length", "L");

            // Act
            // Assert
            Assert.True(calcLength.TryParse("5.5 cm"));
            Assert.Equal(5.5, calcLength.Value);
            Assert.Equal("cm", calcLength.Unit);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Meter, "length", "L");

            // Act
            double value = calcLength;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Meter, "length", "L");

            // Act
            Length value = calcLength;

            // Assert
            Assert.Equal(4.5, value.Meters);
            Assert.Equal(LengthUnit.Meter, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Arrange
            var calcLength1 = new CalcLength(4.5, LengthUnit.Centimeter, "l1", "L");
            var calcLength2 = new CalcLength(0.055, LengthUnit.Meter, "l2", "L");

            // Act
            CalcLength result = calcLength1 + calcLength2;

            // Assert
            Assert.Equal(10, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("cm", result.Unit);
            Assert.Equal("l1 + l2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Arrange
            var calcLength1 = new CalcLength(4.5, LengthUnit.Centimeter, "l1", "L");
            var calcLength2 = new CalcLength(0.055, LengthUnit.Meter, "l2", "L");

            // Act
            CalcLength result = calcLength1 - calcLength2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("cm", result.Unit);
            Assert.Equal("l1 - l2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void MultiplicationOperatorTest()
        {
            // Arrange
            var calcLength1 = new CalcLength(4.5, LengthUnit.Centimeter, "l1", "L");
            var calcLength2 = new CalcLength(0.055, LengthUnit.Meter, "l2", "L");

            // Act
            CalcArea result = calcLength1 * calcLength2;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("cm²", result.Unit);
            Assert.Equal("l1 · l2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void MultiplicationAreaOperatorTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Centimeter, "l", "L");
            var calcArea = new CalcArea(5.5, AreaUnit.SquareCentimeter, "a", "A");

            // Act
            CalcVolume result = calcLength * calcArea;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm³", result.Unit);
            Assert.Equal("l · a", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void MultiplicationVolumeOperatorTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Centimeter, "l", "L");
            var calcArea = new CalcVolume(5.5, VolumeUnit.CubicCentimeter, "v", "V");

            // Act
            CalcInertia result = calcLength * calcArea;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("l · v", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Arrange
            var calcLength1 = new CalcLength(4.5, LengthUnit.Centimeter, "l1", "L");
            var calcLength2 = new CalcLength(5.5, LengthUnit.Centimeter, "l2", "L");

            // Act
            CalcStrain result = calcLength1 / calcLength2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("l1 / l2", result.DisplayName); // note: using Thin Space \u2009
            Assert.Equal("ε", result.Unit);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
        }

        [Fact]
        public void AdditionDoubleOperatorTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Centimeter, "a1", "A");

            // Act
            CalcLength result = 2.0 + calcLength;

            // Assert
            Assert.Equal(4.5 + 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLength.Value);
        }

        [Fact]
        public void SubtractionDoubleOperatorTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Centimeter, "a1", "A");

            // Act
            CalcLength result = calcLength - 2;

            // Assert
            Assert.Equal(4.5 - 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLength.Value);
        }

        [Fact]
        public void MultiplicationDoubleOperatorTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Centimeter, "a1", "A");

            // Act
            CalcLength result = 2.0 * calcLength;

            // Assert
            Assert.Equal(4.5 * 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLength.Value);
        }

        [Fact]
        public void DivisionDoubleOperatorTest()
        {
            // Arrange
            var calcLength = new CalcLength(4.5, LengthUnit.Centimeter, "a1", "A");

            // Act
            CalcLength result = calcLength / 2;

            // Assert
            Assert.Equal(4.5 / 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLength.Value);
        }
    }
}
