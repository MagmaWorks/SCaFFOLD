using MagmaWorks.Taxonomy.Sections;
using MagmaWorks.Taxonomy.Sections.SectionProperties;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Sections;
public sealed class CalcConcreteSectionProperties : CalcTaxonomyObject<ConcreteSectionProperties>
#if NET7_0_OR_GREATER
    , IParsable<CalcConcreteSectionProperties>
#endif
{
    public CalcConcreteSectionProperties(ConcreteSectionProperties concretesectionproperties, string name, string symbol = "")
        : base(concretesectionproperties, name, symbol) { }

    public CalcConcreteSectionProperties(IConcreteSection section, string name, string symbol = "")
        : base(new ConcreteSectionProperties(section), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcConcreteSectionProperties result)
    {
        try
        {
            result = s.FromJson<CalcConcreteSectionProperties>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcConcreteSectionProperties Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcConcreteSectionProperties>();
    }
}
