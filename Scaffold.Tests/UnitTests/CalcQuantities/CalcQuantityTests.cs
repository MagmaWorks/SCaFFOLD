using Scaffold.Core.CalcQuantities;
using Scaffold.Core.Exceptions;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class CalcQuantityTests
    {
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
