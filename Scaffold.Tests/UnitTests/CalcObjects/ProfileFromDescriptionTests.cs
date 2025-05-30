
namespace Scaffold.Tests.UnitTests;

public class ProfileFromDescriptionTests
{
    [Theory]
    [InlineData("400 500 60 mm")]
    [InlineData("40 cm 500 60 mm")]
    [InlineData("0.4m 0.5m 60 mm")]
    [InlineData("0.4 500 mm 0.06 m")]
    [InlineData("40 x 50 x 6cm")]
    [InlineData("40x50x6.0 cm")]
    public void Test(string description)
    {
        // Assemble
        // Act
        CalcRectangularHollow profile = CalcRectangularHollow.CreateFromDescription(description);

        // Assert
        Assert.Equal(400, profile.Width.Millimeters);
        Assert.Equal(500, profile.Height.Millimeters);
        Assert.Equal(60, profile.Thickness.Millimeters);
    }
}
