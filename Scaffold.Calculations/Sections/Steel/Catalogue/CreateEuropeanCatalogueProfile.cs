using MagmaWorks.Taxonomy.Profiles;
using Scaffold.Calculations.CalculationUtility;
using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects;

namespace Scaffold.Calculations.Sections.Steel.Catalogue;
public class CreateEuropeanCatalogueProfile : CalcObjectInput<CalcObjectWrapper<IEuropeanCatalogue>>
{
    [InputCalcValue("Cat", "Catalogue")]
    // this is a CalcSelectionsList (ProfileType) but needs to be passed as a type so maintain access
    // to the underlying CalcSelectionList (CatalogueType)
    public CreateEuropeanCatalogues ProfileType { get; set; } = new CreateEuropeanCatalogues();

    public CreateEuropeanCatalogueProfile() { }

    protected override CalcObjectWrapper<IEuropeanCatalogue> InitialiseOutput()
    {
        return new(CatalogueFactory.CreateEuropean(ProfileType.Output.GetEnum<European>()),
                   "Catalogue Profile", ProfileType.Output);
    }
}
