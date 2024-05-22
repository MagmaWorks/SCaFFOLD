using System.Reflection;
using Scaffold.Core.Images.Interfaces;
using Scaffold.Core.Images.Models;

namespace Scaffold.XUnitTests.Core;

/// <summary>
/// On the live system, this is part of our calculation assembly
/// reader which gets the images alongside the assemblies with calculations.
/// </summary>
public class AssemblyImageReader : IAssemblyImageReader
{
    private readonly List<AssemblyImage> _images = new();

    public AssemblyImageReader(string relativePathName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var fullFilePath = assembly.Location.Replace(assembly.ManifestModule.Name, relativePathName);
        using var stream = File.OpenRead(fullFilePath);
        
        var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        memoryStream.Position = 0;
        
        _images.Add(new AssemblyImage {Path = relativePathName, Data = memoryStream.ToArray()});
    }
    
    public IReadOnlyList<AssemblyImage> Images => _images;
}