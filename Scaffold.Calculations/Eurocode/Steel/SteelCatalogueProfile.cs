using System.Collections.Generic;
using MagmaWorks.Taxonomy.Profiles;
using Scaffold.Calculations.CalculationUtility;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.Models;
using System;

namespace Scaffold.Calculations.Eurocode.Steel
{
    public class SteelCatalogueProfile : ICalculation
    {
        public string ReferenceName { get; set; }
        public string CalculationName { get; set; } = "Steel Catalogue Profile";
        public CalcStatus Status { get; set; } = CalcStatus.None;

        [InputCalcValue("Cat", "Catalogue")]
        public CalcSelectionList CatalogueType
        {
            get
            {
                return _catalogueType ??= new CalcSelectionList("Catalogue", "UB", CatalogueProfileSelectionList.Catalogues);
            }

            set
            {
                _profileType = null;
                _catalogueType = value;
            }
        }

        [InputCalcValue]
        public CalcSelectionList ProfileType
        {
            get
            {
                return _profileType ??= new CalcSelectionList("Profile", null,
                    CatalogueProfileSelectionList.GetCatalogueProfiles(CatalogueType.GetEnum<CatalogueType>()));
            }

            set
            {
                _profileType = value;
            }
        }


        // for testing interactive image
        [InputCalcValue]
        public CalcLength X1 { get; set; } = new CalcLength(50, UnitsNet.Units.LengthUnit.Millimeter, "X coordinate 1", "X1");
        [InputCalcValue]
        public CalcLength Y1 { get; set; } = new CalcLength(100, UnitsNet.Units.LengthUnit.Millimeter, "Y coordinate 1", "Y1");
        [InputCalcValue]
        public CalcLength X2 { get; set; } = new CalcLength(75, UnitsNet.Units.LengthUnit.Millimeter, "X coordinate 2", "X2");
        [InputCalcValue]
        public CalcLength Y2 { get; set; } = new CalcLength(200, UnitsNet.Units.LengthUnit.Millimeter, "Y coordinate 2", "Y2");


        [OutputCalcValue]
        public CalcObjectWrapper<IEuropeanCatalogue> Profile =>
            new((IEuropeanCatalogue)CatalogueFactory.CreateEuropean(ProfileType.GetEnum<European>()),
                "Catalogue Profile", ProfileType.Value);

        private CalcSelectionList _catalogueType;
        private CalcSelectionList _profileType;

        public SteelCatalogueProfile() { }

        public SteelCatalogueProfile(European profile)
        {
            ProfileType.Value = profile.ToString();
        }

        public IList<IFormula> GetFormulae()
        {
            return new List<IFormula>() {
                new Formula("", "", "", "X1 = " + X1.Value),
                new Formula("", "", "", "Y1 = " + Y1.Value),
                new Formula("", "", "", "X2 = " + X2.Value),
                new Formula("", "", "", "Y2 = " + Y2.Value),
                new Formula("", "Distance apart", "", Math.Sqrt(Math.Pow((X2.Value - X1.Value),2) + Math.Pow((Y2.Value-Y1.Value),2)).ToString())
            };
        }

        public void Calculate() { }
    }
}
