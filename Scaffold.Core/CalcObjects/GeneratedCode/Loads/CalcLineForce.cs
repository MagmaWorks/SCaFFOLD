using MagmaWorks.Taxonomy.Loads;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Loads;
public sealed class CalcLineForce : CalcTaxonomyObject<LineForce>
#if NET7_0_OR_GREATER
    , IParsable<CalcLineForce>
#endif
{
    public CalcLineForce(LineForce lineforce, string name, string symbol = "")
        : base(lineforce, name, symbol) { }

    public CalcLineForce(ForcePerLength z, string name, string symbol = "")
        : base(new LineForce(z), name, symbol) { }

    public CalcLineForce(ForcePerLength x, ForcePerLength y, ForcePerLength z, LoadApplication application, string name, string symbol = "")
        : base(new LineForce(x, y, z, application), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcLineForce result)
    {
        try
        {
            result = s.FromJson<CalcLineForce>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcLineForce Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcLineForce>();
    }
}
