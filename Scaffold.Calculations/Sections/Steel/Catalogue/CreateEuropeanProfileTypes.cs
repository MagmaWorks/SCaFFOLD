using Scaffold.Calculations.CalculationUtility;
using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcValues;

namespace Scaffold.Calculations.Sections.Steel.Catalogue;
public class CreateEuropeanProfileTypes : CalculationObjectInput<CalcSelectionList>
{
    [InputCalcValue("Cat", "Catalogue")]
    public CalcSelectionList CatalogueType { get; set; }
            = new CalcSelectionList("Catalogue", "UB", CatalogueProfileSelectionList.Catalogues);

    public CreateEuropeanProfileTypes() { }

    protected override CalcSelectionList GetOutput()
    {
        return new CalcSelectionList("Profile", null,
                    CatalogueProfileSelectionList.GetCatalogueProfiles(CatalogueType.GetEnum<CatalogueType>()));
    }
}
