using System;
using System.IO;
using System.Linq;
using System.Text;
using MagmaWorks.Geometry;
using MagmaWorks.Taxonomy.Profiles;
using OasysUnits;
using OasysUnits.Units;
using SkiaSharp;

namespace Scaffold.Core.Utility
{
    public static class SectionDrawing
    {
        public static string DrawProfileSvg(IProfile profile)
        {
            var perimeter = new Perimeter(profile);

            LengthUnit unit = perimeter.OuterEdge.Points.FirstOrDefault().Y.Unit;
            ILocalDomain2d extends = GetExtends(perimeter.OuterEdge);
            (int x, int y) = ImageExtends(extends, unit);
            var path = new SKPath();
            path.AddPoly(ConvertPoints(perimeter.OuterEdge));
            string svg = path.ToSvgPathData();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
            sb.AppendLine($"<svg width=\"{x}px\" height=\"{y}px\" viewBox=\"{-x / 2 - 2} {-y / 2 - 2} {x + 4} {y + 4}\" xmlns=\"http://www.w3.org/2000/svg\" version=\"1.1\">");
            sb.AppendLine($"<path d=\"{svg}\" fill=\"#DDDDDC\" stroke=\"black\"/>");

            sb.AppendLine("</svg>");
            string file = @"C:\Users\k.nielsen\Desktop\svg-test.svg";
            File.WriteAllText(file, sb.ToString());
            return svg;
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

        internal static ILocalDomain2d GetExtends(ILocalPolyline2d polyline)
        {
            Length maxY = polyline.Points.Select(p => p.Y).Max();
            Length minY = polyline.Points.Select(p => p.Y).Min();
            Length maxZ = polyline.Points.Select(p => p.Z).Max();
            Length minZ = polyline.Points.Select(p => p.Z).Min();
            var max = new LocalPoint2d(maxY, maxZ);
            var min = new LocalPoint2d(minY, minZ);
            return new LocalDomain2d(max, min);
        }

        internal static (int x, int y) ImageExtends(ILocalDomain2d domain, LengthUnit unit)
        {
            int x = (int)Math.Ceiling(domain.Max.Y.As(unit) - domain.Min.Y.As(unit));
            int y = (int)Math.Ceiling(domain.Max.Z.As(unit) - domain.Min.Z.As(unit));
            return (x, y);
        }
    }
}
