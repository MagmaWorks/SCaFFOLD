using System.Reflection;
using Scaffold.Core.Images.Interfaces;

namespace Scaffold.Core.Images.Models;

public class AssemblyImageReader : IAssemblyImageReader
{
    private readonly List<AssemblyImage> _images = new();

    public AssemblyImageReader(string relativePathName, Type typeInAssembly)
    {
        var assembly = Assembly.GetAssembly(typeInAssembly);
        if (assembly == null)
            throw new Exception("Assembly not found.");
        
        var fullFilePath = assembly.Location.Replace(assembly.ManifestModule.Name, relativePathName);
        using var stream = File.OpenRead(fullFilePath);
        
        var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        memoryStream.Position = 0;
        
        _images.Add(new AssemblyImage {Path = relativePathName, Data = memoryStream.ToArray()});
    }
    
    public IReadOnlyList<AssemblyImage> Images => _images;
}