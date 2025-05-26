using MagmaWorks.Taxonomy.Loads.Combinations;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Loads.Combinations;
public sealed class CalcQuasiPermanentCombination : CalcTaxonomyObject<QuasiPermanentCombination>
#if NET7_0_OR_GREATER
    , IParsable<CalcQuasiPermanentCombination>
#endif
{
    public CalcQuasiPermanentCombination(QuasiPermanentCombination quasipermanentcombination, string name, string symbol = "")
        : base(quasipermanentcombination, name, symbol) { }

    public CalcQuasiPermanentCombination(string name, string symbol = "")
        : base(new QuasiPermanentCombination(), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcQuasiPermanentCombination result)
    {
        try
        {
            result = s.FromJson<CalcQuasiPermanentCombination>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcQuasiPermanentCombination Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcQuasiPermanentCombination>();
    }
}
