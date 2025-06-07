using System;
using System.Collections.Generic;
using MagmaWorks.Taxonomy.Materials;
using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using MagmaWorks.Taxonomy.Standards.Eurocode;
using Scaffold.Calculations.CalculationUtility;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects.Materials.StandardMaterials.En;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Calculations.Eurocode.Steel
{
    public class SteelCatalogueProfile : ICalculation
    {
        public string ReferenceName { get; set; }
        public string CalculationName { get; set; } = "Steel Catalogue Profile";
        public CalcStatus Status { get; set; } = CalcStatus.None;

        [InputCalcValue("Cat", "Catalogue")]
        public CalcSelectionList CatalogueType { get; set; }
            = new CalcSelectionList("Catalogue", "HEB", CatalogueProfileSelectionList.Catalogues);

        [InputCalcValue]
        public CalcLength Thickness { get; set; }
            = new CalcLength(40, LengthUnit.Millimeter, "Nominal thickness of the element", "t");

        [OutputCalcValue]
        public CalcEnSteelMaterial Material =>
            new CalcEnSteelMaterial(SteelGrade.GetEnum<EnSteelGrade>(),
                                    NationalAnnex.RecommendedValues, "Steel", "S");


        public SteelMaterialProperties()
        {
            Calculate();
        }

        public SteelMaterialProperties(EnSteelGrade grade)
        {
            SteelGrade.Value = grade.ToString();
            Calculate();
        }

        public IList<IFormula> GetFormulae()
        {
            return new List<IFormula>();
        }

        public void Calculate() { }
    }
}
