using MagmaWorks.Geometry;
using MagmaWorks.Taxonomy.Materials;
using MagmaWorks.Taxonomy.Sections.Reinforcement;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Sections.Reinforcement;
public sealed class CalcLongitudinalReinforcement : CalcTaxonomyObject<LongitudinalReinforcement>
#if NET7_0_OR_GREATER
    , IParsable<CalcLongitudinalReinforcement>
#endif
{
    public CalcLongitudinalReinforcement(LongitudinalReinforcement longitudinalreinforcement, string name, string symbol = "")
        : base(longitudinalreinforcement, name, symbol) { }

    public CalcLongitudinalReinforcement(IRebar rebar, ILocalPoint2d position, string name, string symbol = "")
        : base(new LongitudinalReinforcement(rebar, position), name, symbol) { }

    public CalcLongitudinalReinforcement(IMaterial material, Length diameter, ILocalPoint2d position, string name, string symbol = "")
        : base(new LongitudinalReinforcement(material, diameter, position), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcLongitudinalReinforcement result)
    {
        try
        {
            result = s.FromJson<CalcLongitudinalReinforcement>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcLongitudinalReinforcement Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcLongitudinalReinforcement>();
    }
}
