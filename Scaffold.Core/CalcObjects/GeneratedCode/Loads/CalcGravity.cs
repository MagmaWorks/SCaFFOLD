using MagmaWorks.Taxonomy.Loads;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Loads;
public sealed class CalcGravity : CalcTaxonomyObject<Gravity>
#if NET7_0_OR_GREATER
    , IParsable<CalcGravity>
#endif
{
    public CalcGravity(Gravity gravity, string name, string symbol = "")
        : base(gravity, name, symbol) { }

    public CalcGravity(string name, string symbol = "")
        : base(new Gravity(), name, symbol) { }

    public CalcGravity(Ratio z, string name, string symbol = "")
        : base(new Gravity(z), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcGravity result)
    {
        try
        {
            result = s.FromJson<CalcGravity>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcGravity Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcGravity>();
    }
}
