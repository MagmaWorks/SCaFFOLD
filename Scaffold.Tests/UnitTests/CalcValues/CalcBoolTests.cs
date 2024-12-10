using Scaffold.Core.CalcValues;

namespace Scaffold.Tests.UnitTests.CalcValues
{
    public class CalcBoolTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Assemble
            var calcBool = new CalcBool(false);

            // Act
            // Assert
            Assert.True(calcBool.TryParse("true"));
            Assert.True(calcBool.Value);
        }

        [Fact]
        public void ImplicitOperatorTest()
        {
            // Assemble
            var calcBool1 = new CalcBool();

            // Act
            bool result = calcBool1;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AndOperatorTest()
        {
            // Assemble
            var calcBooltrue = new CalcBool(true, "truth");
            var calcBoolfalse = new CalcBool(false, "fake");

            // Act
            CalcBool expectedFalse = calcBooltrue & calcBoolfalse;
            CalcBool expectedTrue = calcBooltrue & calcBooltrue;
            CalcBool expectedFalse2 = calcBoolfalse & calcBoolfalse;

            // Assert
            Assert.False(expectedFalse);
            Assert.Equal("truth&fake", expectedFalse.DisplayName);
            Assert.True(expectedTrue);
            Assert.Equal("truth&truth", expectedTrue.DisplayName);
            Assert.False(expectedFalse2);
            Assert.Equal("fake&fake", expectedFalse2.DisplayName);
        }

        [Fact]
        public void OrOperatorTest()
        {
            // Assemble
            var calcBooltrue = new CalcBool(true, "truth");
            var calcBoolfalse = new CalcBool(false, "fake");

            // Act
            CalcBool expectedTrue = calcBooltrue | calcBoolfalse;
            CalcBool expectedTrue2 = calcBooltrue | calcBooltrue;
            CalcBool expectedFalse = calcBoolfalse | calcBoolfalse;

            // Assert
            Assert.True(expectedTrue);
            Assert.Equal("truth∨fake", expectedTrue.DisplayName);
            Assert.True(expectedTrue2);
            Assert.Equal("truth∨truth", expectedTrue2.DisplayName);
            Assert.False(expectedFalse);
            Assert.Equal("fake∨fake", expectedFalse.DisplayName);
        }

        [Fact]
        public void EqualsOperatorTest()
        {
            // Assemble
            var calcBooltrue1 = new CalcBool(true, "truth1");
            var calcBooltrue2 = new CalcBool(true, "truth2");
            var calcBoolfalse1 = new CalcBool(false, "fake1");
            var calcBoolfalse2 = new CalcBool(false, "fake2");

            // Act
            CalcBool expectedFalse1 = calcBooltrue1 == calcBoolfalse1;
            CalcBool expectedTrue = calcBooltrue1 == calcBooltrue2;
            CalcBool expectedTrue2 = calcBoolfalse1 == calcBoolfalse2;

            // Assert
            Assert.False(expectedFalse1);
            Assert.Equal("truth1=fake1", expectedFalse1.DisplayName);
            Assert.True(expectedTrue);
            Assert.Equal("truth1=truth2", expectedTrue.DisplayName);
            Assert.True(expectedTrue2);
            Assert.Equal("fake1=fake2", expectedTrue2.DisplayName);
        }

        [Fact]
        public void NotEqualsOperatorTest()
        {
            // Assemble
            var calcBooltrue1 = new CalcBool(true, "truth1");
            var calcBooltrue2 = new CalcBool(true, "truth2");
            var calcBoolfalse1 = new CalcBool(false, "fake1");
            var calcBoolfalse2 = new CalcBool(false, "fake2");

            // Act
            CalcBool expectedTrue1 = calcBooltrue1 != calcBoolfalse1;
            CalcBool expectedFalse1 = calcBooltrue1 != calcBooltrue2;
            CalcBool expectedFalse2 = calcBoolfalse1 != calcBoolfalse2;

            // Assert
            Assert.True(expectedTrue1);
            Assert.Equal("truth1≠fake1", expectedTrue1.DisplayName);
            Assert.False(expectedFalse1);
            Assert.Equal("truth1≠truth2", expectedFalse1.DisplayName);
            Assert.False(expectedFalse2);
            Assert.Equal("fake1≠fake2", expectedFalse2.DisplayName);
        }

        [Fact]
        public void EqualsTest()
        {
            // Assemble
            var calcBooltrue = new CalcBool(true, "truth");
            var calcBoolfalse = new CalcBool(false, "fake");

            // Act
            // Assert
            Assert.False(calcBooltrue.Equals(calcBoolfalse));
            Assert.True(calcBooltrue.Equals(calcBooltrue));
            Assert.True(calcBoolfalse.Equals(calcBoolfalse));
        }
    }
}
