using System.Collections.Generic;
using MagmaWorks.Taxonomy.Profiles;
using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core;

[CalculationMetadata("Rectangular profile")]
public class RectangularProfile : ICalculation
{
    public string ReferenceName { get; set; }
    public string CalculationName { get; set; }
    public CalcStatus Status { get; }

    [InputCalcValue] public CalcLength Width { get; set; }

    [InputCalcValue] public CalcLength Height { get; set; }

    [OutputCalcValue] public CalcProfile Profile { get; set; }

    public RectangularProfile()
    {
        Width = new CalcLength(new Length(500, LengthUnit.Millimeter), "Width");
        Height = new CalcLength(new Length(800, LengthUnit.Millimeter), "Height");
        Profile = new CalcProfile(CreateProfile());
    }

    public void Calculate()
    {
        Profile.Description = CreateProfile();
    }

    public IList<IFormula> GetFormulae()
    {
        return new List<IFormula>(); // to-do
    }

    private string CreateProfile() => new Rectangle(Width, Height).Description;
}
