using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcVolumeTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicFoot, "volume", "V");

            // Act
            // Assert
            Assert.True(calcVolume.TryParse("5.5 cm^3"));
            Assert.Equal(5.5, calcVolume.Value);
            Assert.Equal("cm³", calcVolume.Unit);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicInch, "volume", "V");

            // Act
            double value = calcVolume;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicYard, "volume", "V");

            // Act
            Volume value = calcVolume;

            // Assert
            Assert.Equal(4.5, value.CubicYards);
            Assert.Equal(VolumeUnit.CubicYard, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Arrange
            var calcVolume1 = new CalcVolume(4.5, VolumeUnit.CubicCentimeter, "v1", "V");
            var calcVolume2 = new CalcVolume(5.5e-6, VolumeUnit.CubicMeter, "v2", "V");

            // Act
            CalcVolume result = calcVolume1 + calcVolume2;

            // Assert
            Assert.Equal(10, result.Value);
            Assert.Equal("V", result.Symbol);
            Assert.Equal("cm³", result.Unit);
            Assert.Equal("v1 + v2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Arrange
            var calcVolume1 = new CalcVolume(4.5, VolumeUnit.CubicCentimeter, "a1", "V");
            var calcVolume2 = new CalcVolume(5.5e-6, VolumeUnit.CubicMeter, "a2", "V");

            // Act
            CalcVolume result = calcVolume1 - calcVolume2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("V", result.Symbol);
            Assert.Equal("cm³", result.Unit);
            Assert.Equal("a1 - a2", result.DisplayName); // note: using Thin Space \u2009
        }


        [Fact]
        public void MultiplicationLengthOperatorTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicCentimeter, "a", "V");
            var calcLength = new CalcLength(0.055, LengthUnit.Meter, "l", "L");

            // Act
            CalcInertia result = calcVolume * calcLength;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("a · l", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Arrange
            var calcVolume1 = new CalcVolume(4.5, VolumeUnit.CubicCentimeter, "v1", "V");
            var calcVolume2 = new CalcVolume(5.5, VolumeUnit.CubicCentimeter, "v2", "V");

            // Act
            CalcDouble result = calcVolume1 / calcVolume2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("v1 / v2", result.DisplayName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.Unit));
        }

        [Fact]
        public void DivisionLengthOperatorTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicCentimeter, "v", "V");
            var calcLength = new CalcLength(0.055, LengthUnit.Meter, "l", "L");

            // Act
            CalcArea result = calcVolume / calcLength;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("v / l", result.DisplayName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm²", result.Unit);
        }

        [Fact]
        public void DivisionAreaOperatorTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicCentimeter, "v", "V");
            var calcArea = new CalcArea(0.00055, AreaUnit.SquareMeter, "a", "A");

            // Act
            CalcLength result = calcVolume / calcArea;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("v / a", result.DisplayName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm", result.Unit);
        }

        [Fact]
        public void AdditionDoubleOperatorTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicCentimeter, "a1", "A");

            // Act
            CalcVolume result = 2.0 + calcVolume;

            // Assert
            Assert.Equal(4.5 + 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm³", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcVolume.Value);
        }

        [Fact]
        public void SubtractionDoubleOperatorTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicCentimeter, "a1", "A");

            // Act
            CalcVolume result = calcVolume - 2;

            // Assert
            Assert.Equal(4.5 - 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm³", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcVolume.Value);
        }

        [Fact]
        public void MultiplicationDoubleOperatorTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicCentimeter, "a1", "A");

            // Act
            CalcVolume result = 2.0 * calcVolume;

            // Assert
            Assert.Equal(4.5 * 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm³", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcVolume.Value);
        }

        [Fact]
        public void DivisionDoubleOperatorTest()
        {
            // Arrange
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicCentimeter, "a1", "A");

            // Act
            CalcVolume result = calcVolume / 2;

            // Assert
            Assert.Equal(4.5 / 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm³", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcVolume.Value);
        }
    }
}
