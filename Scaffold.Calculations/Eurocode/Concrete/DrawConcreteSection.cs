using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using MagmaWorks.Geometry;
using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using MagmaWorks.Taxonomy.Sections.Reinforcement;
using MagmaWorks.Taxonomy.Standards.Eurocode;
using Scaffold.Calculations.CalculationUtility;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects.Materials.StandardMaterials.En;
using Scaffold.Core.CalcObjects.Profiles;
using Scaffold.Core.CalcObjects.Sections;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Images.Drawing;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;
using SkiaSharp;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Calculations.Eurocode.Steel;
public class DrawConcreteSection : ICalculation
{
    public string ReferenceName { get; set; }
    public string CalculationName { get; set; } = "Concrete Section Drawing";
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [InputCalcValue("Grd", "Grade")]
    public CalcSelectionList ConcreteGrade { get; set; }
            = new CalcSelectionList("Concrete Grade", "C30/37", EnumSelectionListParser.ConcreteGrades);

    [InputCalcValue] // todo - make another calc act as input
    public ICalcProfile Profile { get; set; }
        = new CalcRectangularProfile(new Length(500, LengthUnit.Millimeter), new Length(1000, LengthUnit.Millimeter), "500 x 1000mm");

    public IList<IFormula> GetFormulae()
    {
        var material = new CalcEnConcreteMaterial(ConcreteGrade.GetEnum<EnConcreteGrade>("/", "_"),
                                       NationalAnnex.RecommendedValues, "Concrete", "C");
        IEnRebarMaterial rebarMaterial = new EnRebarMaterial(EnRebarGrade.B500B, NationalAnnex.Germany);
        Length linkBar = new Length(20, LengthUnit.Millimeter);
        var link = new Rebar(rebarMaterial, linkBar);
        Length diameter = new Length(20, LengthUnit.Millimeter);
        var rebar = new Rebar(rebarMaterial, diameter);
        var rebars = new List<ILongitudinalReinforcement>()
        {
            new LongitudinalReinforcement(rebar, new LocalPoint2d(22, 47, LengthUnit.Millimeter)),
            new LongitudinalReinforcement(rebar, new LocalPoint2d(-22, 47, LengthUnit.Millimeter)),
            new LongitudinalReinforcement(rebar, new LocalPoint2d(-22, -47, LengthUnit.Millimeter)),
            new LongitudinalReinforcement(rebar, new LocalPoint2d(22, -47, LengthUnit.Millimeter)),
        };
        var section = new CalcConcreteSection(Profile, material, link, new Length(8, LengthUnit.Millimeter), rebars, "Concrete Section");
        ICalcImage image = Sections.DrawSection(section);
        var formula = new Formula();
        formula.SetImage(image);
        DrawingUtiliy.Save(image, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ConcreteImage"),
            SKEncodedImageFormat.Png);
        return new List<IFormula>() { formula };
    }

    public DrawConcreteSection()
    {
        Calculate();
    }

    public void Calculate() { }
}
