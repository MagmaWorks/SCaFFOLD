using Scaffold.Core.Images.Interfaces;
using SkiaSharp;

namespace Scaffold.Core.Images.Models;

public class ImageFromSkBitmap(SKBitmap bitmap) : ICalcImage
{
    private SKBitmap Bitmap { get; } = bitmap;

    public SKBitmap GetImage() => Bitmap;
}