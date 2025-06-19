using System.IO;
using SkiaSharp;
using Svg.Skia;

namespace Scaffold.Core.Images.Models;

public class ImageFromSkSvg : ICalcImage
{
    public ImageFromSkSvg(SKSvg svg) => Svg = svg;
    public ImageFromSkSvg(string svg) => Svg = SKSvg.CreateFromSvg(svg);
    public SKSvg Svg { get; private set; }
    public SKBitmap Bitmap => GetImage();
    public SKBitmap GetImage()
    {
        FileStream fs = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.svg"), FileMode.OpenOrCreate);
        Svg.Save(fs, SKColor.Empty);

        return Svg.Picture.ToBitmap(
            SKColors.Empty, 100, 100, SKColorType.Unknown, SKAlphaType.Unknown, SKColorSpace.CreateSrgb());
    }
}
