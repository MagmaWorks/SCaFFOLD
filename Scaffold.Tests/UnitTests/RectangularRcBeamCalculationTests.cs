using FluentAssertions;
using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core;
using Scaffold.Core.Abstract;
using Scaffold.Core.CalcObjects;

namespace Scaffold.Tests.UnitTests;

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

        inputs[0].DisplayName.Should().Be("500 × 800 mm");

        calc.Calculate();
    }

    [Fact]
    public void Updated_FromUI_Ok()
    {
        var profile = new CalcRectangularProfile();
        profile.TryParse("500mm x 800mm");

        var calc = new RectangularRcBeamCalculation
        {
            Profile = profile
        };

        //calc.bendingMom.Should()
        //    .Be("500 × 800 mm", because: "result has not changed yet through the update method.");

        //Reader.Update(calc);

        //calc.bendingMom.Should()
        //    .Be("800 × 500 mm", because: "result has not changed yet through the update method.");

        //var formulae = Reader.GetFormulae(calc).ToList();
        //formulae.Count.Should().Be(0);
    }
}
