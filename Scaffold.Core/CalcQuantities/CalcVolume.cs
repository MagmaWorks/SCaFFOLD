using Scaffold.Core.Abstract;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Static;

namespace Scaffold.Core.CalcQuantities;

public sealed class CalcVolume : CalcQuantity<Volume>
{
    public CalcVolume(Volume volume, string name, string symbol = "")
        : base(volume, name, symbol) { }

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
            ? string.Empty : $"{x.DisplayName}\u2009·\u2009{y.DisplayName}";
        VolumeUnit unit = (VolumeUnit)x.Quantity.Unit;
        return new CalcInertia(new AreaMomentOfInertia(x.Quantity.As(unit) * y.Quantity.As(unit.GetEquivilantLengthUnit()),
            unit.GetEquivilantInertiaUnit()), name, "");
    }

    public static CalcDouble operator /(CalcVolume x, CalcVolume y)
    {
        (string name, string _, VolumeUnit unit) = OperatorMetadataHelper<VolumeUnit>(x, y, '/');
        return new CalcDouble((Volume)x.Quantity / (Volume)y.Quantity, name, string.Empty);
    }

    public static CalcLength operator /(CalcVolume x, CalcArea y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}\u2009/\u2009{y.DisplayName}";
        VolumeUnit unit = (VolumeUnit)x.Quantity.Unit;
        return new CalcLength(new Length(x.Quantity.As(unit) / y.Quantity.As(unit.GetEquivilantAreaUnit()),
            unit.GetEquivilantLengthUnit()), name, "");
    }

    public static CalcArea operator /(CalcVolume x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}\u2009/\u2009{y.DisplayName}";
        VolumeUnit unit = (VolumeUnit)x.Quantity.Unit;
        return new CalcArea(new Area(x.Quantity.As(unit) / y.Quantity.As(unit.GetEquivilantLengthUnit()),
            unit.GetEquivilantAreaUnit()), name, "");
    }

    public static CalcVolume operator +(CalcVolume x, double y)
    {
        return new CalcVolume(x.Value + y, (VolumeUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcVolume operator +(double x, CalcVolume y)
    {
        return y + x;
    }

    public static CalcVolume operator -(CalcVolume x, double y)
    {
        return new CalcVolume(x.Value - y, (VolumeUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcVolume operator *(CalcVolume x, double y)
    {
        return new CalcVolume(x.Value * y, (VolumeUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcVolume operator *(double x, CalcVolume y)
    {
        return y * x;
    }

    public static CalcVolume operator /(CalcVolume x, double y)
    {
        return new CalcVolume(x.Value / y, (VolumeUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }
}
