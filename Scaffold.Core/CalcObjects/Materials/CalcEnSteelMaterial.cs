using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Materials;
public sealed class CalcEnSteelMaterial : CalcTaxonomyObject<EnSteelMaterial>
#if NET7_0_OR_GREATER
    , IParsable<CalcEnSteelMaterial>
#endif
{
    public CalcEnSteelMaterial(EnSteelMaterial ensteelmaterial, string name, string symbol = "")
        : base(ensteelmaterial, name, symbol) { }

    public CalcEnSteelMaterial(EnSteelGrade grade, MagmaWorks.Taxonomy.Standards.Eurocode.NationalAnnex nationalAnnex, string name, string symbol = "")
        : base(new EnSteelMaterial(grade, nationalAnnex), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcEnSteelMaterial result)
    {
        try
        {
            result = s.FromJson<CalcEnSteelMaterial>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcEnSteelMaterial Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcEnSteelMaterial>();
    }
}
