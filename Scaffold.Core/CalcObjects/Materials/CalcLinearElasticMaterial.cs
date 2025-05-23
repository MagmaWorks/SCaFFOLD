using MagmaWorks.Taxonomy.Materials;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Materials;
public sealed class CalcLinearElasticMaterial : CalcTaxonomyObject<LinearElasticMaterial>
#if NET7_0_OR_GREATER
    , IParsable<CalcLinearElasticMaterial>
#endif
{
    public CalcLinearElasticMaterial(LinearElasticMaterial linearelasticmaterial, string name, string symbol = "")
        : base(linearelasticmaterial, name, symbol) { }

    public CalcLinearElasticMaterial(MaterialType type, Pressure elasticModulus, Pressure strength, string name, string symbol = "")
        : base(new LinearElasticMaterial(type, elasticModulus, strength), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcLinearElasticMaterial result)
    {
        try
        {
            result = s.FromJson<CalcLinearElasticMaterial>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcLinearElasticMaterial Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcLinearElasticMaterial>();
    }
}
