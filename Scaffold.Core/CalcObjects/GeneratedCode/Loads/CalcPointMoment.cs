using MagmaWorks.Taxonomy.Loads;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Loads;
public sealed class CalcPointMoment : CalcTaxonomyObject<PointMoment>
#if NET7_0_OR_GREATER
    , IParsable<CalcPointMoment>
#endif
{
    public CalcPointMoment(PointMoment pointmoment, string name, string symbol = "")
        : base(pointmoment, name, symbol) { }

    public CalcPointMoment(Torque xx, Torque yy, Torque zz, string name, string symbol = "")
        : base(new PointMoment(xx, yy, zz), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcPointMoment result)
    {
        try
        {
            result = s.FromJson<CalcPointMoment>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcPointMoment Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcPointMoment>();
    }
}
