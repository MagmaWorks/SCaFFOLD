using MagmaWorks.Taxonomy.Materials;
using MagmaWorks.Taxonomy.Sections;
using SkiaSharp;

namespace Scaffold.Core.Images.Drawing
{
    public static class Sections
    {
        private static readonly LengthUnit Unit = LengthUnit.Millimeter;

        public static ICalcImage DrawSection(this ISection section)
        {
            if (section is IConcreteSection concreteSection)
            {
                return concreteSection.DrawConcreteSection();
            }

            var path = new SKPath();
            path.AddProfile(section.Profile);

            (SKColor stroke, SKColor fill) = GetMaterialColours(section.Material);
            return path.DrawFilledPath(stroke, fill);
        }

        public static ICalcImage DrawConcreteSection(this IConcreteSection section)
        {
            var path = new SKPath();
            path.AddProfile(section.Profile);

            var rebarPath = new SKPath();
            foreach (var rebar in section.Rebars)
            {
                rebarPath.AddCircle((float)rebar.Position.Y.As(Unit), (float)rebar.Position.Z.As(Unit),
                    (float)rebar.Rebar.Diameter.As(Unit) / 2);
            }

            (SKColor stroke, SKColor fill) = GetMaterialColours(section.Material);
            return DrawingUtiliy.DrawFilledPaths(path, stroke, fill, rebarPath, stroke, Colours.RebarFill);
        }

        private static (SKColor Stroke, SKColor Fill) GetMaterialColours(IMaterial material)
        {
            switch (material.Type)
            {
                case MaterialType.Concrete:
                    return (Colours.ConcreteOutline, Colours.ConcreteFill);

                case MaterialType.Steel:
                    return (Colours.SteelOutline, Colours.SteelFill);

                default:
                    return (Colours.BlueDark, Colours.BlueLight);
            }
        }
    }
}
