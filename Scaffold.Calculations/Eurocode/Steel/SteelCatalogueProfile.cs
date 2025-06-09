using System.Collections.Generic;
using MagmaWorks.Taxonomy.Profiles;
using Scaffold.Calculations.CalculationUtility;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Calculations.Eurocode.Steel
{
    public class SteelCatalogueProfile : ICalculation
    {
        public string ReferenceName { get; set; }
        public string CalculationName { get; set; } = "Steel Catalogue Profile";
        public CalcStatus Status { get; set; } = CalcStatus.None;

        [InputCalcValue("Cat", "Catalogue")]
        public CalcSelectionList CatalogueType { get; set; }
            = new CalcSelectionList("Catalogue", "UB", CatalogueProfileSelectionList.Catalogues);

        [InputCalcValue]
        public CalcSelectionList ProfileType => new CalcSelectionList("Profile", null,
            CatalogueProfileSelectionList.GetCatalogueProfiles(CatalogueType.GetEnum<CatalogueType>()));

        [OutputCalcValue]
        public CalcObjectWrapper<IEuropeanCatalogue> Profile =>
            new((IEuropeanCatalogue)CatalogueFactory.CreateEuropean(ProfileType.GetEnum<European>()),
                "Catalogue Profile", ProfileType.Value);

        public SteelCatalogueProfile() { }

        public SteelCatalogueProfile(European profile)
        {
            ProfileType.Value = profile.ToString();
        }

        public IList<IFormula> GetFormulae()
        {

            return new List<IFormula>();
        }

        public void Calculate() { }
    }
}
