using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcCatalogueProfile : CalcTaxonomyObject<ICatalogue>
#if NET7_0_OR_GREATER
    , IParsable<CalcCatalogueProfile>
#endif
{
    public CalcCatalogueProfile(European profile, string name, string symbol = "")
        : base(CatalogueFactory.CreateEuropean(profile), name, symbol) { }

    public CalcCatalogueProfile(American profile, string name, string symbol = "")
        : base(CatalogueFactory.CreateAmerican(profile), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcCatalogueProfile result)
    {
        try
        {
            result = s.FromJson<CalcCatalogueProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcCatalogueProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcCatalogueProfile>();
    }
}
