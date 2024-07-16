using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;

namespace Scaffold.XUnitTests.Core;

public class FluentDisplayNameSingle : ICalculation, ICalculationConfiguration<FluentDisplayNameSingle>
{
    public FluentDisplayNameSingle()
    {
        Result = new CalcDouble(1);
    }
    
    public CalcDouble Result { get; set; }
    
    public void Update()
    {
        // not required
    }

    public IEnumerable<Formula> GetFormulae() => [];

    public string Title { get; set; }
    public string Type { get; set; }
    public CalcStatus Status { get; }
    
    public void Configure(CalculationConfigurationBuilder<FluentDisplayNameSingle> configurationBuilder)
    {
        configurationBuilder.Define(x => x.Result)
            .WithDisplayName("Result")
            .AsOutput();
    }
}

public class FluentDisplayNameMultiple : ICalculation, ICalculationConfiguration<FluentDisplayNameMultiple>
{
    public FluentDisplayNameMultiple()
    {
        Result = new CalcDouble(1);
        AnotherResult = new CalcDouble(2);
    }
    
    public CalcDouble Result { get; set; }
    public CalcDouble AnotherResult { get; set; }
    
    public void Update()
    {
        // not required
    }

    public IEnumerable<Formula> GetFormulae() => [];

    public string Title { get; set; }
    public string Type { get; set; }
    public CalcStatus Status { get; }
    
    public void Configure(CalculationConfigurationBuilder<FluentDisplayNameMultiple> configurationBuilder)
    {
        configurationBuilder
            .Define(x => new { x.Result, x.AnotherResult })
            .WithDisplayName("Result")
            .WithSymbol("=")
            .AsOutput();
    }
}