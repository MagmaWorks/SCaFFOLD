using Scaffold.Core.Attributes;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;

namespace Scaffold.XUnitTests.Core;

[CalculationMetadata("TypeNameSet", "TitleSet")]
public class FluentDisplayNameSingle : ICalculation, ICalculationConfiguration<FluentDisplayNameSingle>
{
    public string ReferenceName { get; set; }
    public string CalculationName { get; set; }
    public CalcStatus Status { get; }
    public CalcDouble Result { get; set; }

    public FluentDisplayNameSingle()
    {
        Result = new CalcDouble(1);
    }

    public void Calculate()
    {
        // not required
    }

    public IEnumerable<Formula> GetFormulae() => [];

    public void Configure(CalculationConfigurationBuilder<FluentDisplayNameSingle> builder)
    {
        builder.Define(x => x.Result)
            .WithDisplayName("Result")
            .AsOutput();
    }
}

public class FluentDisplayNameMultiple : ICalculation, ICalculationConfiguration<FluentDisplayNameMultiple>
{
    public string ReferenceName { get; set; }
    public string CalculationName { get; set; }
    public CalcStatus Status { get; }

    public CalcDouble Result { get; set; }
    public CalcDouble AnotherResult { get; set; }

    public FluentDisplayNameMultiple()
    {
        Result = new CalcDouble(1);
        AnotherResult = new CalcDouble(2);
    }

    public void Calculate()
    {
        // not required
    }

    public IEnumerable<Formula> GetFormulae() => [];

    public void Configure(CalculationConfigurationBuilder<FluentDisplayNameMultiple> builder)
    {
        builder
            .Define(x => new { x.Result, x.AnotherResult })
            .WithDisplayName("Result")
            .WithSymbol("=")
            .AsOutput();
    }
}
