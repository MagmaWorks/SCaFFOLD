using MagmaWorks.Taxonomy.Loads;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Loads;
public sealed class CalcPointMoment2d : CalcTaxonomyObject<PointMoment2d>
#if NET7_0_OR_GREATER
    , IParsable<CalcPointMoment2d>
#endif
{
    public CalcPointMoment2d(PointMoment2d pointmoment2d, string name, string symbol = "")
        : base(pointmoment2d, name, symbol) { }

    public CalcPointMoment2d(Torque yy, Torque zz, string name, string symbol = "")
        : base(new PointMoment2d(yy, zz), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcPointMoment2d result)
    {
        try
        {
            result = s.FromJson<CalcPointMoment2d>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcPointMoment2d Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcPointMoment2d>();
    }
}
