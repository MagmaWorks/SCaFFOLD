using System;
using System.Collections.Generic;
using System.IO;
using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using MagmaWorks.Taxonomy.Standards.Eurocode;
using Scaffold.Calculations.CalculationUtility;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects.Materials.StandardMaterials.En;
using Scaffold.Core.CalcObjects.Sections;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Images.Drawing;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;
using SkiaSharp;

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

    public IList<IFormula> GetFormulae()
    {
        var material = new CalcEnSteelMaterial(SteelGrade.GetEnum<EnSteelGrade>(),
                        NationalAnnex.RecommendedValues, "Steel", "S");
        CalcSection section = new CalcSection(Profile.Profile.Value, material, "Steel Section");
        ICalcImage image = Sections.DrawSection(section);
        var formula = new Formula();
        formula.SetImage(image);
        DrawingUtiliy.Save(image, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SteelImage"),
            SKEncodedImageFormat.Png);
        return new List<IFormula>() { formula };
    }

    public DrawSteelSection()
    {
        Calculate();
    }

    public void Calculate() { }
}
