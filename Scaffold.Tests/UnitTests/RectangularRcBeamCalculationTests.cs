using FluentAssertions;
using Scaffold.Core.Abstract;
using Scaffold.RcBeam;

namespace Scaffold.XUnitTests.UnitTests;

public class RectangularRcBeamCalculationTests
{
    private CalculationReader Reader { get; } = new();
    private const string TypeName = "Rectangular RC beam calculation";
    private const string Title = "Rectangular Rc Beam Calculation";

    [Fact]
    public void UnreadCalculation_Ok()
    {
        var calc = new RectangularRcBeamCalculation();
        calc.CalculationName.Should().BeNull();
        calc.ReferenceName.Should().BeNull();
    }

    [Fact]
    public void Default_Read_Ok()
    {
        var calc = new RectangularRcBeamCalculation();

        var metadata = Reader.GetMetadata(calc);
        var inputs = Reader.GetInputs(calc);
        var outputs = Reader.GetOutputs(calc);

        calc.CalculationName.Should().Be(TypeName);
        calc.ReferenceName.Should().Be(Title);

        metadata.Type.Should().Be(TypeName);
        metadata.Title.Should().Be(Title);

        inputs.Count.Should().Be(24);
        outputs.Count.Should().Be(18);

        inputs[0].DisplayName.Should().Be("Profile");

        calc.Calculate();
    }
}
