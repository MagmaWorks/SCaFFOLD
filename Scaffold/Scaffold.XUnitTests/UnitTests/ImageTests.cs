using Scaffold.Core.Images.Models;
using Scaffold.XUnitTests.Core;

namespace Scaffold.XUnitTests.UnitTests;

public class ImageTests
{
    [Fact]
    public void FromEmbeddedResource_Ok()
    {
        const string embeddedResourceName = "ImageAsEmbeddedResource.png";
        
        var image = new ImageFromEmbeddedResource<AdditionCalculation>(embeddedResourceName);
        var result = image.GetImage();
        
        Assert.NotNull(result);
    }
    
    [Fact]
    public void FromRelativePath_Ok()
    {
        const string relativePathName = "ImageAsRelativePath.png";
        
        var image = new ImageFromRelativePath(relativePathName)
        {
            ImageReader = new AssemblyImageReader(relativePathName, typeof(AdditionCalculation))
        };

        var result = image.GetImage();
        
        Assert.NotNull(result);
    }

    [Fact]
    public void FromSkBitmap_Ok()
    {
        var image = new ImageFromSkBitmap(new SkiaSharp.SKBitmap(1000, 200));
        var result = image.GetImage();
        Assert.NotNull(result);
    }
}