using MagmaWorks.Taxonomy.Sections.Reinforcement;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Sections.Reinforcement;
public sealed class CalcPerimeterReinforcementLayer : CalcTaxonomyObject<PerimeterReinforcementLayer>
#if NET7_0_OR_GREATER
    , IParsable<CalcPerimeterReinforcementLayer>
#endif
{
    public CalcPerimeterReinforcementLayer(PerimeterReinforcementLayer perimeterreinforcementlayer, string name, string symbol = "")
        : base(perimeterreinforcementlayer, name, symbol) { }

    public CalcPerimeterReinforcementLayer(IRebar rebar, int numberOfRebars, string name, string symbol = "")
        : base(new PerimeterReinforcementLayer(rebar, numberOfRebars), name, symbol) { }

    public CalcPerimeterReinforcementLayer(IRebar rebar, Length maxSpacing, string name, string symbol = "")
        : base(new PerimeterReinforcementLayer(rebar, maxSpacing), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcPerimeterReinforcementLayer result)
    {
        try
        {
            result = s.FromJson<CalcPerimeterReinforcementLayer>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcPerimeterReinforcementLayer Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcPerimeterReinforcementLayer>();
    }
}
