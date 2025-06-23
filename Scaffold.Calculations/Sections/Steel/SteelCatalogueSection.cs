using System;
using System.Collections.Generic;
using System.IO;
using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Standards.Eurocode;
using Scaffold.Calculations.CalculationUtility;
using Scaffold.Calculations.Sections.Steel.Catalogue;
using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects;
using Scaffold.Core.CalcObjects.Materials.StandardMaterials.En;
using Scaffold.Core.CalcObjects.Sections;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Images.Drawing;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;
using SkiaSharp;

namespace Scaffold.Calculations.Eurocode.Steel;
public class SteelCatalogueSection : CalculationObjectInput<CalcSection>
{

    [InputCalcValue("Grd", "Steel Grade")]
    public CalcSelectionList SteelGrade { get; set; }
        = new CalcSelectionList("Catalogue", "UB", EnumSelectionListParser.SteelGrades);

    [InputCalcValue]
    public CalcObjectWrapper<IEuropeanCatalogue> Profile { get; set; } = new CreateEuropeanCatalogueProfile();

    public override IList<IFormula> GetFormulae()
    {
        ICalcImage image = Core.Images.Drawing.Sections.DrawSection(Output);
        var formula = new Formula();
        formula.SetImage(image);
        DrawingUtiliy.Save(image, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SteelImage"),
            SKEncodedImageFormat.Png);
        return new List<IFormula>() { formula };
    }

    public SteelCatalogueSection() { }

    protected override CalcSection GetOutput()
    {
        var material = new CalcEnSteelMaterial(SteelGrade.GetEnum<EnSteelGrade>(),
                        NationalAnnex.RecommendedValues, "Steel", "S");
        return new CalcSection(Profile.Value, material, "Steel Section");
    }
}
