using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using MagmaWorks.Taxonomy.Serialization;
using MagmaWorks.Taxonomy.Standards.Eurocode;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Materials.StandardMaterials.En;
public sealed class CalcEnRebarMaterial : CalcTaxonomyObject<EnRebarMaterial>
#if NET7_0_OR_GREATER
    , IParsable<CalcEnRebarMaterial>
#endif
{
    public CalcEnRebarMaterial(EnRebarMaterial enrebarmaterial, string name, string symbol = "")
        : base(enrebarmaterial, name, symbol) { }

    public CalcEnRebarMaterial(EnRebarGrade grade, NationalAnnex nationalAnnex, string name, string symbol = "")
        : base(new EnRebarMaterial(grade, nationalAnnex), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcEnRebarMaterial result)
    {
        try
        {
            result = s.FromJson<CalcEnRebarMaterial>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcEnRebarMaterial Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcEnRebarMaterial>();
    }
}
