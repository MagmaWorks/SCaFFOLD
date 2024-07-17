using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;

namespace Scaffold.XUnitTests.Core;

public class FluentTitle : ICalculation, ICalculationConfiguration<FluentTitle>
{
    public string Title { get; set; }
    public string Type { get; set; }
    public CalcStatus Status { get; }
    public IEnumerable<Formula> GetFormulae() => [];

    public void Update()
    {
        // not required
    }

    public void Configure(CalculationConfigurationBuilder<FluentTitle> builder)
    {
        builder.SetTitle("Core library tester");
    }
}

public class FluentType : ICalculation, ICalculationConfiguration<FluentType>
{
    public string Title { get; set; }
    public string Type { get; set; }
    public CalcStatus Status { get; }
    public IEnumerable<Formula> GetFormulae() => [];

    public void Update()
    {
        // not required
    }

    public void Configure(CalculationConfigurationBuilder<FluentType> builder)
    {
        builder.SetType("Add values");
    }
}

public class FluentMetadata : ICalculation, ICalculationConfiguration<FluentMetadata>
{
    public string Title { get; set; }
    public string Type { get; set; }
    public CalcStatus Status { get; }
    public IEnumerable<Formula> GetFormulae() => [];

    public void Update()
    {
        // not required
    }

    public void Configure(CalculationConfigurationBuilder<FluentMetadata> builder)
    {
        builder.SetMetadata("Core library tester", "Add values");
    }
}