using MagmaWorks.Taxonomy.Profiles;
using Scaffold.Calculations.CalculationUtility;
using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects;
using Scaffold.Core.CalcValues;

namespace Scaffold.Calculations.Sections.Steel.Catalogue;
public class CreateEuropeanCatalogueProfile : CalculationObjectInput<CalcObjectWrapper<IEuropeanCatalogue>>
{
    [InputCalcValue("Cat", "Catalogue")]
    public CalcSelectionList ProfileType { get; set; } = new CreateEuropeanProfileTypes();

    public CreateEuropeanCatalogueProfile() { }

    protected override CalcObjectWrapper<IEuropeanCatalogue> GetOutput()
    {
        return new(CatalogueFactory.CreateEuropean(ProfileType.GetEnum<European>()),
                   "Catalogue Profile", ProfileType.Value);
    }
}
