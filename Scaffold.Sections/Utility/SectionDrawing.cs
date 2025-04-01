using MagmaWorks.Geometry;
using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Profiles.PerimeterFactory;
using SkiaSharp;

namespace Scaffold.Core.Utility
{
    internal static class SectionDrawing
    {


        internal static SKBitmap DrawProfile(IProfile profile)
        {
            var keyImage = new SKBitmap(400, 400);

            Perimeter perimeter = new Perimeter(profile).OuterEdge.;

            using (var canvas = new SKCanvas(keyImage))
            {
                SKPaint strokePaint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = SKColors.Black,
                    StrokeWidth = 10,
                    IsAntialias = true,
                };

                SKPath path = new SKPath();
                path.AddPoly(ConvertPoints(perimeter.OuterEdge));
            }

            SKCanvas sKCanvas = new SKCanvas(keyImage);
            sKCanvas.

            return keyImage;
        }

        

        private static SKPoint[] ConvertPoints(ILocalPolyline2d polyline) {
            var pts = new SKPoint[polyline.Points.Count];
            int i = 0;
            foreach (var point in polyline.Points)
            {
                pts[i++] = new SKPoint((float)point.Y.Value, (float)point.Z.Value);
            }
            return pts;
        }
    }
}
