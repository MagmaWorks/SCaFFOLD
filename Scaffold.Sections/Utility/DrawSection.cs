using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagmaWorks.Geometry;
using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Sections;
using UnitsNet;
using UnitsNet.Units;
using SkiaSharp;

namespace Scaffold.Core.Utility
{
    public static class DrawSection
    {
        internal static StringBuilder BeginSvg(int width, int height)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<svg xmlns='http://www.w3.org/2000/svg' width=\"{width}px\" height=\"{height}px\" " +
                $"viewBox=\"{-width / 2 - 2} {-height / 2 - 2} {width + 4} {height + 4}\" >");
            return sb;
        }

        internal static (StringBuilder svg, LengthUnit unit) BeginSvg(ISection section, string fill = "#DDDDDC", string stroke = "black")
        {
            var perimeter = new Perimeter(section.Profile);
            LengthUnit unit = perimeter.OuterEdge.Points.FirstOrDefault().Y.Unit;
            ILocalDomain2d extends = GetExtends(perimeter.OuterEdge);
            (int x, int y) = ImageExtends(extends, unit);
            StringBuilder svg = BeginSvg(x, y);
            var outline = new SKPath();
            outline.AddPoly(ConvertPoints(perimeter.OuterEdge));
            svg.AppendLine($"<path d=\"{outline.ToSvgPathData()}\"");
            if (perimeter.VoidEdges != null)
            {
                foreach (ILocalPolyline2d voidline in perimeter.VoidEdges)
                {
                    outline = new SKPath();
                    outline.AddPoly(ConvertPoints(voidline));
                    svg.AppendLine($"\"{outline.ToSvgPathData()}\"");
                }
            }
            svg.AppendLine($"fill=\"{fill}\" stroke=\"{stroke}\" fill-rule=\"evenodd\"/>");
            return (svg, unit);
        }

        internal static string EndSvg(StringBuilder svg)
        {
            svg.AppendLine("</svg>");
            return svg.ToString();
        }

        internal static StringBuilder AddCircles(StringBuilder svg, IList<ILongitudinalReinforcement> rebars,
            LengthUnit unit, string fill = "#464D5F")
        {
            foreach (var rebar in rebars)
            {
                svg.AppendLine($"<circle r=\"{rebar.Rebar.Diameter.As(unit) / 2}\" " +
                    $"cx=\"{rebar.Position.Y.As(unit)}\" " +
                    $"cy=\"{rebar.Position.Z.As(unit) * -1}\"" +
                    $" fill=\"{fill}\"/>");
            }

            return svg;
        }

        internal static StringBuilder AddRoundedRectangle(StringBuilder svg, ConcreteSection concreteSection,
            LengthUnit unit, string stroke = "#9EA2AC")
        {
            if (concreteSection.Profile is not Rectangle profile)
            {
                return svg;
            }

            double x = profile.Width.As(unit) / 2 * -1 + concreteSection.Cover.As(unit) - concreteSection.Link.Diameter.As(unit) / 2;
            double y = profile.Height.As(unit) / 2 * -1 + concreteSection.Cover.As(unit) - concreteSection.Link.Diameter.As(unit) / 2;
            double r = concreteSection.Link.Diameter.As(unit) * 2;
            double link = concreteSection.Link.Diameter.As(unit);
            svg.AppendLine($"<rect x=\"{x}\" y=\"{y}\" width=\"{x * -2}\" height=\"{y * -2}\" rx=\"{r}\" ");
            svg.AppendLine($"fill=\"none\" stroke=\"{stroke}\" stroke-width=\"{link}\" />");
            return svg;
        }

        private static SKPoint[] ConvertPoints(ILocalPolyline2d polyline)
        {
            var pts = new SKPoint[polyline.Points.Count];
            int i = 0;
            foreach (ILocalPoint2d point in polyline.Points)
            {
                pts[i++] = new SKPoint((float)point.Y.Value, (float)point.Z.Value * -1);
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
