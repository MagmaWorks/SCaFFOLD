using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcLinearForceTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Assemble
            var calcForce = new CalcLinearForce(4.5, ForcePerLengthUnit.PoundForcePerFoot, "force", "w");

            // Act
            // Assert
            Assert.True(calcForce.TryParse("5.5 kN/m"));
            Assert.Equal(5.5, calcForce.Value);
            Assert.Equal("kN/m", calcForce.Unit);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Assemble
            var calcForce = new CalcLinearForce(4.5, "force", "w");

            // Act
            double value = calcForce;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Assemble
            var calcForce = new CalcLinearForce(4.5, "force", "w");

            // Act
            ForcePerLength value = calcForce;

            // Assert
            Assert.Equal(4.5, value.KilonewtonsPerMeter);
            Assert.Equal(ForcePerLengthUnit.KilonewtonPerMeter, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Assemble
            var calcForce1 = new CalcLinearForce(4.5, ForcePerLengthUnit.NewtonPerMeter, "w1", "w");
            var calcForce2 = new CalcLinearForce(5.5 / 1000, ForcePerLengthUnit.KilonewtonPerMeter, "w2", "w");

            // Act
            CalcLinearForce result = calcForce1 + calcForce2;

            // Assert
            Assert.Equal(10, result.Value);
            Assert.Equal("w", result.Symbol);
            Assert.Equal("N/m", result.Unit);
            Assert.Equal("w1 + w2", result.DisplayName);  // note: using Thin Space
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Assemble
            var calcForce1 = new CalcLinearForce(4.5, ForcePerLengthUnit.NewtonPerMeter, "w1", "w");
            var calcForce2 = new CalcLinearForce(5.5 / 1000, ForcePerLengthUnit.KilonewtonPerMeter, "w2", "w");

            // Act
            CalcLinearForce result = calcForce1 - calcForce2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("w", result.Symbol);
            Assert.Equal("N/m", result.Unit);
            Assert.Equal("w1 - w2", result.DisplayName);  // note: using Thin Space
        }

        [Fact]
        public void MultiplicationLengthOperatorTest()
        {
            // Assemble
            var calcForce = new CalcLinearForce(4.5, ForcePerLengthUnit.KilonewtonPerMeter, "w", "w");
            var calcLength = new CalcLength(5.5, LengthUnit.Meter, "l", "L");

            // Act
            CalcForce result = calcLength * calcForce; // reverse order to test both operators

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("kN", result.Unit);
            Assert.Equal("w · l", result.DisplayName);  // note: using Thin Space
        }

        [Fact]
        public void DivisionLengthOperatorTest()
        {
            // Assemble
            var calcForce = new CalcLinearForce(4.5, ForcePerLengthUnit.KilonewtonPerMeter, "w", "w");
            var calcLength = new CalcLength(5.5, LengthUnit.Meter, "l", "L");

            // Act
            CalcStress result = calcForce / calcLength;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value);
            Assert.Equal("w / l", result.DisplayName);  // note: using Thin Space
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("kN/m²", result.Unit);
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Assemble
            var calcForce1 = new CalcLinearForce(4.5, "w1", "w");
            var calcForce2 = new CalcLinearForce(5.5, "w2", "w");

            // Act
            CalcDouble result = calcForce1 / calcForce2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("w1 / w2", result.DisplayName);  // note: using Thin Space
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.Unit));
        }

        [Fact]
        public void AdditionDoubleOperatorTest()
        {
            // Arrange
            var calcLinearForce = new CalcLinearForce(4.5, ForcePerLengthUnit.KilonewtonPerMeter, "a1", "A");

            // Act
            CalcLinearForce result = 2.0 + calcLinearForce;

            // Assert
            Assert.Equal(4.5 + 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("kN/m", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLinearForce.Value);
        }

        [Fact]
        public void SubtractionDoubleOperatorTest()
        {
            // Arrange
            var calcLinearForce = new CalcLinearForce(4.5, ForcePerLengthUnit.KilonewtonPerMeter, "a1", "A");

            // Act
            CalcLinearForce result = calcLinearForce - 2;

            // Assert
            Assert.Equal(4.5 - 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("kN/m", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLinearForce.Value);
        }

        [Fact]
        public void MultiplicationDoubleOperatorTest()
        {
            // Arrange
            var calcLinearForce = new CalcLinearForce(4.5, ForcePerLengthUnit.KilonewtonPerMeter, "a1", "A");

            // Act
            CalcLinearForce result = 2.0 * calcLinearForce;

            // Assert
            Assert.Equal(4.5 * 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("kN/m", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLinearForce.Value);
        }

        [Fact]
        public void DivisionDoubleOperatorTest()
        {
            // Arrange
            var calcLinearForce = new CalcLinearForce(4.5, ForcePerLengthUnit.KilonewtonPerMeter, "a1", "A");

            // Act
            CalcLinearForce result = calcLinearForce / 2;

            // Assert
            Assert.Equal(4.5 / 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("kN/m", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcLinearForce.Value);
        }
    }
}
