using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Sections.Reinforcement;
using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core;

[CalculationMetadata("Concrete section")]
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
    [OutputCalcValue] public CalcConcreteSection Section { get; set; }

    private List<string> _barDiameters = Enum.GetValues(typeof(BarDiameter)).Cast<BarDiameter>()
        .Select(v => v.ToString().Replace("D", string.Empty)).ToList();
    private List<string> _concreteGrades = Enum.GetValues(typeof(EnConcreteGrade)).Cast<EnConcreteGrade>()
        .Select(v => v.ToString().Replace("_", "/")).ToList();
    private List<string> _rebarGrades = Enum.GetValues(typeof(EnRebarGrade)).Cast<EnRebarGrade>()
        .Select(v => v.ToString()).ToList();

    public ConcreteSectionCalculation()
    {
        Width = new CalcLength(new Length(500, LengthUnit.Millimeter), "Width");
        Height = new CalcLength(new Length(800, LengthUnit.Millimeter), "Height");
        ConcreteGrade = new CalcSelectionList("Concrete Grade", "C30/37", _concreteGrades);
        RebarGrade = new CalcSelectionList("Rebar Grade", "B500B", _concreteGrades);

        Cover = new CalcLength(new Length(30, LengthUnit.Millimeter), "Cover");
        LinkDiameter = new CalcSelectionList("Link Ø", "10", _barDiameters);

        TopRebarCount = new CalcInt(2, "Nº Bars Top");
        TopRebarDiameter = new CalcSelectionList("Top Bar Ø", "12", _barDiameters);

        BottomRebarCount = new CalcInt(3, "Nº Bars Bottom");
        BottomRebarDiameter = new CalcSelectionList("Bottom Bar Ø", "20", _barDiameters);

        SideRebarCount = new CalcInt(1, "Nº Bars per Side");
        SideRebarDiameter = new CalcSelectionList("Side Bar Ø", "8", _barDiameters);

        Section = CreateSection();
    }

    public void Calculate()
    {
        Profile = CreateProfile();
    }

    public IList<IFormula> GetFormulae()
    {
        return new List<IFormula>(); // to-do
    }

    private CalcConcreteSection CreateSection()
    {
        var profile = new Rectangle(Width, Height);
        Enum.TryParse(ConcreteGrade.Value, out EnConcreteGrade myStatus);
        var section = new CalcConcreteSection(profile, )
    }
}
