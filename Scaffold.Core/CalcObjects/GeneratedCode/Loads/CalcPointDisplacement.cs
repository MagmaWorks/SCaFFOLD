using MagmaWorks.Taxonomy.Loads;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Loads;
public sealed class CalcPointDisplacement : CalcTaxonomyObject<PointDisplacement>
#if NET7_0_OR_GREATER
    , IParsable<CalcPointDisplacement>
#endif
{
    public CalcPointDisplacement(PointDisplacement pointdisplacement, string name, string symbol = "")
        : base(pointdisplacement, name, symbol) { }

    public CalcPointDisplacement(Length x, Length y, Length z, string name, string symbol = "")
        : base(new PointDisplacement(x, y, z), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcPointDisplacement result)
    {
        try
        {
            result = s.FromJson<CalcPointDisplacement>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcPointDisplacement Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcPointDisplacement>();
    }
}
