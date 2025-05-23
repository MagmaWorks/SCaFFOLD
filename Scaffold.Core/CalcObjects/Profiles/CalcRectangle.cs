using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcRectangle : CalcTaxonomyObject<Rectangle>
#if NET7_0_OR_GREATER
    , IParsable<CalcRectangle>
#endif
{
    public CalcRectangle(Rectangle rectangle, string name, string symbol = "")
        : base(rectangle, name, symbol) { }

    public CalcRectangle(Length width, Length height, string name, string symbol = "")
        : base(new Rectangle(width, height), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcRectangle result)
    {
        try
        {
            result = s.FromJson<CalcRectangle>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcRectangle Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcRectangle>();
    }
}
