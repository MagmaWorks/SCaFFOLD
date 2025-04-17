using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core.Abstract;
using Scaffold.Core.CalcValues;

namespace Scaffold.Core.CalcQuantities;

public sealed class CalcStrain : CalcQuantity<Strain>
{
    public CalcStrain(Strain strain, string name, string symbol = "")
        : base(strain, name, symbol) { }

    public CalcStrain(double value, StrainUnit unit, string name, string symbol)
        : base(new Strain(value, unit), name, symbol) { }

    public static CalcStrain operator +(CalcStrain x, CalcStrain y)
    {
        (string name, string symbol, StrainUnit unit) = OperatorMetadataHelper<StrainUnit>(x, y, '+');
        return new CalcStrain(new Strain(x.Quantity.As(unit) + y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcStrain operator -(CalcStrain x, CalcStrain y)
    {
        (string name, string symbol, StrainUnit unit) = OperatorMetadataHelper<StrainUnit>(x, y, '-');
        return new CalcStrain(new Strain(x.Quantity.As(unit) - y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcDouble operator /(CalcStrain x, CalcStrain y)
    {
        (string name, string _, StrainUnit unit) = OperatorMetadataHelper<StrainUnit>(x, y, '/');
        return new CalcDouble((Strain)x.Quantity / (Strain)y.Quantity, name, string.Empty);
    }

    public static CalcStrain operator +(CalcStrain x, double y)
    {
        return new CalcStrain(x.Value + y, (StrainUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcStrain operator +(double x, CalcStrain y)
    {
        return y + x;
    }

    public static CalcStrain operator -(CalcStrain x, double y)
    {
        return new CalcStrain(x.Value - y, (StrainUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcStrain operator *(CalcStrain x, double y)
    {
        return new CalcStrain(x.Value * y, (StrainUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcStrain operator *(double x, CalcStrain y)
    {
        return y * x;
    }

    public static CalcStrain operator /(CalcStrain x, double y)
    {
        return new CalcStrain(x.Value / y, (StrainUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }
}
