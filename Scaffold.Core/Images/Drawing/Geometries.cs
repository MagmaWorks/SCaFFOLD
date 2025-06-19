using System.Text;
using MagmaWorks.Geometry;
using SkiaSharp;

namespace Scaffold.Core.Images.Drawing
{
    public static class Geometries
    {
        public static string DrawPolygon(ILocalPolyline2d polygon, SKColor outline)
        {
            return DrawFilledPolygon(polygon, outline, SKColor.Empty);
        }
        public static string DrawFilledPolygon(ILocalPolyline2d polygon, SKColor outline, SKColor fill)
        {
            string fillColour = fill.ToString().Replace("#ff", "#");
            string strokeColour = outline.ToString().Replace("#ff", "#");

            LengthUnit unit = polygon.Points.FirstOrDefault().Y.Unit;
            ILocalDomain2d domain = GetDomain(polygon);
            (int x, int y) = GetExtents(domain, unit);
            var path = new SKPath();
            path.AddPoly(ConvertPoints(polygon));
            string svg = path.ToSvgPathData();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
            sb.AppendLine($"<svg width=\"{x}px\" height=\"{y}px\" viewBox=\"{-x / 2 - 2} {-y / 2 - 2} {x + 4} {y + 4}\" xmlns=\"http://www.w3.org/2000/svg\" version=\"1.1\">");
            sb.AppendLine($"<path d=\"{svg}\" fill=\"{fillColour}\" stroke=\"{strokeColour}\"/>");

            sb.AppendLine("</svg>");
            return sb.ToString();
        }

        private static SKPoint[] ConvertPoints(ILocalPolyline2d polyline)
        {
            var pts = new SKPoint[polyline.Points.Count];
            int i = 0;
            foreach (ILocalPoint2d point in polyline.Points)
            {
                pts[i++] = new SKPoint((float)point.Y.Value, (float)point.Z.Value);
            }

            return pts;
        }

        internal static ILocalDomain2d GetDomain(ILocalPolyline2d polyline)
        {
            Length maxY = polyline.Points.Select(p => p.Y).Max();
            Length minY = polyline.Points.Select(p => p.Y).Min();
            Length maxZ = polyline.Points.Select(p => p.Z).Max();
            Length minZ = polyline.Points.Select(p => p.Z).Min();
            var max = new LocalPoint2d(maxY, maxZ);
            var min = new LocalPoint2d(minY, minZ);
            return new LocalDomain2d(max, min);
        }

        internal static (int x, int y) GetExtents(ILocalDomain2d domain, LengthUnit unit)
        {
            int x = (int)Math.Ceiling(domain.Max.Y.As(unit) - domain.Min.Y.As(unit));
            int y = (int)Math.Ceiling(domain.Max.Z.As(unit) - domain.Min.Z.As(unit));
            return (x, y);
        }
    }
}
