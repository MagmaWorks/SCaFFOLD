﻿using System.Collections.Generic;
using MagmaWorks.Taxonomy.Profiles;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects.Profiles;
using Scaffold.Core.CalcObjects.Sections;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Calculations;
public class SectionPropertiesCalculation : ICalculation
{
    public string ReferenceName { get; set; }
    public string CalculationName { get; set; } = "Section Properties";
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [InputCalcValue]
    public IProfile Profile { get; set; }
        = new CalcRectangleProfile(new Length(400, LengthUnit.Millimeter), new Length(800, LengthUnit.Millimeter), "400 x 800mm");

    [OutputCalcValue("Section Properties", "SP")]
    public CalcSectionProperties Properties => new CalcSectionProperties(Profile, ReferenceName);

    [OutputCalcValue]
    public CalcLength CentroidY => Properties.Centroid.Y;

    [OutputCalcValue]
    public CalcLength CentroidZ => Properties.Centroid.Z;

    [OutputCalcValue]
    public CalcLength Perimeter => Properties.Perimeter;

    [OutputCalcValue]
    public CalcArea Area => Properties.Area;

    [OutputCalcValue]
    public CalcVolume ElasticSectionModulusYy => Properties.ElasticSectionModulusYy;

    [OutputCalcValue]
    public CalcVolume ElasticSectionModulusZz => Properties.ElasticSectionModulusZz;

    [OutputCalcValue]
    public CalcInertia MomentOfInertiaYy => Properties.MomentOfInertiaYy;

    [OutputCalcValue]
    public CalcInertia MomentOfInertiaZz => Properties.MomentOfInertiaZz;

    [OutputCalcValue]
    public CalcLength RadiusOfGyrationYy => Properties.RadiusOfGyrationYy;

    [OutputCalcValue]
    public CalcLength RadiusOfGyrationZz => Properties.RadiusOfGyrationZz;

    public List<IFormula> Expressions = new List<IFormula>();
    public IList<IFormula> GetFormulae() => Expressions;

    public SectionPropertiesCalculation()
    {
        Calculate();
    }

    public void Calculate() { }
}
