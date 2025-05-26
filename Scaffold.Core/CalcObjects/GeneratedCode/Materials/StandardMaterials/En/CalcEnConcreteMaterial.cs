using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using MagmaWorks.Taxonomy.Serialization;
using MagmaWorks.Taxonomy.Standards.Eurocode;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Materials.StandardMaterials.En;
public sealed class CalcEnConcreteMaterial : CalcTaxonomyObject<EnConcreteMaterial>
#if NET7_0_OR_GREATER
    , IParsable<CalcEnConcreteMaterial>
#endif
{
    public CalcEnConcreteMaterial(EnConcreteMaterial enconcretematerial, string name, string symbol = "")
        : base(enconcretematerial, name, symbol) { }

    public CalcEnConcreteMaterial(EnConcreteGrade grade, NationalAnnex nationalAnnex, string name, string symbol = "")
        : base(new EnConcreteMaterial(grade, nationalAnnex), name, symbol) { }

    public CalcEnConcreteMaterial(EnConcreteGrade grade, NationalAnnex nationalAnnex, EnConcreteExposureClass exposureClass, Length maxAggregateSize, EnCementClass cementClass, string name, string symbol = "")
        : base(new EnConcreteMaterial(grade, nationalAnnex, exposureClass, maxAggregateSize, cementClass), name, symbol) { }

    public CalcEnConcreteMaterial(EnConcreteGrade grade, NationalAnnex nationalAnnex, EnConcreteExposureClass exposureClass, Length maxAggregateSize, EnCementClass cementClass, Length crackWidthLimit, Length minimumCover, string name, string symbol = "")
        : base(new EnConcreteMaterial(grade, nationalAnnex, exposureClass, maxAggregateSize, cementClass, crackWidthLimit, minimumCover), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcEnConcreteMaterial result)
    {
        try
        {
            result = s.FromJson<CalcEnConcreteMaterial>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcEnConcreteMaterial Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcEnConcreteMaterial>();
    }
}
