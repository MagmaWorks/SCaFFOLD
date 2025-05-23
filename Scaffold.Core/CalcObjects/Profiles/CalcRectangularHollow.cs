using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcRectangularHollow : CalcTaxonomyObject<RectangularHollow>
#if NET7_0_OR_GREATER
    , IParsable<CalcRectangularHollow>
#endif
{
    public CalcRectangularHollow(RectangularHollow rectangularhollow, string name, string symbol = "")
        : base(rectangularhollow, name, symbol) { }

    public CalcRectangularHollow(Length width, Length height, Length thickness, string name, string symbol = "")
        : base(new RectangularHollow(width, height, thickness), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcRectangularHollow result)
    {
        try
        {
            result = s.FromJson<CalcRectangularHollow>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcRectangularHollow Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcRectangularHollow>();
    }
}
