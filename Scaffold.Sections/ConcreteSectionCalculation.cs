using System.Collections.Generic;
using System.IO;
using System.Text;
using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Sections;
using MagmaWorks.Taxonomy.Sections.Reinforcement;
using MagmaWorks.Taxonomy.Standards.Eurocode;
using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Images.Models;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;
using Scaffold.Core.Utility;

namespace Scaffold.Core;

[CalculationMetadata("Rectangular Concrete section")]
public class ConcreteSectionCalculation : ICalculation
{
    public string ReferenceName { get; set; }
    public string CalculationName { get; set; }
    public CalcStatus Status { get; }

    [InputCalcValue] public CalcLength Width { get; set; }
    [InputCalcValue] public CalcLength Height { get; set; }
    [InputCalcValue] public CalcSelectionList ConcreteGrade { get; set; }
    [InputCalcValue] public CalcSelectionList RebarGrade { get; set; }
    [InputCalcValue] public CalcLength Cover { get; set; }
    [InputCalcValue] public CalcSelectionList LinkDiameter { get; set; }
    [InputCalcValue] public CalcInt TopRebarCount { get; set; }
    [InputCalcValue] public CalcSelectionList TopRebarDiameter { get; set; }
    [InputCalcValue] public CalcInt BottomRebarCount { get; set; }
    [InputCalcValue] public CalcSelectionList BottomRebarDiameter { get; set; }
    [InputCalcValue] public CalcInt SideRebarCount { get; set; }
    [InputCalcValue] public CalcSelectionList SideRebarDiameter { get; set; }
    [OutputCalcValue] public CalcConcreteSection Section { get; private set; }

    public ConcreteSectionCalculation()
    {
        Width = new CalcLength(new Length(500, LengthUnit.Millimeter), "Width");
        Height = new CalcLength(new Length(800, LengthUnit.Millimeter), "Height");
        ConcreteGrade = new CalcSelectionList("Concrete Grade", "C30/37", EnumToFromSelectionList.ConcreteGrades);
        RebarGrade = new CalcSelectionList("Rebar Grade", "B500B", EnumToFromSelectionList.RebarGrades);

        Cover = new CalcLength(new Length(30, LengthUnit.Millimeter), "Cover");
        LinkDiameter = new CalcSelectionList("Link Ø", "10", EnumToFromSelectionList.BarDiameters);

        TopRebarCount = new CalcInt(2, "Nº Bars Top");
        TopRebarDiameter = new CalcSelectionList("Top Bar Ø", "12", EnumToFromSelectionList.BarDiameters);

        BottomRebarCount = new CalcInt(3, "Nº Bars Bottom");
        BottomRebarDiameter = new CalcSelectionList("Bottom Bar Ø", "20", EnumToFromSelectionList.BarDiameters);

        SideRebarCount = new CalcInt(1, "Nº Bars per Side");
        SideRebarDiameter = new CalcSelectionList("Side Bar Ø", "8", EnumToFromSelectionList.BarDiameters);

        Section = CreateSection();
    }

    public void Calculate()
    {
        Section = CreateSection();
    }

    public IList<IFormula> GetFormulae()
    {
        var formula = new Formula("", "", "", "", CalcStatus.None);
        formula.SetImage(new ImageFromSkSvg(DrawSectionSvg()));
        return new List<IFormula>() { formula };
    }

    private string DrawSectionSvg()
    {
        Calculate();
        (StringBuilder sb, LengthUnit unit) = DrawSection.BeginSvg(Section);
        sb = DrawSection.AddRoundedRectangle(sb, Section, unit);
        sb = DrawSection.AddCircles(sb, Section.Rebars, unit);
        return DrawSection.EndSvg(sb);
    }

    private CalcConcreteSection CreateSection()
    {
        var profile = new Rectangle(Width, Height);
        NationalAnnex nationalAnnex = NationalAnnex.UnitedKingdom;

        EnConcreteGrade concreteGrade = EnumToFromSelectionList.GetConcreteGrade(ConcreteGrade);
        var concreteMaterial = new EnConcreteMaterial(concreteGrade, nationalAnnex);

        EnRebarGrade rebarGrade = EnumToFromSelectionList.GetRebarGrade(RebarGrade);
        var rebarMaterial = new EnRebarMaterial(rebarGrade, nationalAnnex);

        BarDiameter linkDiameter = EnumToFromSelectionList.GetBarDiameter(LinkDiameter);
        var link = new Rebar(rebarMaterial, linkDiameter);

        var section = new CalcConcreteSection(profile, concreteMaterial, link, Cover);

        if (TopRebarCount > 0)
        {
            BarDiameter topDiameter = EnumToFromSelectionList.GetBarDiameter(TopRebarDiameter);
            var topRebars = new Rebar(rebarMaterial, topDiameter);
            var topBars = new FaceReinforcementLayer(SectionFace.Top, topRebars, TopRebarCount);
            section.AddRebarLayer(topBars);
        }

        if (BottomRebarCount > 0)
        {
            BarDiameter bottomDiameter = EnumToFromSelectionList.GetBarDiameter(BottomRebarDiameter);
            var bottomRebars = new Rebar(rebarMaterial, bottomDiameter);
            var bottomBars = new FaceReinforcementLayer(SectionFace.Bottom, bottomRebars, BottomRebarCount);
            section.AddRebarLayer(bottomBars);
        }

        if (SideRebarCount > 0)
        {
            BarDiameter sideDiameter = EnumToFromSelectionList.GetBarDiameter(SideRebarDiameter);
            var sideRebars = new Rebar(rebarMaterial, sideDiameter);
            var leftBars = new FaceReinforcementLayer(SectionFace.Left, sideRebars, SideRebarCount + 2);
            var rigthBars = new FaceReinforcementLayer(SectionFace.Right, sideRebars, SideRebarCount + 2);
            section.AddRebarLayer(leftBars);
            section.AddRebarLayer(rigthBars);
        }

        return section;
    }
}
