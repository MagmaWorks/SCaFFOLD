using MagmaWorks.Taxonomy.Standards;
using Scaffold.Core.CalcObjects;
using Scaffold.Core.Enums;

namespace Scaffold.Tests.UnitTests.CalcObjects
{
    public class CalcStandardTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            var standard = new CalcStandard();
            string json =
                "{\r\n  \"$type\": \"Scaffold.Core.CalcObjects.CalcStandard, Scaffold.Sections\",\r\n  \"DisplayName\": \"A\",\r\n  \"Symbol\": \"A\",\r\n  \"Status\": \"None\",\r\n  \"Body\": \"EN\",\r\n  \"Title\": \"My favorite standard\"\r\n}";

            // Act & Assert
            Assert.False(standard.TryParse("invalid"));
            Assert.True(standard.TryParse(json));
            Assert.Equal("A", standard.DisplayName);
            Assert.Equal("A", standard.Symbol);
            Assert.Equal(CalcStatus.None, standard.Status);
            Assert.Equal(StandardBody.EN, standard.Body);
            Assert.Equal("My favorite standard", standard.Title);
        }

        [Fact]
        public void ParseFromMinimalStringTest()
        {
            // Arrange
            var standard = new CalcStandard();
            string json =
                "{\r\n  \"Body\": \"EN\",\r\n  \"Title\": \"My favorite standard\"\r\n}";

            // Act & Assert
            Assert.False(standard.TryParse("invalid"));
            Assert.True(standard.TryParse(json));
            Assert.Equal(StandardBody.EN, standard.Body);
            Assert.Equal("My favorite standard", standard.Title);
        }
    }
}
