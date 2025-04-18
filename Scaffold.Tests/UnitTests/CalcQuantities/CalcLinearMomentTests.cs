using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcLinearMomentTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            var calcMoment = new CalcLinearMoment(4.5, MomentPerLengthUnit.PoundForceFootPerFoot, "moment", "M");

            // Act
            // Assert
            Assert.True(calcMoment.TryParse("5.5 N·cm/m"));
            Assert.Equal(5.5, calcMoment.Value);
            Assert.Equal("N·cm/m", calcMoment.Unit);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Arrange
            var calcMoment = new CalcLinearMoment(4.5, "moment", "M");

            // Act
            double value = calcMoment;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Arrange
            var calcMoment = new CalcLinearMoment(4.5, "moment", "M");

            // Act
            MomentPerLength value = calcMoment;

            // Assert
            Assert.Equal(4.5, value.KilonewtonMetersPerMeter);
            Assert.Equal(MomentPerLengthUnit.KilonewtonMeterPerMeter, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Arrange
            var calcMoment1 = new CalcLinearMoment(4.5, MomentPerLengthUnit.NewtonMeterPerMeter, "m1", "M");
            var calcMoment2 = new CalcLinearMoment(5.5 / 1000, MomentPerLengthUnit.KilonewtonMeterPerMeter, "m2", "M");

            // Act
            CalcLinearMoment result = calcMoment1 + calcMoment2;

            // Assert
            Assert.Equal(10, result.Value);
            Assert.Equal("M", result.Symbol);
            Assert.Equal("N·m/m", result.Unit);
            Assert.Equal("m1 + m2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Arrange
            var calcMoment1 = new CalcLinearMoment(4.5, MomentPerLengthUnit.NewtonMeterPerMeter, "m1", "M");
            var calcMoment2 = new CalcLinearMoment(5.5 / 1000, MomentPerLengthUnit.KilonewtonMeterPerMeter, "m2", "M");

            // Act
            CalcLinearMoment result = calcMoment1 - calcMoment2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("M", result.Symbol);
            Assert.Equal("N·m/m", result.Unit);
            Assert.Equal("m1 - m2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void MultiplicationLengthOperatorTest()
        {
            // Arrange
            var calcMoment = new CalcLinearMoment(4.5, MomentPerLengthUnit.KilonewtonMeterPerMeter, "m", "M");
            var calcLength = new CalcLength(5.5, LengthUnit.Meter, "l", "L");

            // Act
            CalcMoment result = calcLength * calcMoment; // reverse order to test both operators

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("kN·m", result.Unit);
            Assert.Equal("m · l", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Arrange
            var calcMoment1 = new CalcLinearMoment(4.5, "m1", "M");
            var calcMoment2 = new CalcLinearMoment(5.5, "m2", "M");

            // Act
            CalcDouble result = calcMoment1 / calcMoment2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("m1 / m2", result.DisplayName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.Unit));
        }

        [Fact]
        public void AdditionDoubleOperatorTest()
        {
            // Arrange
            var calcLinearMoment = new CalcLinearMoment(4.5, MomentPerLengthUnit.KilonewtonMeterPerMeter, "a1", "A");

            // Act
            CalcLinearMoment result = 2.0 + calcLinearMoment;

            // Assert
            Assert.Equal(4.5 + 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("kN·m/m", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLinearMoment.Value);
        }

        [Fact]
        public void SubtractionDoubleOperatorTest()
        {
            // Arrange
            var calcLinearMoment = new CalcLinearMoment(4.5, MomentPerLengthUnit.KilonewtonMeterPerMeter, "a1", "A");

            // Act
            CalcLinearMoment result = calcLinearMoment - 2;

            // Assert
            Assert.Equal(4.5 - 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("kN·m/m", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLinearMoment.Value);
        }

        [Fact]
        public void MultiplicationDoubleOperatorTest()
        {
            // Arrange
            var calcLinearMoment = new CalcLinearMoment(4.5, MomentPerLengthUnit.KilonewtonMeterPerMeter, "a1", "A");

            // Act
            CalcLinearMoment result = 2.0 * calcLinearMoment;

            // Assert
            Assert.Equal(4.5 * 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("kN·m/m", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLinearMoment.Value);
        }

        [Fact]
        public void DivisionDoubleOperatorTest()
        {
            // Arrange
            var calcLinearMoment = new CalcLinearMoment(4.5, MomentPerLengthUnit.KilonewtonMeterPerMeter, "a1", "A");

            // Act
            CalcLinearMoment result = calcLinearMoment / 2;

            // Assert
            Assert.Equal(4.5 / 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("kN·m/m", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLinearMoment.Value);
        }
    }
}
