using Scaffold.Core.Extensions;
using Scaffold.Core.CalcObjects.Profiles;

namespace Scaffold.Tests.UnitTests;

public class ProfileFromDescriptionTests
{
    [Theory]
    [InlineData("400 500 50 mm")]
    [InlineData("40 cm 500 50 mm")]
    [InlineData("0.4m 0.5m 50 mm")]
    [InlineData("0.4 500 mm 0.05 m")]
    public void Test(string description)
    {
        // Assemble
        CalcRectangularHollow profile = default;

        // Act
        profile = profile.ProfileFromDescription(description); 

        // Assert
        Assert.Equal(400, profile.Width.Millimeters);
    }
}
