using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core.CalcQuantities;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcStrainTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            var calcStrain = new CalcStrain(4.5, StrainUnit.Ratio, "ratio", "r");

            // Act
            // Assert
            Assert.True(calcStrain.TryParse("5.5 %"));
            Assert.Equal(5.5, calcStrain.Value);
            Assert.Equal("%", calcStrain.Unit);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Arrange
            var calcStrain = new CalcStrain(4.5, StrainUnit.Ratio, "ratio", "r");

            // Act
            double value = calcStrain;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Arrange
            var calcStrain = new CalcStrain(4.5, StrainUnit.Ratio, "ratio", "r");

            // Act
            Strain value = calcStrain;

            // Assert
            Assert.Equal(4.5, value.Ratio);
            Assert.Equal(StrainUnit.Ratio, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Arrange
            var calcStrain1 = new CalcStrain(4.5, StrainUnit.Percent, "r1", "R");
            var calcStrain2 = new CalcStrain(0.055, StrainUnit.Ratio, "r2", "R");

            // Act
            CalcStrain result = calcStrain1 + calcStrain2;

            // Assert
            Assert.Equal(10, result.Value);
            Assert.Equal("R", result.Symbol);
            Assert.Equal("%", result.Unit);
            Assert.Equal("r1 + r2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Arrange
            var calcStrain1 = new CalcStrain(4.5, StrainUnit.Percent, "r1", "R");
            var calcStrain2 = new CalcStrain(0.055, StrainUnit.Ratio, "r2", "R");

            // Act
            CalcStrain result = calcStrain1 - calcStrain2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("R", result.Symbol);
            Assert.Equal("%", result.Unit);
            Assert.Equal("r1 - r2", result.DisplayName); // note: using Thin Space \u2009
        }
    }
}
