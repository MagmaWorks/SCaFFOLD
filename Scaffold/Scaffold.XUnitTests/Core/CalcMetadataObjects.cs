using System.Diagnostics.CodeAnalysis;
using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.Models;

namespace Scaffold.XUnitTests.Core;

[ExcludeFromCodeCoverage]
[CalcMetadata(TypeName = "TypeNameSet", Title = "TitleSet")]
public class CalcMetadataEmptyConstructor : CalculationBase
{
    protected override void DefineInputs()
    {
        throw new NotImplementedException();
    }

    protected override void DefineOutputs()
    {
        throw new NotImplementedException();
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }

    protected override IEnumerable<Formula> GenerateFormulae()
    {
        throw new NotImplementedException();
    }
}

[ExcludeFromCodeCoverage]
[CalcMetadata("TypeNameSet")]
public class CalcMetadataTypeNameConstructor : CalculationBase
{
    protected override void DefineInputs()
    {
        throw new NotImplementedException();
    }

    protected override void DefineOutputs()
    {
        throw new NotImplementedException();
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }

    protected override IEnumerable<Formula> GenerateFormulae()
    {
        throw new NotImplementedException();
    }
}

[ExcludeFromCodeCoverage]
[CalcMetadata("TypeNameSet", "TitleSet")]
public class CalcMetadataTypeAndTitleConstructor : CalculationBase
{
    protected override void DefineInputs()
    {
        throw new NotImplementedException();
    }

    protected override void DefineOutputs()
    {
        throw new NotImplementedException();
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }

    protected override IEnumerable<Formula> GenerateFormulae()
    {
        throw new NotImplementedException();
    }
}

[ExcludeFromCodeCoverage]
public class CalcMetadataFallback : CalculationBase
{
    protected override void DefineInputs()
    {
        throw new NotImplementedException();
    }

    protected override void DefineOutputs()
    {
        throw new NotImplementedException();
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }

    protected override IEnumerable<Formula> GenerateFormulae()
    {
        throw new NotImplementedException();
    }
}