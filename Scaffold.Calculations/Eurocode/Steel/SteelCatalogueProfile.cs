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
using MagmaWorks.Geometry;

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


        //testing interactive graphic
        [InputCalcValue]
        public CalcDoubleMultiArray Points { get; set; } = new CalcDoubleMultiArray(new List<double[]>() { new double[2] { 50, 50 } }, "Points");
        [InputCalcValue]
        public CalcDoubleMultiArray Points2 { get; set; } = new CalcDoubleMultiArray(new List<double[]>() { new double[2] { 100, 50 } }, "Points2");


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
                new Formula("", "", "", "X1 = " + Points.Value[0][0]),
                new Formula("", "", "", "Y1 = " + Points.Value[0][1]),
                new Formula("", "", "", "X2 = " + Points2.Value[0][0]),
                new Formula("", "", "", "Y2 = " + Points2.Value[0][1]),
                new Formula("", "Distance apart", "", Math.Sqrt(Math.Pow((Points2.Value[0][0] - Points.Value[0][0]),2) + Math.Pow((Points2.Value[0][1] - Points.Value[0][1]),2)).ToString())
            };
        }

        public void Calculate() { }

        public IList<ICalcGraphic> GetGraphic()
        {
            var returnValue = new List<ICalcGraphic>();
            returnValue.Add(new CalcGraphic(
                new List<MagmaWorks.Geometry.IGeometryBase>()
                {
                    new Line2d(new Point2d(25, 25, UnitsNet.Units.LengthUnit.Millimeter), new Point2d(100, 25, UnitsNet.Units.LengthUnit.Millimeter)),
                    new Line2d(
                        new Point2d(Points.Value[0][0], Points.Value[0][1], UnitsNet.Units.LengthUnit.Millimeter),
                        new Point2d(Points2.Value[0][0], Points2.Value[0][1], UnitsNet.Units.LengthUnit.Millimeter))
                },
                new List<CalcDoubleMultiArray>()
                {
                    Points,
                    Points2
                })
            );

            return returnValue;
        }
    }
}
