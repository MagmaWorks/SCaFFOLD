using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core.Abstract;
using Scaffold.Core.Static;

namespace Scaffold.Core.CalcValues;

public sealed class CalcArea : CalcQuantity<Area>
{
    public CalcArea(Area Area, string name, string symbol = "")
        : base(Area, name, symbol) { }

    public CalcArea(double value, AreaUnit unit, string name, string symbol)
        : base(new Area(value, unit), name, symbol) { }

    public static CalcArea operator +(CalcArea x, CalcArea y)
    {
        (string name, string symbol, AreaUnit unit) = OperatorMetadataHelper<AreaUnit>(x, y, '+');
        return new CalcArea(new Area(x.Quantity.As(unit) + y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcArea operator -(CalcArea x, CalcArea y)
    {
        (string name, string symbol, AreaUnit unit) = OperatorMetadataHelper<AreaUnit>(x, y, '-');
        return new CalcArea(new Area(x.Quantity.As(unit) - y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcVolume operator *(CalcArea x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}·{y.DisplayName}";
        AreaUnit unit = x.Quantity.Unit;
        return new CalcVolume(new Volume(x.Quantity.As(unit) * y.Quantity.As(unit.GetEquivilantLengthUnit()),
            unit.GetEquivilantVolumeUnit()), name, "");
    }

    public static CalcInertia operator *(CalcArea x, CalcArea y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}·{y.DisplayName}";
        AreaUnit unit = x.Quantity.Unit;
        return new CalcInertia(new AreaMomentOfInertia(x.Quantity.As(unit) * y.Quantity.As(unit),
            unit.GetEquivilantInertiaUnit()), name, "");
    }

    public static CalcLength operator /(CalcArea x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}/{y.DisplayName}";
        AreaUnit unit = x.Quantity.Unit;
        LengthUnit lengthUnit = unit.GetEquivilantLengthUnit();
        return new CalcLength(new Length(x.Quantity.As(unit) / y.Quantity.As(lengthUnit), lengthUnit), name, "");
    }

    public static CalcDouble operator /(CalcArea x, CalcArea y)
    {
        (string name, string _, AreaUnit _) = OperatorMetadataHelper<AreaUnit>(x, y, '/');
        return new CalcDouble(x.Quantity / y.Quantity, name, string.Empty);
    }
}
