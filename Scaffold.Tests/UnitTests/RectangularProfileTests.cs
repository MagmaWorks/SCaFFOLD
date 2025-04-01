﻿using FluentAssertions;
using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core.Abstract;
using Scaffold.Sections;

namespace Scaffold.XUnitTests.UnitTests;

public class RectangularProfileTests
{
    private CalculationReader Reader { get; } = new();
    private const string TypeName = "Rectangular profile";
    private const string Title = "Rectangular Profile";

    [Fact]
    public void UnreadCalculation_Ok()
    {
        var calc = new RectangularProfile();
        calc.CalculationName.Should().BeNull();
        calc.ReferenceName.Should().BeNull();
    }

    [Fact]
    public void Default_Read_Ok()
    {
        var calc = new RectangularProfile();

        var metadata = Reader.GetMetadata(calc);
        var inputs = Reader.GetInputs(calc);
        var outputs = Reader.GetOutputs(calc);

        calc.CalculationName.Should().Be(TypeName);
        calc.ReferenceName.Should().Be(Title);

        metadata.Type.Should().Be(TypeName);
        metadata.Title.Should().Be(Title);

        inputs.Count.Should().Be(2);
        outputs.Count.Should().Be(1);

        inputs[0].DisplayName.Should().Be("Width");
        inputs[1].DisplayName.Should().Be("Height", because: "Fallback to property name, class did not set DisplayName");
        outputs[0].DisplayName.Should().Be("500 × 800 mm");
    }

    [Fact]
    public void Updated_FromUI_Ok()
    {
        var calc = new RectangularProfile
        {
            Width = { Quantity = new Length(800, LengthUnit.Millimeter) },
            Height = { Quantity = new Length(500, LengthUnit.Millimeter) }
        };

        calc.Profile.Profile.Description.Should().Be("500 × 800 mm", because: "result has not changed yet through the update method.");

        Reader.Update(calc);

        calc.Profile.Profile.Description.Should().Be("800 × 500 mm", because: "result has not changed yet through the update method.");

        var formulae = Reader.GetFormulae(calc).ToList();
        formulae.Count.Should().Be(0);
    }
}
