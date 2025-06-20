namespace Scaffold.Calculations.Eurocode.Steel;

public class DrawConcreteSectionTests
{
    private static CalculationReader Reader { get; } = new();

    [Fact]
    public void CalculationBaseSetupTest()
    {
        // Assemble
        var calc = new DrawSteelSection();

        // Act
        CalculationMetadata metadata = Reader.GetMetadata(calc);
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        // Assert
        Assert.Equal("Draw Concrete Section", calc.ReferenceName);
        Assert.Equal("Concrete Section Drawing", calc.CalculationName);
        Assert.Single(inputs); // todo - should be 2 whern another calc can act as input
        Assert.Single(outputs);
    }

    [Fact]
    public void DrawSectionTests()
    {
        // Assemble
        // Act
        var calc = new DrawConcreteSection();

        // Assert
        Assert.NotNull(calc.GetFormulae().FirstOrDefault());
    }
}
