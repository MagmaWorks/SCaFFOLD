using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcEllipseHollow : CalcTaxonomyObject<EllipseHollow>
#if NET7_0_OR_GREATER
    , IParsable<CalcEllipseHollow>
#endif
{
    public CalcEllipseHollow(EllipseHollow ellipsehollow, string name, string symbol = "")
        : base(ellipsehollow, name, symbol) { }

    public CalcEllipseHollow(Length width, Length height, Length thickness, string name, string symbol = "")
        : base(new EllipseHollow(width, height, thickness), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcEllipseHollow result)
    {
        try
        {
            result = s.FromJson<CalcEllipseHollow>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcEllipseHollow Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcEllipseHollow>();
    }
}
