using Scaffold.Core.Images.Interfaces;
using SkiaSharp;

namespace Scaffold.Core.Images.Models;

// TODO: Expand to more CalcImage types.
public class ImageFromRelativePath : ICalcImage
{
    public ImageFromRelativePath(string relativePathName)
    {
        RelativePathName = relativePathName;
    }

    private string RelativePathName { get; }
    public IAssemblyImageReader ImageReader { get; set; }
    
    public SKBitmap GetImage()
    {
        var assemblyImage = ImageReader.Images.FirstOrDefault(x => x.Path.Contains(RelativePathName));
        if (assemblyImage == null)
            throw new Exception($"File from relative path '{RelativePathName}' not found.");

        return SKBitmap.Decode(assemblyImage.Data);
    }
}