using MagmaWorks.Taxonomy.Loads.Cases;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Loads.Cases;
public sealed class CalcVariableCase : CalcTaxonomyObject<VariableCase>
#if NET7_0_OR_GREATER
    , IParsable<CalcVariableCase>
#endif
{
    public CalcVariableCase(VariableCase variablecase, string name, string symbol = "")
        : base(variablecase, name, symbol) { }

    public CalcVariableCase(string name, string symbol = "")
        : base(new VariableCase(), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcVariableCase result)
    {
        try
        {
            result = s.FromJson<CalcVariableCase>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcVariableCase Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcVariableCase>();
    }
}
