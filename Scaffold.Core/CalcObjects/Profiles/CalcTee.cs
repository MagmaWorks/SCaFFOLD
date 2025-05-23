using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcTee : CalcTaxonomyObject<Tee>
#if NET7_0_OR_GREATER
    , IParsable<CalcTee>
#endif
{
    public CalcTee(Tee tee, string name, string symbol = "")
        : base(tee, name, symbol) { }

    public CalcTee(Length height, Length width, Length flangeThickness, Length webThickness, string name, string symbol = "")
        : base(new Tee(height, width, flangeThickness, webThickness), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcTee result)
    {
        try
        {
            result = s.FromJson<CalcTee>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcTee Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcTee>();
    }
}
