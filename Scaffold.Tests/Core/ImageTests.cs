using System.Reflection;
using Scaffold.Core.Images.Models;
using Scaffold.Tests.ExampleCalcsForTests;

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
}
