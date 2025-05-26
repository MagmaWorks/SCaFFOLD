using MagmaWorks.Taxonomy.Sections.Reinforcement;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Sections.Reinforcement;
public sealed class CalcReinforcementLayoutBySpacing : CalcTaxonomyObject<ReinforcementLayoutBySpacing>
#if NET7_0_OR_GREATER
    , IParsable<CalcReinforcementLayoutBySpacing>
#endif
{
    public CalcReinforcementLayoutBySpacing(ReinforcementLayoutBySpacing reinforcementlayoutbyspacing, string name, string symbol = "")
        : base(reinforcementlayoutbyspacing, name, symbol) { }

    public CalcReinforcementLayoutBySpacing(IRebar rebar, Length maxSpacing, string name, string symbol = "")
        : base(new ReinforcementLayoutBySpacing(rebar, maxSpacing), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcReinforcementLayoutBySpacing result)
    {
        try
        {
            result = s.FromJson<CalcReinforcementLayoutBySpacing>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcReinforcementLayoutBySpacing Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcReinforcementLayoutBySpacing>();
    }
}
