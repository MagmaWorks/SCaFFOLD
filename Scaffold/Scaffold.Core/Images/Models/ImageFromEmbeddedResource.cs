using System.Reflection;
using Scaffold.Core.Images.Interfaces;
using SkiaSharp;

namespace Scaffold.Core.Images.Models;

// TODO: Expand to more CalcImage types.
public class ImageFromEmbeddedResource(string embeddedResourceName) : ICalcImage
{
    private string EmbeddedResourceName { get; } = embeddedResourceName;
    public Assembly Assembly { get; set; }
    
    public SKBitmap GetImage()
    {
        var manifestResources = Assembly.GetManifestResourceNames();
        var existing = manifestResources.FirstOrDefault(x => x.EndsWith(EmbeddedResourceName));
        
        if (existing == null)
            throw new Exception($"Embedded resource '{EmbeddedResourceName}' not found.");
        
        using var stream = Assembly.GetManifestResourceStream(existing);
        return SKBitmap.Decode(stream);
    }
}