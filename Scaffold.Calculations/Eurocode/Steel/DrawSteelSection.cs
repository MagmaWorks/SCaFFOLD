using System.Collections.Generic;
using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using MagmaWorks.Taxonomy.Standards.Eurocode;
using Scaffold.Calculations.CalculationUtility;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects.Materials.StandardMaterials.En;
using Scaffold.Core.CalcObjects.Sections;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Images.Models;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;

namespace Scaffold.Calculations.Eurocode.Steel;
public class DrawSteelSection : ICalculation
{
    public string ReferenceName { get; set; }
    public string CalculationName { get; set; } = "Steel Section Drawing";
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [InputCalcValue("Grd", "Grade")]
    public CalcSelectionList SteelGrade { get; set; }
            = new CalcSelectionList("Steel Grade", "S355", EnumSelectionListParser.SteelGrades);

    [InputCalcValue] // todo - make another calc act as input
    public SteelCatalogueProfile Profile { get; set; } = new SteelCatalogueProfile();

    [OutputCalcValue("Svg", "Svg image string")]
    public CalcString Svg
    {
        get
        {
            var material = new CalcEnSteelMaterial(SteelGrade.GetEnum<EnSteelGrade>(),
                        NationalAnnex.RecommendedValues, "Steel", "S");
            CalcSection section = new CalcSection(Profile.Profile.Value, material, "Steel Section");
            return Core.Images.Drawing.Sections.DrawSection(section);
        }
    }

    public IList<IFormula> GetFormulae()
    {
        var image = new ImageFromSkSvg(Svg);
        var formula = new Formula();
        formula.SetImage(image);
        return new List<IFormula>()
        {
            formula
        };
    }

    public DrawSteelSection()
    {
        Calculate();
    }

    public void Calculate() { }
}
