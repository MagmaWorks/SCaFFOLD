using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core.CalcValues;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcLengthTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Assemble
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
            // Assemble
            var calcLength = new CalcLength(4.5, LengthUnit.Meter, "length", "L");

            // Act
            double value = calcLength;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorLengthTest()
        {
            // Assemble
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
            // Assemble
            var calcLength1 = new CalcLength(4.5, LengthUnit.Centimeter, "l1", "L");
            var calcLength2 = new CalcLength(0.055, LengthUnit.Meter, "l2", "L");

            // Act
            CalcLength result = calcLength1 + calcLength2;

            // Assert
            Assert.Equal(10, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("cm", result.Unit);
            Assert.Equal("l1+l2", result.DisplayName);
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Assemble
            var calcLength1 = new CalcLength(4.5, LengthUnit.Centimeter, "l1", "L");
            var calcLength2 = new CalcLength(0.055, LengthUnit.Meter, "l2", "L");

            // Act
            CalcLength result = calcLength1 - calcLength2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("cm", result.Unit);
            Assert.Equal("l1-l2", result.DisplayName);
        }

        [Fact]
        public void MultiplicationOperatorTest()
        {
            // Assemble
            var calcLength1 = new CalcLength(4.5, LengthUnit.Centimeter, "l1", "L");
            var calcLength2 = new CalcLength(0.055, LengthUnit.Meter, "l2", "L");

            // Act
            CalcArea result = calcLength1 * calcLength2;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("cm²", result.Unit);
            Assert.Equal("l1·l2", result.DisplayName);
        }

        [Fact]
        public void MultiplicationAreaOperatorTest()
        {
            // Assemble
            var calcLength = new CalcLength(4.5, LengthUnit.Centimeter, "l", "L");
            var calcArea = new CalcArea(5.5, AreaUnit.SquareCentimeter, "a", "A");

            // Act
            CalcVolume result = calcLength * calcArea;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm³", result.Unit);
            Assert.Equal("l·a", result.DisplayName);
        }

        [Fact]
        public void MultiplicationVolumeOperatorTest()
        {
            // Assemble
            var calcLength = new CalcLength(4.5, LengthUnit.Centimeter, "l", "L");
            var calcArea = new CalcVolume(5.5, VolumeUnit.CubicCentimeter, "v", "V");

            // Act
            CalcInertia result = calcLength * calcArea;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("l·v", result.DisplayName);
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Assemble
            var calcLength1 = new CalcLength(4.5, LengthUnit.Centimeter, "l1", "L");
            var calcLength2 = new CalcLength(5.5, LengthUnit.Centimeter, "l2", "L");

            // Act
            CalcDouble result = calcLength1 / calcLength2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("l1/l2", result.DisplayName);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.Unit));
        }
    }
}
