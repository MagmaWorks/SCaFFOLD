using MagmaWorks.Taxonomy.Loads;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Loads;
public sealed class CalcGravity2d : CalcTaxonomyObject<Gravity2d>
#if NET7_0_OR_GREATER
    , IParsable<CalcGravity2d>
#endif
{
    public CalcGravity2d(Gravity2d gravity2d, string name, string symbol = "")
        : base(gravity2d, name, symbol) { }

    public CalcGravity2d(string name, string symbol = "")
        : base(new Gravity2d(), name, symbol) { }

    public CalcGravity2d(Ratio z, string name, string symbol = "")
        : base(new Gravity2d(z), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcGravity2d result)
    {
        try
        {
            result = s.FromJson<CalcGravity2d>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcGravity2d Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcGravity2d>();
    }
}
