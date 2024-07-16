using System.Reflection;
using Scaffold.Core.Images.Interfaces;

namespace Scaffold.Core.Images.Models;

public class AssemblyImageReader : IAssemblyImageReader
{
    private readonly List<AssemblyImage> _images = new();

    public AssemblyImageReader(string relativePathName, string folderPath)
    {
        var fullFilePath = $@"{folderPath}\{relativePathName}";
        using var stream = File.OpenRead(fullFilePath);
        
        var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        memoryStream.Position = 0;
        
        _images.Add(new AssemblyImage {Path = relativePathName, Data = memoryStream.ToArray()});
    }
    
    public IReadOnlyList<AssemblyImage> Images => _images;
}