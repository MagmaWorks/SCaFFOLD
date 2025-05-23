using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcZ : CalcTaxonomyObject<Z>
#if NET7_0_OR_GREATER
    , IParsable<CalcZ>
#endif
{
    public CalcZ(Z z, string name, string symbol = "")
        : base(z, name, symbol) { }

    public CalcZ(Length height, Length topFlangeWidth, Length bottomFlangeWidth, Length thickness, Length topLip, Length bottomLip, string name, string symbol = "")
        : base(new Z(height, topFlangeWidth, bottomFlangeWidth, thickness, topLip, bottomLip), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcZ result)
    {
        try
        {
            result = s.FromJson<CalcZ>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcZ Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcZ>();
    }
}
