using MagmaWorks.Taxonomy.Loads;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Loads;
public sealed class CalcColumnLoad : CalcTaxonomyObject<ColumnLoad>
#if NET7_0_OR_GREATER
    , IParsable<CalcColumnLoad>
#endif
{
    public CalcColumnLoad(ColumnLoad columnload, string name, string symbol = "")
        : base(columnload, name, symbol) { }

    public CalcColumnLoad(Force force, string name, string symbol = "")
        : base(new ColumnLoad(force), name, symbol) { }

    public CalcColumnLoad(Force force, IPointMoment2d topMoment, IPointMoment2d bottomMoment, string name, string symbol = "")
        : base(new ColumnLoad(force, topMoment, bottomMoment), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcColumnLoad result)
    {
        try
        {
            result = s.FromJson<CalcColumnLoad>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcColumnLoad Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcColumnLoad>();
    }
}
