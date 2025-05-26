using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Materials.StandardMaterials.En;
public sealed class CalcEnSteelSpecification : CalcTaxonomyObject<EnSteelSpecification>
#if NET7_0_OR_GREATER
    , IParsable<CalcEnSteelSpecification>
#endif
{
    public CalcEnSteelSpecification(EnSteelSpecification ensteelspecification, string name, string symbol = "")
        : base(ensteelspecification, name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcEnSteelSpecification result)
    {
        try
        {
            result = s.FromJson<CalcEnSteelSpecification>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcEnSteelSpecification Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcEnSteelSpecification>();
    }
}
