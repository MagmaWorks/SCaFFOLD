using Scaffold.Calculations.Sections.Concrete;

namespace Scaffold.Calculations.Eurocode.Steel;

public class DrawConcreteSectionTests
{
    private static CalculationReader Reader { get; } = new();

    [Fact]
    public void CalculationBaseSetupTest()
    {
        // Assemble
        var calc = new ConcreteSection();

        // Act
        CalculationMetadata metadata = Reader.GetMetadata(calc);
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        // Assert
        Assert.Equal("Draw Concrete Section", calc.ReferenceName);
        Assert.Equal("Concrete Section Drawing", calc.CalculationName);
        Assert.Equal(2, inputs.Count); // todo - should be 2 whern another calc can act as input
        Assert.Empty(outputs);
    }

    [Fact]
    public void DrawSectionTests()
    {
        // Assemble
        // Act
        var calc = new ConcreteSection();

        // Assert
        Assert.NotNull(calc.GetFormulae().FirstOrDefault());
    }
}
