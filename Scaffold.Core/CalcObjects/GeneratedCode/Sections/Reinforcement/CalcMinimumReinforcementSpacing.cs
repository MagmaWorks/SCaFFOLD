using MagmaWorks.Taxonomy.Sections.Reinforcement;
using MagmaWorks.Taxonomy.Serialization;
using MagmaWorks.Taxonomy.Standards.Eurocode;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Sections.Reinforcement;
public sealed class CalcMinimumReinforcementSpacing : CalcTaxonomyObject<MinimumReinforcementSpacing>
#if NET7_0_OR_GREATER
    , IParsable<CalcMinimumReinforcementSpacing>
#endif
{
    public CalcMinimumReinforcementSpacing(MinimumReinforcementSpacing minimumreinforcementspacing, string name, string symbol = "")
        : base(minimumreinforcementspacing, name, symbol) { }

    public CalcMinimumReinforcementSpacing(string name, string symbol = "")
        : base(new MinimumReinforcementSpacing(), name, symbol) { }

    public CalcMinimumReinforcementSpacing(NationalAnnex nationalAnnex, string name, string symbol = "")
        : base(new MinimumReinforcementSpacing(nationalAnnex), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcMinimumReinforcementSpacing result)
    {
        try
        {
            result = s.FromJson<CalcMinimumReinforcementSpacing>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcMinimumReinforcementSpacing Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcMinimumReinforcementSpacing>();
    }
}
