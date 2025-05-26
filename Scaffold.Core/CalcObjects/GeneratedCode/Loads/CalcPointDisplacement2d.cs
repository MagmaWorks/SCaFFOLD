using MagmaWorks.Taxonomy.Loads;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Loads;
public sealed class CalcPointDisplacement2d : CalcTaxonomyObject<PointDisplacement2d>
#if NET7_0_OR_GREATER
    , IParsable<CalcPointDisplacement2d>
#endif
{
    public CalcPointDisplacement2d(PointDisplacement2d pointdisplacement2d, string name, string symbol = "")
        : base(pointdisplacement2d, name, symbol) { }

    public CalcPointDisplacement2d(Length x, Length z, string name, string symbol = "")
        : base(new PointDisplacement2d(x, z), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcPointDisplacement2d result)
    {
        try
        {
            result = s.FromJson<CalcPointDisplacement2d>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcPointDisplacement2d Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcPointDisplacement2d>();
    }
}
