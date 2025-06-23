using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using MagmaWorks.Taxonomy.Sections.Reinforcement;
using MagmaWorks.Taxonomy.Standards.Eurocode;
using Scaffold.Calculations.CalculationUtility;
using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects.Sections.Reinforcement;
using Scaffold.Core.CalcValues;
using UnitsNet;

namespace Scaffold.Calculations.Sections.Concrete.Reinforcement;
public class CreateRebar : CalculationObjectInput<CalcRebar>
{

    [InputCalcValue("Grd", "Grade")]
    public CalcSelectionList RebarGrade { get; set; }
            = new CalcSelectionList("Rebar Grade", "B500B", EnumSelectionListParser.RebarGrades);

    [InputCalcValue]
    public CalcSelectionList BarDiameter { get; set; }
            = new CalcSelectionList("Rebar Grade", "12", EnumSelectionListParser.BarDiameters);

    protected override CalcRebar GetOutput()
    {
        EnRebarGrade grade = RebarGrade.GetEnum<EnRebarGrade>(string.Empty, string.Empty, "D");
        var material = new EnRebarMaterial(grade, NationalAnnex.UnitedKingdom);
        Length diameter = Length.Parse($"{BarDiameter.Value}mm");
        return new CalcRebar(material, diameter, ReferenceName ?? string.Empty);
    }

    public CreateRebar() { }

    public CreateRebar(BarDiameter diameter)
    {
        BarDiameter.Value = diameter.ToString().Replace("D", string.Empty);
    }
}
