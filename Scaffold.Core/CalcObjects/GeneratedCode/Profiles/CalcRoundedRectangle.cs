using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcRoundedRectangle : CalcTaxonomyObject<RoundedRectangle>
#if NET7_0_OR_GREATER
    , IParsable<CalcRoundedRectangle>
#endif
{
    public CalcRoundedRectangle(RoundedRectangle roundedrectangle, string name, string symbol = "")
        : base(roundedrectangle, name, symbol) { }

    public CalcRoundedRectangle(Length width, Length height, Length flatWidth, Length flatHeight, string name, string symbol = "")
        : base(new RoundedRectangle(width, height, flatWidth, flatHeight), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcRoundedRectangle result)
    {
        try
        {
            result = s.FromJson<CalcRoundedRectangle>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcRoundedRectangle Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcRoundedRectangle>();
    }
}
