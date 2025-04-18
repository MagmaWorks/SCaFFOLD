using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcStressTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Assemble
            var calcStress = new CalcStress(4.5, PressureUnit.PoundForcePerSquareInch, "stress", "σ");

            // Act
            // Assert
            Assert.True(calcStress.TryParse("5.5 kPa"));
            Assert.Equal(5.5, calcStress.Value);
            Assert.Equal("kPa", calcStress.Unit);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Assemble
            var calcStress = new CalcStress(4.5, PressureUnit.KilonewtonPerSquareMeter, "stress", "σ");

            // Act
            double value = calcStress;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Assemble
            var calcStress = new CalcStress(4.5, PressureUnit.KilonewtonPerSquareMeter, "stress", "σ");

            // Act
            Pressure value = calcStress;

            // Assert
            Assert.Equal(4.5, value.KilonewtonsPerSquareMeter);
            Assert.Equal(PressureUnit.KilonewtonPerSquareMeter, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Assemble
            var calcStress1 = new CalcStress(4.5, PressureUnit.Pascal, "σ1", "σ");
            var calcStress2 = new CalcStress(5.5 / 1000, PressureUnit.Kilopascal, "σ2", "σ");

            // Act
            CalcStress result = calcStress1 + calcStress2;

            // Assert
            Assert.Equal(10, result.Value);
            Assert.Equal("σ", result.Symbol);
            Assert.Equal("Pa", result.Unit);
            Assert.Equal("σ1 + σ2", result.DisplayName);
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Assemble
            var calcStress1 = new CalcStress(4.5, PressureUnit.Pascal, "σ1", "σ");
            var calcStress2 = new CalcStress(5.5 / 1000, PressureUnit.Kilopascal, "σ2", "σ");

            // Act
            CalcStress result = calcStress1 - calcStress2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("σ", result.Symbol);
            Assert.Equal("Pa", result.Unit);
            Assert.Equal("σ1 - σ2", result.DisplayName);
        }

        [Fact]
        public void MultiplicationLengthOperatorTest()
        {
            // Assemble
            var calcStress = new CalcStress(4.5, PressureUnit.Kilopascal, "σ", "σ");
            var calcLength = new CalcLength(5.5, LengthUnit.Meter, "l", "L");

            // Act
            CalcLinearForce result = calcLength * calcStress; // reverse order to test both operators

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("kN/m", result.Unit);
            Assert.Equal("σ · l", result.DisplayName);
        }

        [Fact]
        public void MultiplicationLengthOperatorTest2()
        {
            // Assemble
            var calcStress = new CalcStress(4.5, PressureUnit.Megapascal, "σ", "σ");
            var calcLength = new CalcLength(5.5, LengthUnit.Meter, "l", "L");

            // Act
            CalcLinearForce result = calcStress * calcLength;

            // Assert
            Assert.Equal(4.5 * 5.5 * 1000, result.Value, 12);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("N/mm", result.Unit);
            Assert.Equal("σ · l", result.DisplayName);
        }

        [Fact]
        public void MultiplicationLengthOperatorTest3()
        {
            // Assemble
            var calcStress = new CalcStress(4.5, PressureUnit.KilonewtonPerSquareCentimeter, "σ", "σ");
            var calcLength = new CalcLength(5.5, LengthUnit.Centimeter, "l", "L");

            // Act
            CalcLinearForce result = calcStress * calcLength;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value, 12);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("kN/cm", result.Unit);
            Assert.Equal("σ · l", result.DisplayName);
        }

        [Fact]
        public void MultiplicationAreaOperatorTest()
        {
            // Assemble
            var calcStress = new CalcStress(4.5, PressureUnit.Kilopascal, "σ", "σ");
            var calcArea = new CalcArea(5.5, AreaUnit.SquareMeter, "A", "A");

            // Act
            CalcForce result = calcArea * calcStress; // reverse order to test both operators

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("kN", result.Unit);
            Assert.Equal("σ · A", result.DisplayName);
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Assemble
            var calcStress1 = new CalcStress(4.5, PressureUnit.Kilopascal, "σ1", "σ");
            var calcStress2 = new CalcStress(5.5, PressureUnit.Kilopascal, "σ2", "σ");

            // Act
            CalcDouble result = calcStress1 / calcStress2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("σ1 / σ2", result.DisplayName);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.Unit));
        }

        [Fact]
        public void AdditionDoubleOperatorTest()
        {
            // Arrange
            var calcStress = new CalcStress(4.5, PressureUnit.Megapascal, "a1", "A");

            // Act
            CalcStress result = 2.0 + calcStress;

            // Assert
            Assert.Equal(4.5 + 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("MPa", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcStress.Value);
        }

        [Fact]
        public void SubtractionDoubleOperatorTest()
        {
            // Arrange
            var calcStress = new CalcStress(4.5, PressureUnit.Megapascal, "a1", "A");

            // Act
            CalcStress result = calcStress - 2;

            // Assert
            Assert.Equal(4.5 - 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("MPa", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcStress.Value);
        }

        [Fact]
        public void MultiplicationDoubleOperatorTest()
        {
            // Arrange
            var calcStress = new CalcStress(4.5, PressureUnit.Megapascal, "a1", "A");

            // Act
            CalcStress result = 2.0 * calcStress;

            // Assert
            Assert.Equal(4.5 * 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("MPa", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcStress.Value);
        }

        [Fact]
        public void DivisionDoubleOperatorTest()
        {
            // Arrange
            var calcStress = new CalcStress(4.5, PressureUnit.Megapascal, "a1", "A");

            // Act
            CalcStress result = calcStress / 2;

            // Assert
            Assert.Equal(4.5 / 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("MPa", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcStress.Value);
        }
    }
}
