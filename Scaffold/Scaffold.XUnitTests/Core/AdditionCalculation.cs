using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Models;

namespace Scaffold.XUnitTests.Core;

[CalcMetadata("Add values", "Core library tester")]
public class AdditionCalculation : CalculationBase
{
    public CalcDouble LeftAssignment { get; set; }
    public CalcDouble RightAssignment { get; set; }
    public CalcDouble Result { get; set; }

    private double Add()
        => LeftAssignment.Value + RightAssignment.Value;
        
    protected override void DefineInputs()
    {
        LeftAssignment = new CalcDouble("Left assignment", 2);
        RightAssignment = new CalcDouble("Right assignment", 3);
    }

    protected override void DefineOutputs()
    {
        Result = new CalcDouble("Result", Add());
    }

    public override void Update()
    {
        Result.Value = Add();
    }

    protected override IEnumerable<Formula> GenerateFormulae()
    {
        return new List<Formula>
        {
            new() {Expression = ["x &=& a + b"] }
        };
    }
}