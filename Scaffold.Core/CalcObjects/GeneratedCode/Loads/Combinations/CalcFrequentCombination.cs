using MagmaWorks.Taxonomy.Loads.Combinations;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Loads.Combinations;
public sealed class CalcFrequentCombination : CalcTaxonomyObject<FrequentCombination>
#if NET7_0_OR_GREATER
    , IParsable<CalcFrequentCombination>
#endif
{
    public CalcFrequentCombination(FrequentCombination frequentcombination, string name, string symbol = "")
        : base(frequentcombination, name, symbol) { }

    public CalcFrequentCombination(string name, string symbol = "")
        : base(new FrequentCombination(), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcFrequentCombination result)
    {
        try
        {
            result = s.FromJson<CalcFrequentCombination>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcFrequentCombination Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcFrequentCombination>();
    }
}
