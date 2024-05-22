using Scaffold.Core.Images.Interfaces;
using SkiaSharp;

namespace Scaffold.Core.Images.Models;

public class ImageFromSkBitmap : ICalcImage
{
    public ImageFromSkBitmap(SKBitmap bitmap) => Bitmap = bitmap;
    public SKBitmap Bitmap { get; }
    public SKBitmap GetImage() => Bitmap;
}