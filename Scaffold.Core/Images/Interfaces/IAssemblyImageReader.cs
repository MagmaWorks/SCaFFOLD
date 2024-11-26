using Scaffold.Core.Images.Models;

namespace Scaffold.Core.Images.Interfaces;

public interface IAssemblyImageReader
{
    IReadOnlyList<AssemblyImage> Images { get; }
}