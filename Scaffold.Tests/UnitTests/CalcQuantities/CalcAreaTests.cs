﻿using UnitsNet;
using UnitsNet.Units;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcAreaTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareFoot, "area", "A");

            // Act
            // Assert
            Assert.True(calcArea.TryParse("5.5 cm²"));
            Assert.Equal(5.5, calcArea.Value);
            Assert.Equal("cm²", calcArea.Unit);
        }

        [Fact]
        public void ImplicitOperatorDoubleTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareInch, "area", "A");

            // Act
            double value = calcArea;

            // Assert
            Assert.Equal(4.5, value);
        }

        [Fact]
        public void ImplicitOperatorQuantityTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareYard, "area", "A");

            // Act
            Area value = calcArea;

            // Assert
            Assert.Equal(4.5, value.SquareYards);
            Assert.Equal(AreaUnit.SquareYard, value.Unit);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Arrange
            var calcArea1 = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a1", "A");
            var calcArea2 = new CalcArea(0.00055, AreaUnit.SquareMeter, "a2", "A");

            // Act
            CalcArea result = calcArea1 + calcArea2;

            // Assert
            Assert.Equal(10, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm²", result.Unit);
            Assert.Equal("a1 + a2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void SubtractionOperatorTest()
        {
            // Arrange
            var calcArea1 = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a1", "A");
            var calcArea2 = new CalcArea(0.00055, AreaUnit.SquareMeter, "a2", "A");

            // Act
            CalcArea result = calcArea1 - calcArea2;

            // Assert
            Assert.Equal(-1, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm²", result.Unit);
            Assert.Equal("a1 - a2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void MultiplicationOperatorTest()
        {
            // Arrange
            var calcArea1 = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a1", "A");
            var calcArea2 = new CalcArea(0.00055, AreaUnit.SquareMeter, "a2", "A");

            // Act
            CalcInertia result = calcArea1 * calcArea2;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm⁴", result.Unit);
            Assert.Equal("a1 · a2", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void MultiplicationLengthOperatorTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a", "A");
            var calcLength = new CalcLength(0.055, LengthUnit.Meter, "l", "L");

            // Act
            CalcVolume result = calcArea * calcLength;

            // Assert
            Assert.Equal(4.5 * 5.5, result.Value);
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm³", result.Unit);
            Assert.Equal("a · l", result.DisplayName); // note: using Thin Space \u2009
        }

        [Fact]
        public void DivisionOperatorTest()
        {
            // Arrange
            var calcArea1 = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a1", "A");
            var calcArea2 = new CalcArea(5.5, AreaUnit.SquareCentimeter, "a2", "A");

            // Act
            CalcDouble result = calcArea1 / calcArea2;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("a1 / a2", result.DisplayName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.True(string.IsNullOrEmpty(result.Unit));
        }

        [Fact]
        public void DivisionLengthOperatorTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a", "A");
            var calcLength = new CalcLength(0.055, LengthUnit.Meter, "l", "L");

            // Act
            CalcLength result = calcArea / calcLength;

            // Assert
            Assert.Equal(4.5 / 5.5, result.Value, 12);
            Assert.Equal("a / l", result.DisplayName); // note: using Thin Space \u2009
            Assert.True(string.IsNullOrEmpty(result.Symbol));
            Assert.Equal("cm", result.Unit);
        }

        [Fact]
        public void AdditionDoubleOperatorTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a1", "A");

            // Act
            CalcArea result = 2.0 + calcArea;

            // Assert
            Assert.Equal(4.5 + 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm²", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcArea.Value);
        }

        [Fact]
        public void SubtractionDoubleOperatorTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a1", "A");

            // Act
            CalcArea result = calcArea - 2;

            // Assert
            Assert.Equal(4.5 - 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm²", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcArea.Value);
        }

        [Fact]
        public void MultiplicationDoubleOperatorTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a1", "A");

            // Act
            CalcArea result = 2.0 * calcArea;

            // Assert
            Assert.Equal(4.5 * 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm²", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcArea.Value);
        }

        [Fact]
        public void DivisionDoubleOperatorTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareCentimeter, "a1", "A");

            // Act
            CalcArea result = calcArea / 2;

            // Assert
            Assert.Equal(4.5 / 2, result.Value);
            Assert.Equal("A", result.Symbol);
            Assert.Equal("cm²", result.Unit);
            Assert.Equal("a1", result.DisplayName);
            Assert.Equal(4.5, calcArea.Value);
        }
    }
}
