using MagmaWorks.Taxonomy.Materials;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Materials;
public sealed class CalcParabolaRectangleMaterial : CalcTaxonomyObject<ParabolaRectangleMaterial>
#if NET7_0_OR_GREATER
    , IParsable<CalcParabolaRectangleMaterial>
#endif
{
    public CalcParabolaRectangleMaterial(ParabolaRectangleMaterial parabolarectanglematerial, string name, string symbol = "")
        : base(parabolarectanglematerial, name, symbol) { }

    public CalcParabolaRectangleMaterial(MaterialType type, Pressure yieldStrength, Ratio yieldStrain, Ratio failureStrain, System.Double exponent, string name, string symbol = "")
        : base(new ParabolaRectangleMaterial(type, yieldStrength, yieldStrain, failureStrain, exponent), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcParabolaRectangleMaterial result)
    {
        try
        {
            result = s.FromJson<CalcParabolaRectangleMaterial>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcParabolaRectangleMaterial Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcParabolaRectangleMaterial>();
    }
}
