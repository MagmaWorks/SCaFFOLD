using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using MagmaWorks.Taxonomy.Profiles;
using Scaffold.Calculations.CalculationUtility;
using Scaffold.Core.CalcQuantities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Scaffold.Calculations.Eurocode.Steel;

public class SteelCatalogueProfileTests
{
    private static CalculationReader Reader { get; } = new();

    [Fact]
    public void CalculationBaseSetupTest()
    {
        // Assemble
        var calc = new SteelCatalogueProfile();

        // Act
        CalculationMetadata metadata = Reader.GetMetadata(calc);
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        // Assert
        Assert.Equal("Steel Catalogue Profile", calc.ReferenceName);
        Assert.Equal("Steel Catalogue Profile", calc.CalculationName);
        Assert.Equal(2, inputs.Count);
        Assert.Equal(1, outputs.Count);
    }

    [Theory]
    [InlineData(0, typeof(CalcSelectionList), "Cat", "Catalogue")]
    [InlineData(1, typeof(CalcSelectionList), "", "Profile")]
    public void CalculationInputTests(int id, Type expectedType, string expectedSymbol, string expectedDisplayName)
    {
        // Assemble
        var calc = new SteelCatalogueProfile();

        // Act
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);

        // Assert
        Assert.Equal(expectedType, inputs[id].GetType());
        Assert.Equal(expectedSymbol, inputs[id].Symbol);
        Assert.Equal(expectedDisplayName, inputs[id].DisplayName);
    }

    [Fact]
    public void ChangeCatalogueTest()
    {
        // Assemble
        var calc = new SteelCatalogueProfile();

        // Act
        calc.CatalogueType.Value = "HEB";

        // Assert
        Assert.Equal("HE100B", calc.ProfileType.Value);
    }

    [Fact]
    public void ChangeCatalogueAndProfileTest()
    {
        // Assemble
        var calc = new SteelCatalogueProfile();

        // Act
        calc.CatalogueType.Value = "HEB";
        calc.ProfileType.Value = "HE200B";

        // Assert
        Assert.IsType<HE200B>(calc.Profile.Value);
    }

    [Theory]
    [MemberData(nameof(CatalogueValues))]
    public void SteelCatalogueTests(string catalogue)
    {
        // Assemble
        var calc = new SteelCatalogueProfile();

        // Act
        calc.CatalogueType.Value = catalogue;

        // Assert
        Assert.Equal(catalogue, calc.CatalogueType.Value);
        Assert.True(calc.ProfileType.SelectionList.Count > 10);
        Assert.NotNull(calc.Profile);

        // select each profile in catalogue type
        foreach (string prfl in calc.ProfileType.SelectionList)
        {
            calc.ProfileType.Value = prfl;
            Assert.NotNull(calc.Profile);
            Assert.Equal(prfl, calc.ProfileType.Value);
        }
    }

    public static IEnumerable<object[]> CatalogueValues()
    {
        foreach (var catalogue in Enum.GetValues(typeof(CatalogueType)))
        {
            yield return new object[] { catalogue.ToString() };
        }
    }
}
