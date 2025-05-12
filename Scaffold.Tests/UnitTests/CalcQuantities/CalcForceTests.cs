using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcForceTests
    {
        [Fact]
        public void TryParseFromStringTest()
        {
            // Assemble
            var calcForce = new CalcForce(4.5, ForceUnit.PoundForce, "force", "F");

            // Act
            // Assert
            Assert.True(CalcForce.TryParse("5.5 kN", null, out calcForce));
            Assert.Equal(5.5, calcForce.Value);
            Assert.Equal("kN", calcForce.Unit);
        }

        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            // Act
            var calcForce = CalcForce.Parse("5.5 kN", null);

            // Assert
            Assert.Equal(5.5, calcForce.Value);
            Assert.Equal("kN", calcForce.Unit);
        }

        [Fact]
        public void TryParseFailureTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareFoot, "area", "A");

            // Act
            // Assert
            Assert.False(CalcArea.TryParse("two hundred horses", null, out calcArea));
            Assert.Null(calcArea);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Assemble
            var calcForce = new CalcForce(4.5, "force", "F");

            // Act
            double value = calcForce;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Assemble
            var calcForce = new CalcForce(4.5, "force", "F");

            // Act
            Force value = calcForce;

            // Assert
            Assert.Equal(4.5, value.Kilonewtons);
            Assert.Equal(ForceUnit.Kilonewton, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Assemble
            var calcForce1 = new CalcForce(4.5, ForceUnit.Newton, "f1", "F");
            var calcForce2 = new CalcForce(5.5 / 1000, ForceUnit.Kilonewton, "f2", "F");

            // Act
            CalcForce result = calcForce1 + calcForce2;

            // Assert
            Assert.Equal(10, result.Value);
            Assert.Equal("F", result.Symbol);
            Assert.Equal("N", result.Unit);
            Assert.Equal("f1 + f2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Assemble
            var calcForce1 = new CalcForce(4.5, ForceUnit.Newton, "f1", "F");
            var calcForce2 = new CalcForce(5.5 / 1000, ForceUnit.Kilonewton, "f2", "F");

            // Act
            CalcForce result = calcForce1 - calcForce2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("F", result.Symbol);
            Assert.Equal("N", result.Unit);
            Assert.Equal("f1 - f2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void MultiplicationOperatorTest()
        {
            // Assemble
            var calcForce = new CalcForce(4.5, "f", "F");
            var calcLength = new CalcLength(5.5, LengthUnit.Meter, "l", "L");

            // Act
            CalcMoment result = calcForce * calcLength;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("kN·m", result.Unit);
            Assert.Equal("f · l", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Assemble
            var calcForce1 = new CalcForce(4.5, "f1", "F");
            var calcForce2 = new CalcForce(5.5, "f2", "F");

            // Act
            CalcDouble result = calcForce1 / calcForce2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("f1 / f2", result.DisplayName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.Unit));
        }

        [Fact]
        public void DivisionLengthOperatorTest()
        {
            // Assemble
            var calcForce = new CalcForce(4.5, ForceUnit.Kilonewton, "f", "F");
            var calcLength = new CalcLength(5.5, LengthUnit.Meter, "l", "L");

            // Act
            CalcLinearForce result = calcForce / calcLength;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value);
            Assert.Equal("f / l", result.DisplayName);  // note: using Thin Space
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("kN/m", result.Unit);
        }

        [Fact]
        public void DivisionAreaOperatorTest()
        {
            // Assemble
            var calcForce = new CalcForce(4.5, ForceUnit.Kilonewton, "f", "F");
            var calcArea = new CalcArea(5.5, AreaUnit.SquareMeter, "A", "A");

            // Act
            CalcStress result = calcForce / calcArea;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value);
            Assert.Equal("f / A", result.DisplayName);  // note: using Thin Space
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("kN/m²", result.Unit);
        }

        [Fact]
        public void AdditionDoubleOperatorTest()
        {
            // Arrange
            var calcForce = new CalcForce(4.5, ForceUnit.Newton, "a1", "A");

            // Act
            CalcForce result = 2.0 + calcForce;

            // Assert
            Assert.Equal(4.5 + 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("N", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcForce.Value);
        }

        [Fact]
        public void SubtractionDoubleOperatorTest()
        {
            // Arrange
            var calcForce = new CalcForce(4.5, ForceUnit.Newton, "a1", "A");

            // Act
            CalcForce result = calcForce - 2;

            // Assert
            Assert.Equal(4.5 - 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("N", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcForce.Value);
        }

        [Fact]
        public void MultiplicationDoubleOperatorTest()
        {
            // Arrange
            var calcForce = new CalcForce(4.5, ForceUnit.Newton, "a1", "A");

            // Act
            CalcForce result = 2.0 * calcForce;

            // Assert
            Assert.Equal(4.5 * 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("N", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcForce.Value);
        }

        [Fact]
        public void DivisionDoubleOperatorTest()
        {
            // Arrange
            var calcForce = new CalcForce(4.5, ForceUnit.Newton, "a1", "A");

            // Act
            CalcForce result = calcForce / 2;

            // Assert
            Assert.Equal(4.5 / 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("N", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcForce.Value);
        }

        [Fact]
        public void SumTest()
        {
            // Arrange
            var calcForce1 = new CalcForce(1, ForceUnit.Kilonewton, "a", "A");
            var calcForce2 = new CalcForce(2, ForceUnit.Kilonewton, "a", "A");
            var calcForce3 = new CalcForce(3, ForceUnit.Kilonewton, "a", "A");
            var forces = new List<CalcForce>() { calcForce1, calcForce2, calcForce3 };

            // Act
            CalcForce sum = forces.Sum();

            // Assert
            Assert.Equal(6, sum.Value);
            Assert.Equal("kN", sum.Unit);
        }

        [Fact]
        public void AverageTest()
        {
            // Arrange
            var calcForce1 = new CalcForce(1, ForceUnit.Kilonewton, "a", "A");
            var calcForce2 = new CalcForce(2, ForceUnit.Kilonewton, "a", "A");
            var calcForce3 = new CalcForce(3, ForceUnit.Kilonewton, "a", "A");
            var forces = new List<CalcForce>() { calcForce1, calcForce2, calcForce3 };

            // Act
            CalcForce sum = forces.Average();

            // Assert
            Assert.Equal(2, sum.Value);
            Assert.Equal("kN", sum.Unit);
        }
    }
}
