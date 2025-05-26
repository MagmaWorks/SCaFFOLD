using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcI : CalcTaxonomyObject<I>
#if NET7_0_OR_GREATER
    , IParsable<CalcI>
#endif
{
    public CalcI(I i, string name, string symbol = "")
        : base(i, name, symbol) { }

    public CalcI(Length height, Length width, Length flangeThickness, Length webThickness, string name, string symbol = "")
        : base(new I(height, width, flangeThickness, webThickness), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcI result)
    {
        try
        {
            result = s.FromJson<CalcI>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcI Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcI>();
    }
}
