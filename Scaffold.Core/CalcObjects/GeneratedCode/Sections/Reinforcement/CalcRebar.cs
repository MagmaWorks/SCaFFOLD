using MagmaWorks.Taxonomy.Materials;
using MagmaWorks.Taxonomy.Sections.Reinforcement;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Sections.Reinforcement;
public sealed class CalcRebar : CalcTaxonomyObject<Rebar>
#if NET7_0_OR_GREATER
    , IParsable<CalcRebar>
#endif
{
    public CalcRebar(Rebar rebar, string name, string symbol = "")
        : base(rebar, name, symbol) { }

    public CalcRebar(IMaterial material, Length diameter, string name, string symbol = "")
        : base(new Rebar(material, diameter), name, symbol) { }

    public CalcRebar(IMaterial material, BarDiameter diameter, string name, string symbol = "")
        : base(new Rebar(material, diameter), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcRebar result)
    {
        try
        {
            result = s.FromJson<CalcRebar>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcRebar Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcRebar>();
    }
}
