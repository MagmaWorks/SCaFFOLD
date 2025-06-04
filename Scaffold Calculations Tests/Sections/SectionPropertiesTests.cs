using MagmaWorks.Taxonomy.Profiles;
using Scaffold.Core.CalcObjects.Profiles;
using Scaffold.Core.CalcQuantities;

namespace Scaffold.Calculations.Sections;

public class SectionPropertiesTests
{
    private static CalculationReader Reader { get; } = new();

    [Fact]
    public void CalculationBaseSetupTest()
    {
        // Assemble
        var calc = new SectionPropertiesCalculation();

        // Act
        CalculationMetadata metadata = Reader.GetMetadata(calc);
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        // Assert
        Assert.Equal("Section Properties Calculation", calc.ReferenceName);
        Assert.Equal("Section Properties", calc.CalculationName);
        Assert.Equal(1, inputs.Count);
        Assert.Equal(11, outputs.Count);
    }

    [Theory]
    [InlineData("500 x 800 cm", typeof(CalcRectangularProfile), "40m²")]
    public void AreaTests(string description, Type profileType, string expectedArea)
    {
        // Assemble
        object[] parameters = new object[] { description };
        IProfile profile = (IProfile)profileType.GetMethod("CreateFromDescription").Invoke(null, parameters);
        var calc = new SectionPropertiesCalculation();

        // Act
        calc.Profile = profile;
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        // Assert
        Assert.Equal(expectedArea, outputs[4].ValueAsString());
    }
}
