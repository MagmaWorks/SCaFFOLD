using System;
using System.Linq;
using System.Text;
using MagmaWorks.Geometry;
using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Sections;
using OasysUnits;
using OasysUnits.Units;
using SkiaSharp;

namespace Scaffold.Core.Utility
{
    public static class DrawSection
    {


        internal static StringBuilder BeginSvg(int width, int height)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
            sb.AppendLine($"<svg width=\"{width}px\" height=\"{height}px\" " +
                $"viewBox=\"{-width / 2 - 2} {-height / 2 - 2} {width + 4} {height + 4}\" " +
                $"xmlns=\"http://www.w3.org/2000/svg\" version=\"1.1\">");
            return sb;
        }

        internal static StringBuilder BeginSvg(ISection section)
        {
            var perimeter = new Perimeter(section.Profile);
            LengthUnit unit = perimeter.OuterEdge.Points.FirstOrDefault().Y.Unit;
            ILocalDomain2d extends = GetExtends(perimeter.OuterEdge);
            (int x, int y) = ImageExtends(extends, unit);
            return BeginSvg(x, y);
        }

        internal static string EndSvg(StringBuilder svg)
        {
            svg.AppendLine("</svg>");
            return svg.ToString();
        }

        internal static StringBuilder AddPath(StringBuilder svg, ILocalPolyline2d polyline, string fill = "#DDDDDC", string stroke = "black")
        {
            var path = new SKPath();
            path.AddPoly(ConvertPoints(polyline));
            string svgPath = path.ToSvgPathData();
            svg.AppendLine($"<path d=\"{svg}\" fill=\"{fill}\" stroke=\"{stroke}\"/>");
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

        private static ILocalDomain2d GetExtends(ILocalPolyline2d polyline)
        {
            Length maxY = polyline.Points.Select(p => p.Y).Max();
            Length minY = polyline.Points.Select(p => p.Y).Min();
            Length maxZ = polyline.Points.Select(p => p.Z).Max();
            Length minZ = polyline.Points.Select(p => p.Z).Min();
            var max = new LocalPoint2d(maxY, maxZ);
            var min = new LocalPoint2d(minY, minZ);
            return new LocalDomain2d(max, min);
        }

        private static (int x, int y) ImageExtends(ILocalDomain2d domain, LengthUnit unit)
        {
            int x = (int)Math.Ceiling(domain.Max.Y.As(unit) - domain.Min.Y.As(unit));
            int y = (int)Math.Ceiling(domain.Max.Z.As(unit) - domain.Min.Z.As(unit));
            return (x, y);
        }
    }
}
