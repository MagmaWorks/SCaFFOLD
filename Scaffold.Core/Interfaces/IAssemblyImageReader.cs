using Scaffold.Core.Images.Models;

namespace Scaffold.Core.Interfaces;

public interface IAssemblyImageReader
{
    IReadOnlyList<AssemblyImage> Images { get; }
}
