using MagmaWorks.Taxonomy.Loads.Combinations;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Loads.Combinations;
public sealed class CalcSeismicCombination : CalcTaxonomyObject<SeismicCombination>
#if NET7_0_OR_GREATER
    , IParsable<CalcSeismicCombination>
#endif
{
    public CalcSeismicCombination(SeismicCombination seismiccombination, string name, string symbol = "")
        : base(seismiccombination, name, symbol) { }

    public CalcSeismicCombination(string name, string symbol = "")
        : base(new SeismicCombination(), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcSeismicCombination result)
    {
        try
        {
            result = s.FromJson<CalcSeismicCombination>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcSeismicCombination Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcSeismicCombination>();
    }
}
