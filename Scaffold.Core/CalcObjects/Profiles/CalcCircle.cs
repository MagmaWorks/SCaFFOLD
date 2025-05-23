using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcCircle : CalcTaxonomyObject<Circle>
#if NET7_0_OR_GREATER
    , IParsable<CalcCircle>
#endif
{
    public CalcCircle(Circle circle, string name, string symbol = "")
        : base(circle, name, symbol) { }

    public CalcCircle(Length diameter, string name, string symbol = "")
        : base(new Circle(diameter), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcCircle result)
    {
        try
        {
            result = s.FromJson<CalcCircle>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcCircle Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcCircle>();
    }
}
