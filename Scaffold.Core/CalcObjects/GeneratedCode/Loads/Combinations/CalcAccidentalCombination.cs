using MagmaWorks.Taxonomy.Loads.Combinations;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Loads.Combinations;
public sealed class CalcAccidentalCombination : CalcTaxonomyObject<AccidentalCombination>
#if NET7_0_OR_GREATER
    , IParsable<CalcAccidentalCombination>
#endif
{
    public CalcAccidentalCombination(AccidentalCombination accidentalcombination, string name, string symbol = "")
        : base(accidentalcombination, name, symbol) { }

    public CalcAccidentalCombination(string name, string symbol = "")
        : base(new AccidentalCombination(), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcAccidentalCombination result)
    {
        try
        {
            result = s.FromJson<CalcAccidentalCombination>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcAccidentalCombination Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcAccidentalCombination>();
    }
}
