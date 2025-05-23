using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcCircularHollow : CalcTaxonomyObject<CircularHollow>
#if NET7_0_OR_GREATER
    , IParsable<CalcCircularHollow>
#endif
{
    public CalcCircularHollow(CircularHollow circularhollow, string name, string symbol = "")
        : base(circularhollow, name, symbol) { }

    public CalcCircularHollow(Length diameter, Length thickness, string name, string symbol = "")
        : base(new CircularHollow(diameter, thickness), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcCircularHollow result)
    {
        try
        {
            result = s.FromJson<CalcCircularHollow>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcCircularHollow Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcCircularHollow>();
    }
}
