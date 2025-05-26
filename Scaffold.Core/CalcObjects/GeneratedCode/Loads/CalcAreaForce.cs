using MagmaWorks.Taxonomy.Loads;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Loads;
public sealed class CalcAreaForce : CalcTaxonomyObject<AreaForce>
#if NET7_0_OR_GREATER
    , IParsable<CalcAreaForce>
#endif
{
    public CalcAreaForce(AreaForce areaforce, string name, string symbol = "")
        : base(areaforce, name, symbol) { }

    public CalcAreaForce(Pressure z, string name, string symbol = "")
        : base(new AreaForce(z), name, symbol) { }

    public CalcAreaForce(Pressure x, Pressure y, Pressure z, LoadApplication application, string name, string symbol = "")
        : base(new AreaForce(x, y, z, application), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcAreaForce result)
    {
        try
        {
            result = s.FromJson<CalcAreaForce>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcAreaForce Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcAreaForce>();
    }
}
