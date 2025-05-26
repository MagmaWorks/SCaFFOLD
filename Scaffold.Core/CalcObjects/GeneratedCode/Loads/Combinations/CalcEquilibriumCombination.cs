using MagmaWorks.Taxonomy.Loads.Combinations;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Loads.Combinations;
public sealed class CalcEquilibriumCombination : CalcTaxonomyObject<EquilibriumCombination>
#if NET7_0_OR_GREATER
    , IParsable<CalcEquilibriumCombination>
#endif
{
    public CalcEquilibriumCombination(EquilibriumCombination equilibriumcombination, string name, string symbol = "")
        : base(equilibriumcombination, name, symbol) { }

    public CalcEquilibriumCombination(string name, string symbol = "")
        : base(new EquilibriumCombination(), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcEquilibriumCombination result)
    {
        try
        {
            result = s.FromJson<CalcEquilibriumCombination>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcEquilibriumCombination Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcEquilibriumCombination>();
    }
}
