using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core.Abstract;
using Scaffold.Core.Static;

namespace Scaffold.Core.CalcQuantities;

public sealed class CalcLength : CalcQuantity<Length>
{
    public CalcLength(Length length, string name, string symbol = "")
        : base(length, name, symbol) { }

    public CalcLength(double value, LengthUnit unit, string name, string symbol)
        : base(new Length(value, unit), name, symbol) { }

    public static CalcLength OneMeter => new CalcLength(Length.FromMeters(1), "Unit Length");

    public static CalcLength operator +(CalcLength x, CalcLength y)
    {
        (string name, string symbol, LengthUnit unit) = OperatorMetadataHelper<LengthUnit>(x, y, '+');
        return new CalcLength(new Length(x.Quantity.As(unit) + y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcLength operator -(CalcLength x, CalcLength y)
    {
        (string name, string symbol, LengthUnit unit) = OperatorMetadataHelper<LengthUnit>(x, y, '-');
        return new CalcLength(new Length(x.Quantity.As(unit) - y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcArea operator *(CalcLength x, CalcLength y)
    {
        (string name, string symbol, LengthUnit unit) = OperatorMetadataHelper<LengthUnit>(x, y, '·');
        return new CalcArea(new Area(x.Quantity.As(unit) * y.Quantity.As(unit),
            unit.GetEquivilantAreaUnit()), name, symbol);
    }

    public static CalcVolume operator *(CalcLength x, CalcArea y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}\u2009·\u2009{y.DisplayName}";
        LengthUnit unit = (LengthUnit)x.Quantity.Unit;
        return new CalcVolume(new Volume(x.Quantity.As(unit) * y.Quantity.As(unit.GetEquivilantAreaUnit()),
            unit.GetEquivilantVolumeUnit()), name, "");
    }

    public static CalcInertia operator *(CalcLength x, CalcVolume y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}\u2009·\u2009{y.DisplayName}";
        LengthUnit unit = (LengthUnit)x.Quantity.Unit;
        return new CalcInertia(new AreaMomentOfInertia(
            x.Quantity.As(unit) * y.Quantity.As(unit.GetEquivilantVolumeUnit()),
            unit.GetEquivilantInertiaUnit()), name, "");
    }

    public static CalcStrain operator /(CalcLength x, CalcLength y)
    {
        (string name, string _, LengthUnit unit) = OperatorMetadataHelper<LengthUnit>(x, y, '/');
        return new CalcStrain((Length)x.Quantity / (Length)y.Quantity, StrainUnit.Ratio, name, string.Empty);
    }


    public static CalcLength operator *(CalcStrain x, CalcLength y)
    {
        return new CalcLength(x.Quantity.As(StrainUnit.Ratio) * (Length)y.Quantity, string.Empty, string.Empty);
    }

    public static CalcLength operator *(CalcLength x, CalcStrain y)
    {
        return y * x;
    }

    public static CalcLength operator +(CalcLength x, double y)
    {
        return new CalcLength(x.Value + y, (LengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcLength operator +(double x, CalcLength y)
    {
        return y + x;
    }

    public static CalcLength operator -(CalcLength x, double y)
    {
        return new CalcLength(x.Value - y, (LengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcLength operator *(CalcLength x, double y)
    {
        return new CalcLength(x.Value * y, (LengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcLength operator *(double x, CalcLength y)
    {
        return y * x;
    }

    public static CalcLength operator /(CalcLength x, double y)
    {
        return new CalcLength(x.Value / y, (LengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }
}
