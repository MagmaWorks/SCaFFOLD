using Scaffold.Core.CalcValues;

namespace Scaffold.Tests.UnitTests.CalcValues
{
    public class CalcStringTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Assemble
            var calcString = new CalcString("hello");

            // Act
            // Assert
            Assert.True(calcString.TryParse("olleh"));
            Assert.Equal("olleh", calcString.Value);
        }

        [Fact]
        public void ImplicitOperatorTest()
        {
            // Assemble
            CalcString calcString1 = new CalcString("hello");

            // Act
            string result = calcString1;

            // Assert
            Assert.Equal("hello", result);
        }

        [Fact]
        public void AdditionOperatorTest()
        {
            // Assemble
            var calcString1 = new CalcString("hello");
            var calcString2 = new CalcString("friend");

            // Act
            CalcString result = calcString1 + calcString2;

            // Assert
            Assert.Equal("hellofriend", result.Value);
        }

        [Fact]
        public void AdditionSameUnitOperatorTest()
        {
            // Assemble
            var calcString1 = new CalcString("hello", "l1", "L", "m");
            var calcString2 = new CalcString("friend", "l2", "L", "m");

            // Act
            CalcString result = calcString1 + calcString2;

            // Assert
            Assert.Equal("hellofriend", result.Value);
            Assert.Equal("L", result.Symbol);
            Assert.Equal("m", result.Unit);
            Assert.Equal("l1 + l2", result.DisplayName); // note: using Thin Space \u2009
        }
    }
}
