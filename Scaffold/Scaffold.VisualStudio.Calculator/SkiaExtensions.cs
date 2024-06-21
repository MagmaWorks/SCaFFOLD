using System.Drawing;
using CSharpMath.Rendering.FrontEnd;
using CSharpMath.SkiaSharp;
using SkiaSharp;

namespace Scaffold.VisualStudio.Calculator;

public static class SkiaExtensions 
{
    private static SizeF AddPaddingToSize(SizeF size)
    {
        // SKSurface does not support zero width/height. Null will be returned from SKSurface.Create.
        if (size.Width is 0) size.Width = 1;
        if (size.Height is 0) size.Height = 1;

        var widthPercentBuffer = size.Width * 10 / 100;
        var heightPercentBuffer = size.Height * 10 / 100;
        
        size.Width += widthPercentBuffer;
        size.Height += heightPercentBuffer;

        return size;
    }
    
    /// <summary>
    /// Taken from https://github.com/verybadcat/CSharpMath/blob/master/CSharpMath.SkiaSharp/Extensions.cs
    /// to resolve the DrawAsStream issues which cuts off some of the content. This adds padding.
    /// </summary>
    /// <returns>The path of the file</returns>
    public static string DrawToFile<TContent>(
        this Painter<SKCanvas, TContent, SKColor> painter,
        string directory, string fileName,
        float textPainterCanvasWidth = TextPainter.DefaultCanvasWidth,
        TextAlignment alignment = TextAlignment.TopLeft,
        SKEncodedImageFormat format = SKEncodedImageFormat.Png,
        int quality = 100) where TContent : class 
    {
        var size = painter.Measure(textPainterCanvasWidth).Size;
        size = AddPaddingToSize(size);
        
        using var surface = SKSurface.Create(new SKImageInfo((int)size.Width, (int)size.Height));
        painter.Draw(surface.Canvas, alignment);
        
        using var snapshot = surface.Snapshot();
        
        var encoded = snapshot.Encode(format, quality).AsStream();

        return WriteSkDataToFile(encoded, directory, fileName);
    }

    public static string WriteSkDataToFile(this Stream skDataStream, string directory, string fileName)
    {
        if (directory != null && Directory.Exists(directory) == false)
            Directory.CreateDirectory(directory);
        
        using var memoryStream = new MemoryStream();
        skDataStream.CopyTo(memoryStream);

        if (memoryStream.Length == 0)
            return null;

        var fullFilePath = $@"{directory}\{fileName}";
        
        File.WriteAllBytes(fullFilePath, memoryStream.ToArray());
        
        return fullFilePath;
    }
}