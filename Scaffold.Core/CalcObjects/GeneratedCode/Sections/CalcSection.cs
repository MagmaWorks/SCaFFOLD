using MagmaWorks.Taxonomy.Materials;
using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Sections;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Sections;
public sealed class CalcSection : CalcTaxonomyObject<Section>
#if NET7_0_OR_GREATER
    , IParsable<CalcSection>
#endif
{
    public CalcSection(Section section, string name, string symbol = "")
        : base(section, name, symbol) { }

    public CalcSection(IProfile profile, IMaterial material, string name, string symbol = "")
        : base(new Section(profile, material), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcSection result)
    {
        try
        {
            result = s.FromJson<CalcSection>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcSection Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcSection>();
    }
}
