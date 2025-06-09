namespace Scaffold.Calculations.Eurocode.Steel;

public class DrawSteelSectionTests
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
        Assert.Equal("Draw Steel Section", calc.ReferenceName);
        Assert.Equal("Steel Section Drawing", calc.CalculationName);
        Assert.Equal(1, inputs.Count); // todo - should be 2 whern another calc can act as input
        Assert.Single(outputs);
    }

    [Fact]
    public void DrawSectionTests()
    {
        // Assemble
        var calc = new DrawSteelSection();
        var input = new SteelCatalogueProfile();

        // Act
        input.CatalogueType.Value = "UC";
        input.ProfileType.Value = "UB254x146x37";
        calc.Profile = input;

        // Assert
        Assert.Equal(expectedSvg, calc.Svg.Value);
    }

    private static string expectedSvg = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<svg width=""147px"" height=""256px"" viewBox=""-75 -130 151 260"" xmlns=""http://www.w3.org/2000/svg"" version=""1.1"">
<path d=""M-73.2 128L73.2 128L73.2 117.1L11.15 117.1L9.58928 116.946L8.08853 116.491L6.70544 115.752L5.49315 114.757L4.49824 113.545L3.75896 112.161L3.30372 110.661L3.15 109.1L3.15 -109.1L3.30372 -110.661L3.75896 -112.161L4.49824 -113.545L5.49315 -114.757L6.70544 -115.752L8.08853 -116.491L9.58928 -116.946L11.15 -117.1L73.2 -117.1L73.2 -128L-73.2 -128L-73.2 -117.1L-11.15 -117.1L-9.58928 -116.946L-8.08853 -116.491L-6.70544 -115.752L-5.49315 -114.757L-4.49824 -113.545L-3.75896 -112.161L-3.30372 -110.661L-3.15 -109.1L-3.15 109.1L-3.30372 110.661L-3.75896 112.161L-4.49824 113.545L-5.49315 114.757L-6.70544 115.752L-8.08853 116.491L-9.58928 116.946L-11.15 117.1L-73.2 117.1L-73.2 128Z"" fill=""#d8bebd"" stroke=""#ed3d3b""/>
</svg>
";
}
