using FluentAssertions;
using Scaffold.XUnitTests.Core;

namespace Scaffold.XUnitTests.UnitTests;

public class CalcMetadataTests
{
    [Fact]
    public void CalcMetadataEmptyConstructor_Ok()
    {
        var calc = new CalcMetadataEmptyConstructor();
        calc.Type.Should().Be("TypeNameSet");
        calc.Title.Should().Be("TitleSet");
    }
    
    [Fact]
    public void CalcMetadataTypeNameConstructor_Ok()
    {
        const string fallbackValue = nameof(CalcMetadataTypeNameConstructor);
        var calc = new CalcMetadataTypeNameConstructor();
        
        calc.Type.Should().Be("TypeNameSet");
        calc.Title.Should().Be(fallbackValue);
    }
    
    [Fact]
    public void CalcMetadataTypeAndTitleConstructor_Ok()
    {
        var calc = new CalcMetadataTypeAndTitleConstructor();
        calc.Type.Should().Be("TypeNameSet");
        calc.Title.Should().Be("TitleSet");
    }
    
    [Fact]
    public void CalculationBase_FallbackValues_Used_Ok()
    {
        const string fallbackValue = nameof(CalcMetadataFallback);
        var calc = new CalcMetadataFallback();
        
        calc.Type.Should().Be(fallbackValue);
        calc.Title.Should().Be(fallbackValue);
    }
}