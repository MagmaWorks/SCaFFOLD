using MagmaWorks.Taxonomy.Sections.Reinforcement;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Sections.Reinforcement;
public sealed class CalcReinforcementLayoutByCount : CalcTaxonomyObject<ReinforcementLayoutByCount>
#if NET7_0_OR_GREATER
    , IParsable<CalcReinforcementLayoutByCount>
#endif
{
    public CalcReinforcementLayoutByCount(ReinforcementLayoutByCount reinforcementlayoutbycount, string name, string symbol = "")
        : base(reinforcementlayoutbycount, name, symbol) { }

    public CalcReinforcementLayoutByCount(IRebar rebar, int numberOfBars, string name, string symbol = "")
        : base(new ReinforcementLayoutByCount(rebar, numberOfBars), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcReinforcementLayoutByCount result)
    {
        try
        {
            result = s.FromJson<CalcReinforcementLayoutByCount>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcReinforcementLayoutByCount Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcReinforcementLayoutByCount>();
    }
}
