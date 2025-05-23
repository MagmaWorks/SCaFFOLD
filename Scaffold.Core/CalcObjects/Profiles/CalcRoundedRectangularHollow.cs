using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcRoundedRectangularHollow : CalcTaxonomyObject<RoundedRectangularHollow>
#if NET7_0_OR_GREATER
    , IParsable<CalcRoundedRectangularHollow>
#endif
{
    public CalcRoundedRectangularHollow(RoundedRectangularHollow roundedrectangularhollow, string name, string symbol = "")
        : base(roundedrectangularhollow, name, symbol) { }

    public CalcRoundedRectangularHollow(Length width, Length height, Length flatWidth, Length flatHeight, Length thickness, string name, string symbol = "")
        : base(new RoundedRectangularHollow(width, height, flatWidth, flatHeight, thickness), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcRoundedRectangularHollow result)
    {
        try
        {
            result = s.FromJson<CalcRoundedRectangularHollow>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcRoundedRectangularHollow Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcRoundedRectangularHollow>();
    }
}
