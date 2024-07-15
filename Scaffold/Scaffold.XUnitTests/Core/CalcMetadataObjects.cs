using System.Diagnostics.CodeAnalysis;
using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;

namespace Scaffold.XUnitTests.Core;

[ExcludeFromCodeCoverage]
[CalcMetadata(TypeName = "TypeNameSet", Title = "TitleSet")]
public class CalcMetadataEmptyConstructor : ICalculation
{
    public string Title { get; set; }
    public string Type { get; set; }
    public CalcStatus Status { get; }

    public IReadOnlyList<ICalcValue> GetInputs() => [];
    public IReadOnlyList<ICalcValue> GetOutputs() => [];
    public IEnumerable<Formula> GetFormulae() => [];

    public void Update()
    {
        // not required.
    }
}

[ExcludeFromCodeCoverage]
[CalcMetadata("TypeNameSet")]
public class CalcMetadataTypeNameConstructor : ICalculation
{
    public string Title { get; set; }
    public string Type { get; set; }
    public CalcStatus Status { get; }
    public IReadOnlyList<ICalcValue> GetInputs() => [];
    public IReadOnlyList<ICalcValue> GetOutputs() => [];
    public IEnumerable<Formula> GetFormulae() => [];

    public void Update()
    {
        // not required.
    }
}

[ExcludeFromCodeCoverage]
[CalcMetadata("TypeNameSet", "TitleSet")]
public class CalcMetadataTypeAndTitleConstructor : ICalculation
{
    public string Title { get; set; }
    public string Type { get; set; }
    public CalcStatus Status { get; }
    public IReadOnlyList<ICalcValue> GetInputs() => [];
    public IReadOnlyList<ICalcValue> GetOutputs() => [];
    public IEnumerable<Formula> GetFormulae() => [];

    public void Update()
    {
        // not required.
    }
}

[ExcludeFromCodeCoverage]
public class CalcMetadataFallback : ICalculation
{
    public string Title { get; set; }
    public string Type { get; set; }
    public CalcStatus Status { get; }
    public IReadOnlyList<ICalcValue> GetInputs() => [];
    public IReadOnlyList<ICalcValue> GetOutputs() => [];
    public IEnumerable<Formula> GetFormulae() => [];

    public void Update()
    {
        // not required.
    }
}