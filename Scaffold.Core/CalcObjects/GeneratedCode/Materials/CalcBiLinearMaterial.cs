using MagmaWorks.Taxonomy.Materials;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Materials;
public sealed class CalcBiLinearMaterial : CalcTaxonomyObject<BiLinearMaterial>
#if NET7_0_OR_GREATER
    , IParsable<CalcBiLinearMaterial>
#endif
{
    public CalcBiLinearMaterial(BiLinearMaterial bilinearmaterial, string name, string symbol = "")
        : base(bilinearmaterial, name, symbol) { }

    public CalcBiLinearMaterial(MaterialType type, Pressure elasticModulus, Pressure yieldStrength, Pressure ultimateStrength, Ratio failureStrain, string name, string symbol = "")
        : base(new BiLinearMaterial(type, elasticModulus, yieldStrength, ultimateStrength, failureStrain), name, symbol) { }

    public CalcBiLinearMaterial(ILinearElasticMaterial linearElasticMaterial, Pressure ultimateStrength, Ratio failureStrain, string name, string symbol = "")
        : base(new BiLinearMaterial(linearElasticMaterial, ultimateStrength, failureStrain), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcBiLinearMaterial result)
    {
        try
        {
            result = s.FromJson<CalcBiLinearMaterial>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcBiLinearMaterial Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcBiLinearMaterial>();
    }
}
