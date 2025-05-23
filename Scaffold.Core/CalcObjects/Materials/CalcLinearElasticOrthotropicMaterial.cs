using MagmaWorks.Taxonomy.Materials;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Materials;
public sealed class CalcLinearElasticOrthotropicMaterial : CalcTaxonomyObject<LinearElasticOrthotropicMaterial>
#if NET7_0_OR_GREATER
    , IParsable<CalcLinearElasticOrthotropicMaterial>
#endif
{
    public CalcLinearElasticOrthotropicMaterial(LinearElasticOrthotropicMaterial linearelasticorthotropicmaterial, string name, string symbol = "")
        : base(linearelasticorthotropicmaterial, name, symbol) { }

    public CalcLinearElasticOrthotropicMaterial(MaterialType type, Pressure elasticModulusX, Pressure strengthX, Pressure elasticModulusY, Pressure strengthY, Pressure elasticModulusZ, Pressure strengthZ, string name, string symbol = "")
        : base(new LinearElasticOrthotropicMaterial(type, elasticModulusX, strengthX, elasticModulusY, strengthY, elasticModulusZ, strengthZ), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcLinearElasticOrthotropicMaterial result)
    {
        try
        {
            result = s.FromJson<CalcLinearElasticOrthotropicMaterial>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcLinearElasticOrthotropicMaterial Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcLinearElasticOrthotropicMaterial>();
    }
}
