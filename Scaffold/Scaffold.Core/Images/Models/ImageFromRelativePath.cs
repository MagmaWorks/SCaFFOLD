using System.Reflection;
using Scaffold.Core.Images.Interfaces;
using SkiaSharp;

namespace Scaffold.Core.Images.Models;

// TODO: Expand to more CalcImage types.
public class ImageFromRelativePath(string relativePathName) : ICalcImage
{
    private string RelativePathName { get; } = relativePathName;
    public Assembly Assembly { get; set; }
    
    public SKBitmap GetImage()
    {
        var fullFilePath = Assembly.Location.Replace(Assembly.ManifestModule.Name, RelativePathName);
        
        if (File.Exists(fullFilePath) == false)
            throw new Exception($"File from relative path '{RelativePathName}' not found.");

        using var stream = File.OpenRead(fullFilePath);
        return SKBitmap.Decode(stream);
    }
}