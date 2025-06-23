using MagmaWorks.Taxonomy.Sections;
using Scaffold.Calculations.CalculationUtility;
using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects.Sections.Reinforcement;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using UnitsNet.Units;

namespace Scaffold.Calculations.Sections.Concrete.Reinforcement;
public class CreateFaceReinforcementLayerBySpacing : CalculationObjectInput<CalcFaceReinforcementLayer>
{
    [InputCalcValue("Face", "Section Face")]
    public CalcSelectionList SectionFace { get; set; }
            = new CalcSelectionList("Section Face", "Bottom", EnumSelectionListParser.SectionFaces);

    [InputCalcValue("Bar", "Rebar")]
    public CalcRebar Rebar { get; set; } = new CreateRebar();

    [InputCalcValue("a", "Min. rebar spacing")]
    public CalcLength Spacing { get; set; } = new CalcLength(200, LengthUnit.Millimeter, "Spacing", "a");

    public CreateFaceReinforcementLayerBySpacing() { }

    protected override CalcFaceReinforcementLayer GetOutput()
    {
        SectionFace face = SectionFace.GetEnum<SectionFace>();
        return new CalcFaceReinforcementLayer(face, Rebar, Spacing, ReferenceName ?? string.Empty);
    }
}
