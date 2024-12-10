using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core.Abstract;
using Scaffold.Core.Static;

namespace Scaffold.Core.CalcValues;

public sealed class CalcVolume : CalcQuantity<Volume>
{
    public CalcVolume(Volume Volume, string name, string symbol = "")
        : base(Volume, name, symbol) { }

    public CalcVolume(double value, VolumeUnit unit, string name, string symbol)
        : base(new Volume(value, unit), name, symbol) { }

    public static CalcVolume operator +(CalcVolume x, CalcVolume y)
    {
        (string name, string symbol, VolumeUnit unit) = OperatorMetadataHelper<VolumeUnit>(x, y, '+');
        return new CalcVolume(new Volume(x.Quantity.As(unit) + y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcVolume operator -(CalcVolume x, CalcVolume y)
    {
        (string name, string symbol, VolumeUnit unit) = OperatorMetadataHelper<VolumeUnit>(x, y, '-');
        return new CalcVolume(new Volume(x.Quantity.As(unit) - y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcInertia operator *(CalcVolume x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}·{y.DisplayName}";
        VolumeUnit unit = x.Quantity.Unit;
        return new CalcInertia(new AreaMomentOfInertia(x.Quantity.As(unit) * y.Quantity.As(unit.GetEquivilantLengthUnit()),
            unit.GetEquivilantInertiaUnit()), name, "");
    }

    public static CalcDouble operator /(CalcVolume x, CalcVolume y)
    {
        (string name, string _, VolumeUnit unit) = OperatorMetadataHelper<VolumeUnit>(x, y, '/');
        return new CalcDouble(x.Quantity / y.Quantity, name, string.Empty);
    }

    public static CalcLength operator /(CalcVolume x, CalcArea y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}/{y.DisplayName}";
        VolumeUnit unit = x.Quantity.Unit;
        return new CalcLength(new Length(x.Quantity.As(unit) / y.Quantity.As(unit.GetEquivilantAreaUnit()),
            unit.GetEquivilantLengthUnit()), name, "");
    }

    public static CalcArea operator /(CalcVolume x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}/{y.DisplayName}";
        VolumeUnit unit = x.Quantity.Unit;
        return new CalcArea(new Area(x.Quantity.As(unit) / y.Quantity.As(unit.GetEquivilantLengthUnit()),
            unit.GetEquivilantAreaUnit()), name, "");
    }
}
