using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcC : CalcTaxonomyObject<C>
#if NET7_0_OR_GREATER
    , IParsable<CalcC>
#endif
{
    public CalcC(C c, string name, string symbol = "")
        : base(c, name, symbol) { }

    public CalcC(Length height, Length width, Length webThickness, Length flangeThickness, Length lip, string name, string symbol = "")
        : base(new C(height, width, webThickness, flangeThickness, lip), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcC result)
    {
        try
        {
            result = s.FromJson<CalcC>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcC Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcC>();
    }
}
