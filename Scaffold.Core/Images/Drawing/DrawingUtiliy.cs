using System.IO;
using Scaffold.Core.Images.Models;
using SkiaSharp;

namespace Scaffold.Core.Images.Drawing
{
    public static class DrawingUtiliy
    {
        private static readonly int Padding = 16;
        public static void Save(ICalcImage image, string filenameAndPath,
            SKEncodedImageFormat format = SKEncodedImageFormat.Png, int quality = 100)
        {
            SKBitmap bitmap = image.GetImage();
            var canvas = new SKCanvas(bitmap);
            FileStream output = File.OpenWrite($"{filenameAndPath}.{format.ToString().ToLower()}");
            bitmap.Encode(output, format, quality);
            output.Close();
            canvas.Discard();
        }

        public static ICalcImage DrawFilledPath(this SKPath path, SKColor stroke, SKColor fill, int scalar = 4)
        {
            return DrawFilledPaths(path, stroke, fill, null, SKColor.Empty, SKColor.Empty, scalar);
        }

        public static ICalcImage DrawFilledPaths(SKPath primaryPath, SKColor primaryStroke, SKColor primaryFill,
            SKPath internalPath, SKColor internalStroke, SKColor internalFill, int scalar = 4)
        {
            primaryPath.GetBounds(out SKRect rect);
            var bitmap = new SKBitmap((int)rect.Width * scalar + Padding, (int)rect.Height * scalar + Padding);
            var canvas = new SKCanvas(bitmap);
            var pathPositiveCoordinate = new SKPath(primaryPath);
            pathPositiveCoordinate.Transform(SKMatrix.CreateScale(scalar, scalar));
            pathPositiveCoordinate.Offset(((int)rect.Width * scalar + Padding) / 2, ((int)rect.Height * scalar + Padding) / 2);
            SKPaint paint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = primaryFill,
            };

            canvas.DrawPath(pathPositiveCoordinate, paint);
            paint.Style = SKPaintStyle.Stroke;
            paint.Color = primaryStroke;
            paint.StrokeWidth = scalar;
            canvas.DrawPath(pathPositiveCoordinate, paint);

            if (internalPath != null)
            {
                var internalPathPositiveCoordinate = new SKPath(internalPath);
                internalPathPositiveCoordinate.Transform(SKMatrix.CreateScale(scalar, scalar));
                internalPathPositiveCoordinate.Offset(((int)rect.Width * scalar + Padding) / 2, ((int)rect.Height * scalar + Padding) / 2);
                paint = new SKPaint()
                {
                    Style = SKPaintStyle.Fill,
                    Color = internalFill,
                };

                canvas.DrawPath(internalPathPositiveCoordinate, paint);
                paint.Style = SKPaintStyle.Stroke;
                paint.Color = internalStroke;
                paint.StrokeWidth = scalar;
                canvas.DrawPath(internalPathPositiveCoordinate, paint);
            }

            return new ImageFromSkBitmap(bitmap);
        }
    }
}
