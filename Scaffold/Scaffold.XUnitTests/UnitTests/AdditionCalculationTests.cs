using FluentAssertions;
using Scaffold.Core.Abstract;
using Scaffold.XUnitTests.Core;

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
        
        var metadata = Reader.GetMetadata(calc);
        var inputs = Reader.GetInputs(calc);
        var outputs = Reader.GetOutputs(calc);
        
        calc.Type.Should().Be(TypeName);
        calc.Title.Should().Be(Title);

        metadata.Type.Should().Be(TypeName);
        metadata.Title.Should().Be(Title);
        
        inputs.Count.Should().Be(2);
        outputs.Count.Should().Be(1);
        
        inputs[0].DisplayName.Should().Be("Left assignment");
        inputs[1].DisplayName.Should().Be("Right Assignment", because: "Fallback to property name, class did not set DisplayName");
        outputs[0].DisplayName.Should().Be("Result");
    }
    
    [Fact]
    public void Fluent_Read_Ok()
    {
        var calc = new AdditionCalculationFluent();

        var metadata = Reader.GetMetadata(calc);
        var inputs = Reader.GetInputs(calc);
        var outputs = Reader.GetOutputs(calc);
        
        metadata.Type.Should().Be(TypeName);
        metadata.Title.Should().Be(Title);
        
        inputs.Count.Should().Be(2);
        outputs.Count.Should().Be(1);
        
        inputs[0].DisplayName.Should().Be("Left assignment");
        inputs[1].DisplayName.Should().Be("Right Assignment", because: "Fallback to property name, class did not set DisplayName");
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

    [Fact]
    public void FluentPrimitives_Ok()
    {
        var calc = new AdditionCalculationFluentPrimitives();
        
        //
        // Initial values
        //
        var inputs = Reader.GetInputs(calc);
        var outputs = Reader.GetOutputs(calc);
        
        inputs.Count.Should().Be(2);
        outputs.Count.Should().Be(1);
        
        inputs[0].DisplayName.Should().Be("Left assignment");
        inputs[1].DisplayName.Should().Be("Right assignment");
        
        outputs[0].DisplayName.Should().Be("Result");
        
        inputs[0].GetValue().Should().Be("2");
        inputs[1].GetValue().Should().Be("3");
        
        outputs[0].GetValue().Should().Be("5");
        calc.Result.Should().Be(5);
        
        //
        // Updated values (checking that InternalCalcValue works, pairing with true primitive value)
        //
        inputs[0].SetValue("6");
        
        Reader.Update(calc);
        outputs[0].GetValue().Should().Be("9");
        calc.Result.Should().Be(9);
    }
    
    [Fact]
    public void FluentPrimitivesJoined_Ok()
    {
        var calc = new AdditionCalculationFluentPrimitivesJoined();
        
        //
        // Initial values
        //
        var inputs = Reader.GetInputs(calc);
        var outputs = Reader.GetOutputs(calc);
        
        inputs.Count.Should().Be(2);
        outputs.Count.Should().Be(1);
        
        inputs[0].DisplayName.Should().Be("Value (1)");
        inputs[1].DisplayName.Should().Be("Value (2)");
        
        outputs[0].DisplayName.Should().Be("Result");
        
        inputs[0].GetValue().Should().Be("2");
        inputs[1].GetValue().Should().Be("3");
        
        outputs[0].GetValue().Should().Be("5");
        calc.Result.Should().Be(5);
        
        //
        // Updated values (checking that InternalCalcValue works, pairing with true primitive value)
        //
        inputs[0].SetValue("6");
        
        Reader.Update(calc);
        outputs[0].GetValue().Should().Be("9");
        calc.Result.Should().Be(9);
    }
    
    [Fact]
    public void AttributePrimitives_Ok()
    {
        var calc = new AdditionCalculationAttributePrimitives();
        
        //
        // Initial values
        //
        var inputs = Reader.GetInputs(calc);
        var outputs = Reader.GetOutputs(calc);
        
        inputs.Count.Should().Be(2);
        outputs.Count.Should().Be(1);
        
        inputs[0].DisplayName.Should().Be("Left Assignment");
        inputs[0].Symbol.Should().Be("L");
        
        inputs[1].DisplayName.Should().Be("Right Assignment");
        inputs[1].Symbol.Should().Be("R");
        
        outputs[0].DisplayName.Should().Be("Result");
        outputs[0].Symbol.Should().Be("=");
        
        inputs[0].GetValue().Should().Be("2");
        inputs[1].GetValue().Should().Be("3");
        
        outputs[0].GetValue().Should().Be("5");
        calc.Result.Should().Be(5);
        
        //
        // Updated values (checking that InternalCalcValue works, pairing with true primitive value)
        //
        inputs[0].SetValue("6");
        
        Reader.Update(calc);
        outputs[0].GetValue().Should().Be("9");
        calc.Result.Should().Be(9);
    }
   
    [Fact]
    public void Updated_FromUI_Ok()
    {
        var calc = new AdditionCalculation
        {
            LeftAssignment = { Value = 5 },
            RightAssignment = { Value = 4 }
        };

        calc.Result.Value.Should().Be(5, because: "result has not changed yet through the update method.");
    
        Reader.Update(calc);
        
        calc.Result.Value.Should().Be(9);
    
        var formulae = Reader.GetFormulae(calc).ToList();
        formulae.Count.Should().Be(3);
        formulae[0].Expression.Count.Should().Be(1);
        formulae[0].Expression[0].Should().Be("x &=& a + b");
    }
}