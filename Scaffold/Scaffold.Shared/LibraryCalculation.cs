using Scaffold.Core.Interfaces;

namespace Scaffold.Shared;

public class LibraryCalculation : ICalculationMetadata
{
    public string AssemblyName { get; set; }
    public string QualifiedTypeName { get; set; }
    public string Title { get; set; }
    public string Type { get; init; }
}