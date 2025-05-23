using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcDoubleAngle : CalcTaxonomyObject<DoubleAngle>
#if NET7_0_OR_GREATER
    , IParsable<CalcDoubleAngle>
#endif
{
    public CalcDoubleAngle(DoubleAngle doubleangle, string name, string symbol = "")
        : base(doubleangle, name, symbol) { }

    public CalcDoubleAngle(Length height, Length width, Length webThickness, Length flangeThickness, Length backToBackDistance, string name, string symbol = "")
        : base(new DoubleAngle(height, width, webThickness, flangeThickness, backToBackDistance), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcDoubleAngle result)
    {
        try
        {
            result = s.FromJson<CalcDoubleAngle>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcDoubleAngle Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcDoubleAngle>();
    }
}
