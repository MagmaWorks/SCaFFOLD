using MagmaWorks.Taxonomy.Loads.Cases;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Loads.Cases;
public sealed class CalcPermanentCase : CalcTaxonomyObject<PermanentCase>
#if NET7_0_OR_GREATER
    , IParsable<CalcPermanentCase>
#endif
{
    public CalcPermanentCase(PermanentCase permanentcase, string name, string symbol = "")
        : base(permanentcase, name, symbol) { }

    public CalcPermanentCase(string name, string symbol = "")
        : base(new PermanentCase(), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcPermanentCase result)
    {
        try
        {
            result = s.FromJson<CalcPermanentCase>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcPermanentCase Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcPermanentCase>();
    }
}
