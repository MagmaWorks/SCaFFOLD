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
    public IReadOnlyList<ICalcValue> GetInputs()
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<ICalcValue> GetOutputs()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Formula> GetFormulae()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        throw new NotImplementedException();
    }
}

[ExcludeFromCodeCoverage]
[CalcMetadata("TypeNameSet")]
public class CalcMetadataTypeNameConstructor : ICalculation
{
    public string Title { get; set; }
    public string Type { get; set; }
    public CalcStatus Status { get; }
    public IReadOnlyList<ICalcValue> GetInputs()
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<ICalcValue> GetOutputs()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Formula> GetFormulae()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        throw new NotImplementedException();
    }
}

[ExcludeFromCodeCoverage]
[CalcMetadata("TypeNameSet", "TitleSet")]
public class CalcMetadataTypeAndTitleConstructor : ICalculation
{
    public string Title { get; set; }
    public string Type { get; set; }
    public CalcStatus Status { get; }
    public IReadOnlyList<ICalcValue> GetInputs()
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<ICalcValue> GetOutputs()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Formula> GetFormulae()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        throw new NotImplementedException();
    }
}

[ExcludeFromCodeCoverage]
public class CalcMetadataFallback : ICalculation
{
    public string Title { get; set; }
    public string Type { get; set; }
    public CalcStatus Status { get; }
    public IReadOnlyList<ICalcValue> GetInputs()
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<ICalcValue> GetOutputs()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Formula> GetFormulae()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        throw new NotImplementedException();
    }
}