using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcTrapezoid : CalcTaxonomyObject<Trapezoid>
#if NET7_0_OR_GREATER
    , IParsable<CalcTrapezoid>
#endif
{
    public CalcTrapezoid(Trapezoid trapezoid, string name, string symbol = "")
        : base(trapezoid, name, symbol) { }

    public CalcTrapezoid(Length topWidth, Length bottomWidth, Length height, string name, string symbol = "")
        : base(new Trapezoid(topWidth, bottomWidth, height), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcTrapezoid result)
    {
        try
        {
            result = s.FromJson<CalcTrapezoid>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcTrapezoid Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcTrapezoid>();
    }
}
