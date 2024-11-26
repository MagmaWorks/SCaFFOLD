using FluentAssertions;
using Scaffold.Core.Abstract;
using Scaffold.Core.Interfaces;
using Scaffold.XUnitTests.Core;

namespace Scaffold.XUnitTests.UnitTests;

public class CalcMetadataTests
{
    private CalculationReader Reader { get; } = new();

    [Fact]
    public void CalcMetadataEmptyConstructor_Ok()
    {
        var calc = new CalcMetadataEmptyConstructor();
        Reader.Update(calc);

        calc.Type.Should().Be("TypeNameSet");
        calc.Title.Should().Be("TitleSet");
    }

    [Fact]
    public void CalcMetadataTypeNameConstructor_Ok()
    {
        var calc = new CalcMetadataTypeNameConstructor();
        Reader.Update(calc);

        calc.Type.Should().Be("TypeNameSet");
        calc.Title.Should().Be("Calc Metadata Type Name Constructor");
    }

    [Fact]
    public void CalcMetadataTypeAndTitleConstructor_Ok()
    {
        var calc = new CalcMetadataEmptyConstructor();
        Reader.Update(calc);

        calc.Type.Should().Be("TypeNameSet");
        calc.Title.Should().Be("TitleSet");
    }

    [Fact]
    public void CalculationBase_FallbackValues_Used_Ok()
    {
        const string fallbackValue = "Calc Metadata Fallback";
        var calc = new CalcMetadataFallback();
        Reader.Update(calc);

        calc.Type.Should().Be(fallbackValue);
        calc.Title.Should().Be(fallbackValue);
    }
}
