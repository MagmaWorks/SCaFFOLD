using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcDoubleChannel : CalcTaxonomyObject<DoubleChannel>
#if NET7_0_OR_GREATER
    , IParsable<CalcDoubleChannel>
#endif
{
    public CalcDoubleChannel(DoubleChannel doublechannel, string name, string symbol = "")
        : base(doublechannel, name, symbol) { }

    public CalcDoubleChannel(Length height, Length width, Length webThickness, Length flangeThickness, Length backToBackDistance, string name, string symbol = "")
        : base(new DoubleChannel(height, width, webThickness, flangeThickness, backToBackDistance), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcDoubleChannel result)
    {
        try
        {
            result = s.FromJson<CalcDoubleChannel>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcDoubleChannel Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcDoubleChannel>();
    }
}
