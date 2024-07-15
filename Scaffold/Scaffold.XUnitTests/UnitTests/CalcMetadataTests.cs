using FluentAssertions;
using Scaffold.Core.Abstract;
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
        const string fallbackValue = nameof(CalcMetadataTypeNameConstructor);
        var calc = new CalcMetadataTypeNameConstructor();
        Reader.Update(calc);
        
        calc.Type.Should().Be("TypeNameSet");
        calc.Title.Should().Be(fallbackValue);
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
        const string fallbackValue = nameof(CalcMetadataFallback);
        var calc = new CalcMetadataFallback();
        Reader.Update(calc);
        
        calc.Type.Should().Be(fallbackValue);
        calc.Title.Should().Be(fallbackValue);
    }
    
    [Fact]
    public void FluentTitle_Ok()
    {
        const string fallbackValue = nameof(FluentTitle);
        var calc = new FluentTitle();
        Reader.Update(calc);
        
        calc.Type.Should().Be(fallbackValue);
        calc.Title.Should().Be("Core library tester");
    }
    
    [Fact]
    public void FluentType_Ok()
    {
        const string fallbackValue = nameof(FluentType);
        var calc = new FluentType();
        Reader.Update(calc);
        
        calc.Type.Should().Be("Add values");
        calc.Title.Should().Be(fallbackValue);
    }

    [Fact] 
    public void FluentMetadata_Ok()
    {
        var calc = new FluentMetadata();
        Reader.Update(calc);
        
        calc.Title.Should().Be("Core library tester");
        calc.Type.Should().Be("Add values");
    }
}