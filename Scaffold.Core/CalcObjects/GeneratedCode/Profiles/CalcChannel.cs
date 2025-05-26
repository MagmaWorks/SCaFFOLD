using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcChannel : CalcTaxonomyObject<Channel>
#if NET7_0_OR_GREATER
    , IParsable<CalcChannel>
#endif
{
    public CalcChannel(Channel channel, string name, string symbol = "")
        : base(channel, name, symbol) { }

    public CalcChannel(Length height, Length width, Length webThickness, Length flangeThickness, string name, string symbol = "")
        : base(new Channel(height, width, webThickness, flangeThickness), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcChannel result)
    {
        try
        {
            result = s.FromJson<CalcChannel>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcChannel Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcChannel>();
    }
}
