using MagmaWorks.Taxonomy.Materials;
using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Sections;
using MagmaWorks.Taxonomy.Sections.Reinforcement;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Sections;
public sealed class CalcConcreteSection : CalcTaxonomyObject<ConcreteSection>
#if NET7_0_OR_GREATER
    , IParsable<CalcConcreteSection>
#endif
{
    public CalcConcreteSection(ConcreteSection concretesection, string name, string symbol = "")
        : base(concretesection, name, symbol) { }

    public CalcConcreteSection(IProfile profile, IMaterial material, string name, string symbol = "")
        : base(new ConcreteSection(profile, material), name, symbol) { }

    public CalcConcreteSection(IProfile profile, IMaterial material, IRebar link, string name, string symbol = "")
        : base(new ConcreteSection(profile, material, link), name, symbol) { }

    public CalcConcreteSection(IProfile profile, IMaterial material, IRebar link, Length cover, string name, string symbol = "")
        : base(new ConcreteSection(profile, material, link, cover), name, symbol) { }

    public CalcConcreteSection(IProfile profile, IMaterial material, IRebar link, Length cover, IList<ILongitudinalReinforcement> rebars, string name, string symbol = "")
        : base(new ConcreteSection(profile, material, link, cover, rebars), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcConcreteSection result)
    {
        try
        {
            result = s.FromJson<CalcConcreteSection>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcConcreteSection Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcConcreteSection>();
    }
}
