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

        calc.rebarAsReqd.Value.Should().BeApproximately(327.6, 1.0);
    }

    public class Foo
    {
        public decimal Value1 { get; set; }
        public decimal Value2 { get; set; }
    }



    [Fact]
    public void Updated_FromUI_Ok()
    {
        var calc = new RectangularRcBeamCalculation();
        calc.Calculate();

        var profile = new CalcRectangularProfile();
        profile.TryParse("800mm x 500mm");
        calc.Profile = profile;

        calc.rebarAsReqd.Value.Should().BeApproximately(327.6, 1.0, because: "result has not changed yet through the update method.");

        Reader.Update(calc);

        calc.rebarAsReqd.Value.Should().BeApproximately(551.5, 1.0, because: "result has not changed yet through the update method.");

        var formulae = Reader.GetFormulae(calc).ToList();
        formulae.Count.Should().Be(5);
    }
}
