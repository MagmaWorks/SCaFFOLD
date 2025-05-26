using MagmaWorks.Taxonomy.Sections;
using MagmaWorks.Taxonomy.Sections.Reinforcement;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Sections.Reinforcement;
public sealed class CalcFaceReinforcementLayer : CalcTaxonomyObject<FaceReinforcementLayer>
#if NET7_0_OR_GREATER
    , IParsable<CalcFaceReinforcementLayer>
#endif
{
    public CalcFaceReinforcementLayer(FaceReinforcementLayer facereinforcementlayer, string name, string symbol = "")
        : base(facereinforcementlayer, name, symbol) { }

    public CalcFaceReinforcementLayer(SectionFace face, IRebar rebar, int numberOfRebars, string name, string symbol = "")
        : base(new FaceReinforcementLayer(face, rebar, numberOfRebars), name, symbol) { }

    public CalcFaceReinforcementLayer(SectionFace face, IRebar rebar, Length maxSpacing, string name, string symbol = "")
        : base(new FaceReinforcementLayer(face, rebar, maxSpacing), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcFaceReinforcementLayer result)
    {
        try
        {
            result = s.FromJson<CalcFaceReinforcementLayer>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcFaceReinforcementLayer Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcFaceReinforcementLayer>();
    }
}
