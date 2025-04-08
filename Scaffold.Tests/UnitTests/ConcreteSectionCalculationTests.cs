using FluentAssertions;
using Scaffold.Core;
using Scaffold.Core.Abstract;
using Scaffold.Core.Images.Interfaces;

namespace Scaffold.Tests.UnitTests;

public class ConcreteSectionCalculationTests
{
    private CalculationReader Reader { get; } = new();
    private const string TypeName = "Rectangular Concrete section";
    private const string Title = "Concrete Section Calculation";

    [Fact]
    public void UnreadCalculation_Ok()
    {
        var calc = new ConcreteSectionCalculation();
        calc.CalculationName.Should().BeNull();
        calc.ReferenceName.Should().BeNull();
    }

    [Fact]
    public void Default_Read_Ok()
    {
        var calc = new ConcreteSectionCalculation();

        var metadata = Reader.GetMetadata(calc);
        var inputs = Reader.GetInputs(calc);
        var outputs = Reader.GetOutputs(calc);

        calc.CalculationName.Should().Be(TypeName);
        calc.ReferenceName.Should().Be(Title);

        metadata.Type.Should().Be(TypeName);
        metadata.Title.Should().Be(Title);

        inputs.Count.Should().Be(12);
        outputs.Count.Should().Be(1);

        calc.Calculate();
        //calc.GetFormulae(); skiSVG not working
    }
}
