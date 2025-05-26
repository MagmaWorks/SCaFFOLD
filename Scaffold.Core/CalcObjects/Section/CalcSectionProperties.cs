using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Sections;
using MagmaWorks.Taxonomy.Sections.SectionProperties;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Sections;
public sealed class CalcSectionProperties : CalcTaxonomyObject<SectionProperties>
#if NET7_0_OR_GREATER
    , IParsable<CalcSectionProperties>
#endif
{
    public CalcSectionProperties(SectionProperties sectionproperties, string name, string symbol = "")
        : base(sectionproperties, name, symbol) { }

    public CalcSectionProperties(ISection section, string name, string symbol = "")
        : base(new SectionProperties(section), name, symbol) { }

    public CalcSectionProperties(IProfile profile, string name, string symbol = "")
        : base(new SectionProperties(profile), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcSectionProperties result)
    {
        try
        {
            result = s.FromJson<CalcSectionProperties>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcSectionProperties Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcSectionProperties>();
    }
}
