using System.Collections.Generic;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Core;

[CalculationMetadata("Rectangular profile")]
public class RectangularProfileCalculation : ICalculation
{
    public string ReferenceName { get; set; }
    public string CalculationName { get; set; }
    public CalcStatus Status { get; }

    [InputCalcValue] public CalcLength Width { get; set; }

    [InputCalcValue] public CalcLength Height { get; set; }

    [OutputCalcValue] public CalcRectangularProfile Profile { get; set; }

    public RectangularProfileCalculation()
    {
        Width = new CalcLength(new Length(500, LengthUnit.Millimeter), "Width");
        Height = new CalcLength(new Length(800, LengthUnit.Millimeter), "Height");
        Profile = new CalcRectangularProfile(Width, Height);
    }

    public void Calculate()
    {
        Profile = CreateProfile();
    }

    public IList<IFormula> GetFormulae()
    {
        return new List<IFormula>(); // to-do
    }

    private CalcRectangularProfile CreateProfile() => new CalcRectangularProfile(Width, Height);
}
