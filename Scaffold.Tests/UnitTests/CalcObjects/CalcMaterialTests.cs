using MagmaWorks.Taxonomy.Materials;
using MagmaWorks.Taxonomy.Standards;
using Scaffold.Core.CalcObjects;
using Scaffold.Core.Enums;

namespace Scaffold.Tests.UnitTests.CalcValues
{
    public class CalcMaterialTests
    {
        [Fact]
        public void ParseFromStringTest()
        {
            // Arrange
            var standard = new CalcStandard()
            {
                Body = StandardBody.EN
            };

            var material = new CalcMaterial()
            {
                DisplayName = "Foo",
                Symbol = "M",
                Status = CalcStatus.Fail,
                Standard = standard,
                Type = MaterialType.Concrete
            };

            string json = "{\r\n  \"$type\": \"Scaffold.Core.CalcObjects.CalcMaterial, Scaffold.Sections\",\r\n  \"DisplayName\": \"Foo\",\r\n  \"Symbol\": \"M\",\r\n  \"Status\": \"Fail\",\r\n  \"Standard\": {\r\n    \"$type\": \"Scaffold.Core.CalcObjects.CalcStandard, Scaffold.Sections\",\r\n    \"DisplayName\": null,\r\n    \"Symbol\": null,\r\n    \"Status\": \"None\",\r\n    \"Body\": \"EN\",\r\n    \"Title\": null\r\n  },\r\n  \"Type\": \"Concrete\"\r\n}";

            // Act & Assert
            Assert.False(material.TryParse("invalid"));
            Assert.True(material.TryParse(json));
            Assert.Equal("Foo", material.DisplayName);
            Assert.Equal("M", material.Symbol);
            Assert.Equal(CalcStatus.Fail, material.Status);
            Assert.Equal(StandardBody.EN, material.Standard.Body);
            Assert.Equal(MaterialType.Concrete, material.Type);
        }
    }
}
