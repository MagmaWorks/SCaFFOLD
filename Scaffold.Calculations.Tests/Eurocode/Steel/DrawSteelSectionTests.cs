namespace Scaffold.Calculations.Eurocode.Steel;

public class DrawSteelSectionTests
{
    private static CalculationReader Reader { get; } = new();

    [Fact]
    public void CalculationBaseSetupTest()
    {
        // Assemble
        var calc = new SteelCatalogueSection();

        // Act
        CalculationMetadata metadata = Reader.GetMetadata(calc);
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        // Assert
        Assert.Equal("Draw Steel Section", calc.ReferenceName);
        Assert.Equal("Steel Section Drawing", calc.CalculationName);
        Assert.Single(inputs); // todo - should be 2 whern another calc can act as input
        Assert.Empty(outputs);
    }

    [Fact]
    public void DrawSectionTests()
    {
        // Assemble
        var calc = new SteelCatalogueSection();
        var input = new SteelCatalogueProfile();

        // Act
        input.CatalogueType.Value = "UB";
        input.ProfileType.Value = "UB254x146x37";
        calc.Profile = input;

        // Assert
        Assert.NotNull(calc.GetFormulae().FirstOrDefault());
    }
}
