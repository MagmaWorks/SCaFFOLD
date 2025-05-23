using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcCruciform : CalcTaxonomyObject<Cruciform>
#if NET7_0_OR_GREATER
    , IParsable<CalcCruciform>
#endif
{
    public CalcCruciform(Cruciform cruciform, string name, string symbol = "")
        : base(cruciform, name, symbol) { }

    public CalcCruciform(Length height, Length width, Length flangeThickness, Length webThickness, string name, string symbol = "")
        : base(new Cruciform(height, width, flangeThickness, webThickness), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcCruciform result)
    {
        try
        {
            result = s.FromJson<CalcCruciform>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcCruciform Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcCruciform>();
    }
}
