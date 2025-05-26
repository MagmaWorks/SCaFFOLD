using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcEllipse : CalcTaxonomyObject<Ellipse>
#if NET7_0_OR_GREATER
    , IParsable<CalcEllipse>
#endif
{
    public CalcEllipse(Ellipse ellipse, string name, string symbol = "")
        : base(ellipse, name, symbol) { }

    public CalcEllipse(Length width, Length height, string name, string symbol = "")
        : base(new Ellipse(width, height), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcEllipse result)
    {
        try
        {
            result = s.FromJson<CalcEllipse>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcEllipse Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcEllipse>();
    }
}
