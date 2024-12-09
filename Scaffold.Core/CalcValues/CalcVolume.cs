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


    public static implicit operator double(CalcVolume value) => value.Value;
    public static implicit operator Volume(CalcVolume value) => value.Quantity;

    public static CalcVolume operator +(CalcVolume x, CalcVolume y)
    {
        (string name, string symbol, VolumeUnit unit) = OperatorMetadataHelper(x, y, '+');
        return new CalcVolume(new Volume(x.Quantity.As(unit) + y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcVolume operator -(CalcVolume x, CalcVolume y)
    {
        (string name, string symbol, VolumeUnit unit) = OperatorMetadataHelper(x, y, '-');
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
        (string name, string _, VolumeUnit unit) = OperatorMetadataHelper(x, y, '/');
        return new CalcDouble(name, string.Empty, x.Quantity / y.Quantity);
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

    private static (string name, string symbol, VolumeUnit unit) OperatorMetadataHelper(CalcVolume x, CalcVolume y, char operation)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}{operation}{y.DisplayName}";
        string symbol = x.Symbol == y.Symbol ? x.Symbol : string.Empty;
        VolumeUnit unit = x.Quantity.Unit;
        return (name, symbol, unit);
    }
}
