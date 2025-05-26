using MagmaWorks.Taxonomy.Loads;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Loads;
public sealed class CalcPointForce : CalcTaxonomyObject<PointForce>
#if NET7_0_OR_GREATER
    , IParsable<CalcPointForce>
#endif
{
    public CalcPointForce(PointForce pointforce, string name, string symbol = "")
        : base(pointforce, name, symbol) { }

    public CalcPointForce(Force x, Force y, Force z, string name, string symbol = "")
        : base(new PointForce(x, y, z), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcPointForce result)
    {
        try
        {
            result = s.FromJson<CalcPointForce>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcPointForce Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcPointForce>();
    }
}
