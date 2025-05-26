using MagmaWorks.Taxonomy.Loads;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Loads;
public sealed class CalcPointForce2d : CalcTaxonomyObject<PointForce2d>
#if NET7_0_OR_GREATER
    , IParsable<CalcPointForce2d>
#endif
{
    public CalcPointForce2d(PointForce2d pointforce2d, string name, string symbol = "")
        : base(pointforce2d, name, symbol) { }

    public CalcPointForce2d(Force z, string name, string symbol = "")
        : base(new PointForce2d(z), name, symbol) { }

    public CalcPointForce2d(Force x, Force z, string name, string symbol = "")
        : base(new PointForce2d(x, z), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcPointForce2d result)
    {
        try
        {
            result = s.FromJson<CalcPointForce2d>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcPointForce2d Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcPointForce2d>();
    }
}
