using FluentAssertions;
using Scaffold.XUnitTests.Core;

namespace Scaffold.XUnitTests.UnitTests;

/// <summary>
/// The host application normally calls LoadIoCollections. When it is not called, the default values are expected.
/// </summary>
public class AdditionCalculationTests
{
    private const string typeName = "Add values";
    private const string title = "Core library tester";
    
    [Fact]
    public void DefaultSetup_Unassigned_Expected()
    {
        var calc = new AdditionCalculation();
        calc.Type.Should().Be(typeName);
        calc.Title.Should().Be(title);
        
        calc.LeftAssignment.Should().BeNull();
        calc.RightAssignment.Should().BeNull();
        calc.Result.Should().BeNull();
    }
    
    [Fact]
    public void DefaultSetup_Ok()
    {
        var calc = new AdditionCalculation();
        calc.Type.Should().Be(typeName);
        calc.Title.Should().Be(title);
        
        calc.LoadIoCollections();
        
        calc.LeftAssignment.Value.Should().Be(2);
        calc.RightAssignment.Value.Should().Be(3);
        calc.Result.Value.Should().Be(5);
    }
    
    [Fact]
    public void Updated_FromUI_Ok()
    {
        var calc = new AdditionCalculation();
        calc.Type.Should().Be(typeName);
        calc.Title.Should().Be(title);
        
        calc.LoadIoCollections();

        calc.LeftAssignment.Value = 5;
        calc.RightAssignment.Value = 4;
        calc.Result.Value.Should().Be(5, because: "result has not changed yet through the update method.");

        calc.Update();
        calc.Result.Value.Should().Be(9);
    }
}