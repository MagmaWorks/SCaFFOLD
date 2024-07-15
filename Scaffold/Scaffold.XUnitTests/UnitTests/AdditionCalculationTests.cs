using FluentAssertions;
using Scaffold.Core.Abstract;
using Scaffold.XUnitTests.Core;
// Special TODO: base types to a calc value type (e.g. double > CalcDouble under the hood).
namespace Scaffold.XUnitTests.UnitTests;

public class AdditionCalculationTests
{
    private CalculationReader Reader { get; } = new();
    private const string TypeName = "Add values";
    private const string Title = "Core library tester";
    
    [Fact]
    public void UnreadCalculation_Ok()
    {
        var calc = new AdditionCalculation();
        calc.Type.Should().BeNull();
        calc.Title.Should().BeNull();
    }
    
    [Fact]
    public void Default_Read_Ok()
    {
        var calc = new AdditionCalculation();
        
        var inputs = Reader.GetInputs(calc);
        var outputs = Reader.GetOutputs(calc);
        
        calc.Type.Should().Be(TypeName);
        calc.Title.Should().Be(Title);
        
        inputs.Count.Should().Be(2);
        outputs.Count.Should().Be(1);
        
        inputs[0].DisplayName.Should().Be("Left assignment");
        inputs[1].DisplayName.Should().Be("RightAssignment", because: "Fallback to property name, class did not set DisplayName");
        outputs[0].DisplayName.Should().Be("Result");
    }
    
    [Fact]
    public void Fluent_Read_Ok()
    {
        var calc = new AdditionCalculationFluent();
        
        var inputs = Reader.GetInputs(calc);
        var outputs = Reader.GetOutputs(calc);
        
        inputs.Count.Should().Be(2);
        outputs.Count.Should().Be(1);
        
        inputs[0].DisplayName.Should().Be("Left assignment");
        inputs[1].DisplayName.Should().Be("RightAssignment", because: "Fallback to property name, class did not set DisplayName");
        outputs[0].DisplayName.Should().Be("Result");
    }
    
    [Fact]
    public void Fluent_DisplayNameSingle_Ok()
    {
        var calc = new FluentDisplayNameSingle();
        var outputs = Reader.GetOutputs(calc);

        outputs[0].DisplayName.Should().Be("Result");
    }
    
    [Fact]
    public void Fluent_DisplayNameMultiple_Ok()
    {
        var calc = new FluentDisplayNameMultiple();
        var outputs = Reader.GetOutputs(calc);

        outputs[0].DisplayName.Should().Be("Result (1)");
        outputs[1].DisplayName.Should().Be("Result (2)");
        
        outputs[0].Symbol.Should().Be("=");
        outputs[1].Symbol.Should().Be("=");
    }
    
    // [Fact]
    // public void DefaultSetup_Ok()
    // {
    //     var calc = new AdditionCalculation();
    //     calc.Type.Should().Be(typeName);
    //     calc.Title.Should().Be(title);
    //     
    //     calc.LoadIoCollections();
    //     calc.GetInputs().ToList().Count.Should().Be(2);
    //     calc.GetOutputs().ToList().Count.Should().Be(1);
    //     
    //     calc.LeftAssignment.Value.Should().Be(2);
    //     calc.RightAssignment.Value.Should().Be(3);
    //     calc.Result.Value.Should().Be(5);
    //     calc.Status.Should().Be(CalcStatus.None);
    //     
    //     // Hits return statement in LoadIoCollections
    //     calc.LoadIoCollections();
    // }
    //
    // [Fact]
    // public void Updated_FromUI_Ok()
    // {
    //     var calc = new AdditionCalculation();
    //     calc.Type.Should().Be(typeName);
    //     calc.Title.Should().Be(title);
    //     
    //     calc.LoadIoCollections();
    //
    //     calc.LeftAssignment.Value = 5;
    //     calc.RightAssignment.Value = 4;
    //     calc.Result.Value.Should().Be(5, because: "result has not changed yet through the update method.");
    //
    //     calc.Update();
    //     calc.Result.Value.Should().Be(9);
    //
    //     var formulae = calc.GetFormulae().ToList();
    //     formulae.Count.Should().Be(3);
    //     formulae[0].Expression.Count.Should().Be(1);
    //     formulae[0].Expression[0].Should().Be("x &=& a + b");
    // }
}