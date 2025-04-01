using OasysUnits;
using OasysUnits.Units;
using MagmaWorks.Taxonomy.Profiles;
using Scaffold.Core.Utility;

namespace Scaffold.Tests.UnitTests.CalcQuantities
{
    public class SectionDrawingTests
    {
        [Fact]
        public void Draw500x800Rectangle()
        {
            // Arrange
            var rectangle = new Rectangle(
                new Length(500, LengthUnit.Millimeter),
                new Length(800, LengthUnit.Millimeter));

            // Act
            string path = SectionDrawing.DrawProfileSvg(rectangle);

            // Assert
            Assert.NotNull(path);
        }
    }
}
