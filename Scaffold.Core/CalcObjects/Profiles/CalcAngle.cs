using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;
using Angle = MagmaWorks.Taxonomy.Profiles.Angle;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcAngle : CalcTaxonomyObject<Angle>
#if NET7_0_OR_GREATER
    , IParsable<CalcAngle>
#endif
{
    public CalcAngle(Angle angle, string name, string symbol = "")
        : base(angle, name, symbol) { }

    public CalcAngle(Length height, Length width, Length webThickness, Length flangeThickness, string name, string symbol = "")
        : base(new Angle(height, width, webThickness, flangeThickness), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcAngle result)
    {
        try
        {
            result = s.FromJson<CalcAngle>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcAngle Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcAngle>();
    }
}
