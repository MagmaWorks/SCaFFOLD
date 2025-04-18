using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcInertiaTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.FootToTheFourth, "inertia", "I");

            // Act
            // Assert
            Assert.True(calcInertia.TryParse("5.5 cm⁴"));
            Assert.Equal(5.5, calcInertia.Value);
            Assert.Equal("cm⁴", calcInertia.Unit);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.InchToTheFourth, "inertia", "I");

            // Act
            double value = calcInertia;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.DecimeterToTheFourth, "inertia", "I");

            // Act
            AreaMomentOfInertia value = calcInertia;

            // Assert
            Assert.Equal(4.5, value.DecimetersToTheFourth);
            Assert.Equal(AreaMomentOfInertiaUnit.DecimeterToTheFourth, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Arrange
            var calcInertia1 = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "i1", "I");
            var calcInertia2 = new CalcInertia(5.5e-8, AreaMomentOfInertiaUnit.MeterToTheFourth, "i2", "I");

            // Act
            CalcInertia result = calcInertia1 + calcInertia2;

            // Assert
            Assert.Equal(10, result.Value);
            Assert.Equal("I", result.Symbol);
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("i1 + i2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Arrange
            var calcInertia1 = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "i1", "I");
            var calcInertia2 = new CalcInertia(5.5e-8, AreaMomentOfInertiaUnit.MeterToTheFourth, "i2", "I");

            // Act
            CalcInertia result = calcInertia1 - calcInertia2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("I", result.Symbol);
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("i1 - i2", result.DisplayName); // note: using Thin Space \u2009
        }


        [Fact]
        public void DivisionOperatorTest()
        {
            // Arrange
            var calcInertia1 = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "i1", "I");
            var calcInertia2 = new CalcInertia(5.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "i2", "I");

            // Act
            CalcDouble result = calcInertia1 / calcInertia2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("i1 / i2", result.DisplayName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.Unit));
        }

        [Fact]
        public void DivisionLengthOperatorTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "i", "I");
            var calcLength = new CalcLength(0.055, LengthUnit.Meter, "l", "L");

            // Act
            CalcVolume result = calcInertia / calcLength;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("i / l", result.DisplayName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm³", result.Unit);
        }

        [Fact]
        public void DivisionAreaOperatorTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "i", "I");
            var calcArea = new CalcArea(0.00055, AreaUnit.SquareMeter, "a", "A");

            // Act
            CalcArea result = calcInertia / calcArea;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("i / a", result.DisplayName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm²", result.Unit);
        }

        [Fact]
        public void DivisionVolumeOperatorTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "i", "I");
            var calcVolume = new CalcVolume(5.5e-6, VolumeUnit.CubicMeter, "v", "V");

            // Act
            CalcLength result = calcInertia / calcVolume;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("i / v", result.DisplayName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm", result.Unit);
        }

        [Fact]
        public void AdditionDoubleOperatorTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "a1", "I");

            // Act
            CalcInertia result = 2.0 + calcInertia;

            // Assert
            Assert.Equal(4.5 + 2, result.Value);
            Assert.Equal("I", result.Symbol);
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcInertia.Value);
        }

        [Fact]
        public void SubtractionDoubleOperatorTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "a1", "I");

            // Act
            CalcInertia result = calcInertia - 2;

            // Assert
            Assert.Equal(4.5 - 2, result.Value);
            Assert.Equal("I", result.Symbol);
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcInertia.Value);
        }

        [Fact]
        public void MultiplicationDoubleOperatorTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "a1", "I");

            // Act
            CalcInertia result = 2.0 * calcInertia;

            // Assert
            Assert.Equal(4.5 * 2, result.Value);
            Assert.Equal("I", result.Symbol);
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcInertia.Value);
        }

        [Fact]
        public void DivisionDoubleOperatorTest()
        {
            // Arrange
            var calcInertia = new CalcInertia(4.5, AreaMomentOfInertiaUnit.CentimeterToTheFourth, "a1", "I");

            // Act
            CalcInertia result = calcInertia / 2;

            // Assert
            Assert.Equal(4.5 / 2, result.Value);
            Assert.Equal("I", result.Symbol);
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcInertia.Value);
        }
    }
}
