using MagmaWorks.Taxonomy.Loads;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Loads;
public sealed class CalcLineForce2d : CalcTaxonomyObject<LineForce2d>
#if NET7_0_OR_GREATER
    , IParsable<CalcLineForce2d>
#endif
{
    public CalcLineForce2d(LineForce2d lineforce2d, string name, string symbol = "")
        : base(lineforce2d, name, symbol) { }

    public CalcLineForce2d(ForcePerLength z, string name, string symbol = "")
        : base(new LineForce2d(z), name, symbol) { }

    public CalcLineForce2d(ForcePerLength x, ForcePerLength z, LoadApplication application, string name, string symbol = "")
        : base(new LineForce2d(x, z, application), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcLineForce2d result)
    {
        try
        {
            result = s.FromJson<CalcLineForce2d>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcLineForce2d Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcLineForce2d>();
    }
}
