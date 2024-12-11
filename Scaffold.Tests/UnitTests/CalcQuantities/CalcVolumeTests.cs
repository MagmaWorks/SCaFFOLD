using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcVolumeTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Assemble
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
            // Assemble
            var calcVolume = new CalcVolume(4.5, VolumeUnit.CubicInch, "volume", "V");

            // Act
            double value = calcVolume;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorLengthTest()
        {
            // Assemble
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
            // Assemble
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
            // Assemble
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
            // Assemble
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
            // Assemble
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
            // Assemble
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
            // Assemble
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
    }
}
