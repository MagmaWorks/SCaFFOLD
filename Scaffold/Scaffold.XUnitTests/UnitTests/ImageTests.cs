using System.Reflection;

namespace Scaffold.XUnitTests.UnitTests;

public class ImageTests
{
    [Fact]
    public void FromEmbeddedResource_Ok()
    {
        const string embeddedResourceName = "ImageAsEmbeddedResource.png";
        var assembly = Assembly.GetExecutingAssembly();
        
        var image = new Scaffold.Core.Images.Models.ImageFromEmbeddedResource(embeddedResourceName)
        {
            Assembly = assembly
        };

        var result = image.GetImage();
        
        Assert.NotNull(result);
    }
    
    [Fact]
    public void FromRelativePath_Ok()
    {
        const string relativePathName = "ImageAsRelativePath.png";
        var assembly = Assembly.GetExecutingAssembly();
        
        var image = new Scaffold.Core.Images.Models.ImageFromRelativePath(relativePathName)
        {
            Assembly = assembly
        };

        var result = image.GetImage();
        
        Assert.NotNull(result);
    }

    [Fact]
    public void FromSkBitmap_Ok()
    {
        var image = new Scaffold.Core.Images.Models.ImageFromSkBitmap(new SkiaSharp.SKBitmap(1000,200));
        var result = image.GetImage();
        Assert.NotNull(result);
    }
}