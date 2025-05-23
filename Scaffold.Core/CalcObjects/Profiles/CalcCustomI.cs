using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcCustomI : CalcTaxonomyObject<CustomI>
#if NET7_0_OR_GREATER
    , IParsable<CalcCustomI>
#endif
{
    public CalcCustomI(CustomI customi, string name, string symbol = "")
        : base(customi, name, symbol) { }

    public CalcCustomI(Length height, Length topFlangeWidth, Length bottomFlangeWidth, Length topFlangeThickness, Length bottomFlangeThickness, Length webThickness, string name, string symbol = "")
        : base(new CustomI(height, topFlangeWidth, bottomFlangeWidth, topFlangeThickness, bottomFlangeThickness, webThickness), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcCustomI result)
    {
        try
        {
            result = s.FromJson<CalcCustomI>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcCustomI Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcCustomI>();
    }
}
