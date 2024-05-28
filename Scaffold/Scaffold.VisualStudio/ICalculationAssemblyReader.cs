using Scaffold.Core.Images.Interfaces;

namespace Scaffold.VisualStudio;

public interface ICalculationAssemblyReader : IAssemblyImageReader
{
    CalculationPackage Get(Stream fileStream);
}