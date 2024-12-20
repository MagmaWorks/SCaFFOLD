using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcMomentTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, MomentUnit.PoundalFoot, "moment", "M");

            // Act
            // Assert
            Assert.True(calcMoment.TryParse("5.5 kN·m"));
            Assert.Equal(5.5, calcMoment.Value);
            Assert.Equal("kN·m", calcMoment.Unit);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, "moment", "M");

            // Act
            double value = calcMoment;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, "moment", "M");

            // Act
            Moment value = calcMoment;

            // Assert
            Assert.Equal(4.5, value.KilonewtonMeters);
            Assert.Equal(MomentUnit.KilonewtonMeter, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Arrange
            var calcMoment1 = new CalcMoment(4.5, MomentUnit.NewtonMeter, "m1", "M");
            var calcMoment2 = new CalcMoment(5.5 / 1000, MomentUnit.KilonewtonMeter, "m2", "M");

            // Act
            CalcMoment result = calcMoment1 + calcMoment2;

            // Assert
            Assert.Equal(10, result.Value);
            Assert.Equal("M", result.Symbol);
            Assert.Equal("N·m", result.Unit);
            Assert.Equal("m1 + m2", result.DisplayName);  // note: using Thin Space
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Arrange
            var calcMoment1 = new CalcMoment(4.5, MomentUnit.NewtonMeter, "m1", "M");
            var calcMoment2 = new CalcMoment(5.5 / 1000, MomentUnit.KilonewtonMeter, "m2", "M");

            // Act
            CalcMoment result = calcMoment1 - calcMoment2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("M", result.Symbol);
            Assert.Equal("N·m", result.Unit);
            Assert.Equal("m1 - m2", result.DisplayName);  // note: using Thin Space
        }

        [Fact]
        public void DivisionLengthOperatorTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, "m", "M");
            var calcLength = new CalcLength(5.5, LengthUnit.Meter, "l", "L");

            // Act
            CalcForce result = calcMoment / calcLength;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("m / l", result.DisplayName);  // note: using Thin Space
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("kN", result.Unit);
        }

        [Fact]
        public void DivisionForceOperatorTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, "m", "M");
            var calcForce = new CalcForce(5.5, ForceUnit.Kilonewton, "f", "F");

            // Act
            CalcLength result = calcMoment / calcForce;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("m / f", result.DisplayName);  // note: using Thin Space
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("m", result.Unit);
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Arrange
            var calcMoment1 = new CalcMoment(4.5, "m1", "M");
            var calcMoment2 = new CalcMoment(5.5, "m2", "M");

            // Act
            CalcDouble result = calcMoment1 / calcMoment2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("m1 / m2", result.DisplayName);  // note: using Thin Space
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.Unit));
        }

        [Fact]
        public void MakeLinearTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, "m", "M");

            // Act
            CalcLinearMoment result = calcMoment.MakeLinear();

            // Assert
            Assert.Equal(4.5, result.Value, 12);
            Assert.Equal("m", result.DisplayName);
            Assert.Equal("kN·m/m", result.Unit);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
        }

        [Fact]
        public void MakeLinearFromLengthTest()
        {
            // Arrange
            var calcMoment = new CalcMoment(4.5, "m", "M");
            var calcLength = new CalcLength(5.5, LengthUnit.Centimeter, "l", "L");

            // Act
            CalcLinearMoment result = calcMoment.MakeLinear(calcLength);

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("m", result.DisplayName);
            Assert.Equal("kN·m/cm", result.Unit);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
        }
    }
}
