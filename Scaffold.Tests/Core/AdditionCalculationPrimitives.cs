using Scaffold.Core.Attributes;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;

namespace Scaffold.XUnitTests.Core;

public class AdditionCalculationFluentPrimitives : ICalculation, ICalculationConfiguration<AdditionCalculationFluentPrimitives>
{

    public string ReferenceName { get; set; }
    public string CalculationName { get; set; }
    public CalcStatus Status { get; }

    public double LeftAssignment { get; set; }
    public double RightAssignment { get; set; }
    public double Result { get; set; }

    public AdditionCalculationFluentPrimitives()
    {
        LeftAssignment = 2;
        RightAssignment = 3;
        Result = Add();
    }

    public void Configure(CalculationConfigurationBuilder<AdditionCalculationFluentPrimitives> builder)
    {
        builder
            .Define(x => x.LeftAssignment)
            .WithDisplayName("Left assignment")
            .WithSymbol("L")
            .AsInput();

        builder
            .Define(x => x.RightAssignment)
            .WithDisplayName("Right assignment")
            .AsInput();

        builder.Define(x => x.Result)
            .WithDisplayName("Result")
            .AsOutput();
    }

    public IEnumerable<Formula> GetFormulae() => [];
    public void Update() => Result = Add();
    private double Add() => LeftAssignment + RightAssignment;
}

/// <summary>
/// An unlikely configuration, since you're going to want to add names to primitives individually. Still should be supported behaviour though.
/// </summary>
public class AdditionCalculationFluentPrimitivesJoined : ICalculation, ICalculationConfiguration<AdditionCalculationFluentPrimitivesJoined>
{
    public string ReferenceName { get; set; }
    public string CalculationName { get; set; }
    public CalcStatus Status { get; }
    public double LeftAssignment { get; set; }
    public double RightAssignment { get; set; }
    public double Result { get; set; }

    public AdditionCalculationFluentPrimitivesJoined()
    {
        LeftAssignment = 2;
        RightAssignment = 3;
        Result = Add();
    }

    public void Configure(CalculationConfigurationBuilder<AdditionCalculationFluentPrimitivesJoined> builder)
    {
        builder
            .Define(x => new { x.LeftAssignment, x.RightAssignment })
            .WithDisplayName("Value")
            .AsInput();

        builder.Define(x => x.Result)
            .WithDisplayName("Result")
            .AsOutput();
    }

    public IEnumerable<Formula> GetFormulae() => [];
    public void Update() => Result = Add();
    private double Add() => LeftAssignment + RightAssignment;
}

public class AdditionCalculationAttributePrimitives : ICalculation
{
    public string ReferenceName { get; set; }
    public string CalculationName { get; set; }
    public CalcStatus Status { get; }

    [InputCalcValue("L")]
    public double LeftAssignment { get; set; }

    [InputCalcValue("R")]
    public double RightAssignment { get; set; }

    [OutputCalcValue("=")]
    public double Result { get; set; }

    public AdditionCalculationAttributePrimitives()
    {
        LeftAssignment = 2;
        RightAssignment = 3;
        Result = Add();
    }

    public IEnumerable<Formula> GetFormulae() => [];

    public void Update() => Result = Add();
    private double Add() => LeftAssignment + RightAssignment;
}
