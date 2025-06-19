using System.Reflection;
using Scaffold.Core.Images.Models;
using Scaffold.Tests.ExampleCalcsForTests;
using SkiaSharp;

namespace Scaffold.XUnitTests.Core;
public class ImageTests
{
    [Fact]
    public void FromEmbeddedResource_Ok()
    {
        const string embeddedResourceName = "ImageAsEmbeddedResource.png";

        var image = new ImageFromEmbeddedResource<AdditionCalculation>(embeddedResourceName);
        SkiaSharp.SKBitmap result = image.GetImage();

        Assert.NotNull(result);
    }

    [Fact]
    public void FromRelativePath_Ok()
    {
        const string relativePathName = "ImageAsRelativePath.png";
        var executingAssembly = Assembly.GetExecutingAssembly();
        string workingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Resources");

        var image = new ImageFromRelativePath(relativePathName)
        {
            ImageReader = new AssemblyImageReader(relativePathName, workingDirectory)
        };

        SkiaSharp.SKBitmap result = image.GetImage();

        Assert.NotNull(result);
    }

    [Fact]
    public void FromSkBitmap_Ok()
    {
        var image = new ImageFromSkBitmap(new SkiaSharp.SKBitmap(1000, 200));
        SkiaSharp.SKBitmap result = image.GetImage();
        Assert.NotNull(result);
    }

    [Fact]
    public void FromSkSvgString_Ok()
    {
        string svgString = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<svg width=""147px"" height=""256px"" viewBox=""-75 -130 151 260"" xmlns=""http://www.w3.org/2000/svg"" version=""1.1"">
<path d=""M-73.2 128L73.2 128L73.2 117.1L11.15 117.1L9.58928 116.946L8.08853 116.491L6.70544 115.752L5.49315 114.757L4.49824 113.545L3.75896 112.161L3.30372 110.661L3.15 109.1L3.15 -109.1L3.30372 -110.661L3.75896 -112.161L4.49824 -113.545L5.49315 -114.757L6.70544 -115.752L8.08853 -116.491L9.58928 -116.946L11.15 -117.1L73.2 -117.1L73.2 -128L-73.2 -128L-73.2 -117.1L-11.15 -117.1L-9.58928 -116.946L-8.08853 -116.491L-6.70544 -115.752L-5.49315 -114.757L-4.49824 -113.545L-3.75896 -112.161L-3.30372 -110.661L-3.15 -109.1L-3.15 109.1L-3.30372 110.661L-3.75896 112.161L-4.49824 113.545L-5.49315 114.757L-6.70544 115.752L-8.08853 116.491L-9.58928 116.946L-11.15 117.1L-73.2 117.1L-73.2 128Z"" fill=""#d8bebd"" stroke=""#ed3d3b""/>
</svg>
";
        var image = new ImageFromSkSvg(svgString);
        SKBitmap result = image.GetImage();
        Assert.NotNull(result);
        SKFileWStream fs = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "quickstart.jpg"));
        result.Encode(fs, SKEncodedImageFormat.Jpeg, quality: 85);
    }
}
