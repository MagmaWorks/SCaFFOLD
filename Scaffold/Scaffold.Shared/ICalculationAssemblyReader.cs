using Scaffold.Core.Images.Interfaces;

namespace Scaffold.Shared;

public interface ICalculationAssemblyReader 
{
    CalculationPackage Get(Stream fileStream);
}