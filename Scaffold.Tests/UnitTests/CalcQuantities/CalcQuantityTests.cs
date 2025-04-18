using Scaffold.Core.CalcQuantities;
using Scaffold.Core.Exceptions;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcQuantityTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareFoot, "area", "A");

            // Act
            // Assert
            Assert.False(calcArea.TryParse("5.5 cm"));
        }


        [Theory]
        [InlineData(4.3, 4.3, true)]
        [InlineData(4.31, 4.3, false)]
        public void EqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcArea1 = new CalcArea(val1, AreaUnit.SquareMeter, "a1", "A");
            var calcArea2 = new CalcArea(val2 * 10000, AreaUnit.SquareCentimeter, "a2", "A");

            // Act
            bool result = calcArea1 == calcArea2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void NotEqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcArea1 = new CalcArea(val1, AreaUnit.SquareMeter, "a1", "A");
            var calcArea2 = new CalcArea(val2 * 10000, AreaUnit.SquareCentimeter, "a2", "A");

            // Act
            bool result = calcArea1 != calcArea2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.31, 4.3, true)]
        public void GreaterThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcArea1 = new CalcArea(val1, AreaUnit.SquareMeter, "a1", "A");
            var calcArea2 = new CalcArea(val2 * 10000, AreaUnit.SquareCentimeter, "a2", "A");

            // Act
            bool result = calcArea1 > calcArea2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, false)]
        [InlineData(4.3, 4.31, true)]
        public void LessThanOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcArea1 = new CalcArea(val1, AreaUnit.SquareMeter, "a1", "A");
            var calcArea2 = new CalcArea(val2 * 10000, AreaUnit.SquareCentimeter, "a2", "A");

            // Act
            bool result = calcArea1 < calcArea2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, true)]
        [InlineData(4.31, 4.3, true)]
        [InlineData(4.3, 4.31, false)]
        public void GreaterOrEqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcArea1 = new CalcArea(val1, AreaUnit.SquareMeter, "a1", "A");
            var calcArea2 = new CalcArea(val2 * 10000, AreaUnit.SquareCentimeter, "a2", "A");

            // Act
            bool result = calcArea1 >= calcArea2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4.3, 4.3, true)]
        [InlineData(4.3, 4.31, true)]
        [InlineData(4.31, 4.30, false)]
        public void LessOrEqualOperatorTest(double val1, double val2, bool expected)
        {
            // Arrange
            var calcArea1 = new CalcArea(val1, AreaUnit.SquareMeter, "a1", "A");
            var calcArea2 = new CalcArea(val2 * 10000, AreaUnit.SquareCentimeter, "a2", "A");

            // Act
            bool result = calcArea1 <= calcArea2;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EqualsReferenceEqualsObjectTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareFoot, "area", "A");

            // Act
            // Assert
            Assert.True(calcArea.Equals((object)calcArea));
        }

        [Fact]
        public void EqualsNullObjectTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareFoot, "area", "A");

            // Act
            // Assert
            Assert.False(calcArea.Equals((object)null));
        }

        [Fact]
        public void EqualsOtherObjectTest()
        {
            // Arrange
            var calcArea1 = new CalcArea(4.5, AreaUnit.SquareFoot, "area", "A");
            var calcArea2 = new CalcArea(4.5, AreaUnit.SquareFoot, "area", "A");

            // Act
            // Assert
            Assert.True(calcArea1.Equals((object)calcArea2));
        }

        [Fact]
        public void EqualsOtherTypeTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareFoot, "area", "A");
            var notCalcArea = new CalcLength(4.5, LengthUnit.Foot, "length", "l");

            // Act
            // Assert
            Assert.False(calcArea.Equals(notCalcArea));
        }

        [Fact]
        public void EqualsReferenceEqualsTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareFoot, "area", "A");

            // Act
            // Assert
            Assert.True(calcArea.Equals(calcArea));
        }

        [Fact]
        public void EqualsNullTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareFoot, "area", "A");

            // Act
            // Assert
            Assert.False(calcArea.Equals(null));
        }

        [Fact]
        public void EqualsOtherTest()
        {
            // Arrange
            var calcArea1 = new CalcArea(4.5, AreaUnit.SquareFoot, "area", "A");
            var calcArea2 = new CalcArea(4.5, AreaUnit.SquareFoot, "area", "A");

            // Act
            // Assert
            Assert.True(calcArea1.Equals(calcArea2));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            // Arrange
            var calcArea1 = new CalcArea(4.5, AreaUnit.SquareFoot, "area", "A");
            var calcArea2 = new CalcArea(4.5, AreaUnit.SquareFoot, "area", "A");
            var calcArea3 = new CalcArea(4.5, AreaUnit.SquareFoot, "Area", "A");

            // Act
            bool firstEqualsSecond = calcArea1.GetHashCode() == calcArea2.GetHashCode();
            bool firstEqualsThird = calcArea1.GetHashCode() == calcArea3.GetHashCode();

            // Assert
            Assert.True(firstEqualsSecond);
            Assert.False(firstEqualsThird);
        }

        [Fact]
        public void ValueAsStringTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareFoot, "area", "A");

            // Act
            string value = calcArea.ValueAsString();

            // Assert
            Assert.Equal("4.5 ft²", value);
        }

        [Fact]
        public void WrongUnitExceptionTest()
        {
            // Arrange
            var calcArea = new CalcArea(4.5, AreaUnit.SquareFoot, "area", "A");
            Length length = new Length(6, LengthUnit.Centimeter);

            // Act
            // Assert
            Assert.Throws<UnitsNotSameException>(() => calcArea.Quantity = length);
        }
    }
}
