using MagmaWorks.Geometry;
using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcPerimeter : CalcTaxonomyObject<Perimeter>
#if NET7_0_OR_GREATER
    , IParsable<CalcPerimeter>
#endif
{
    public CalcPerimeter(Perimeter perimeter, string name, string symbol = "")
        : base(perimeter, name, symbol) { }

    public CalcPerimeter(ILocalPolyline2d edge, string name, string symbol = "")
        : base(new Perimeter(edge), name, symbol) { }

    public CalcPerimeter(IList<ILocalPoint2d> edgePoints, string name, string symbol = "")
        : base(new Perimeter(edgePoints), name, symbol) { }

    public CalcPerimeter(ILocalPolyline2d outerEdge, IList<ILocalPolyline2d> voidEdges, string name, string symbol = "")
        : base(new Perimeter(outerEdge, voidEdges), name, symbol) { }

    public CalcPerimeter(IProfile profile, string name, string symbol = "")
        : base(new Perimeter(profile), name, symbol) { }

    public CalcPerimeter(IProfile profile, Length tolerance, string name, string symbol = "")
        : base(new Perimeter(profile, tolerance), name, symbol) { }

    public CalcPerimeter(IProfile profile, int divisions, string name, string symbol = "")
        : base(new Perimeter(profile, divisions), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcPerimeter result)
    {
        try
        {
            result = s.FromJson<CalcPerimeter>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcPerimeter Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcPerimeter>();
    }
}
