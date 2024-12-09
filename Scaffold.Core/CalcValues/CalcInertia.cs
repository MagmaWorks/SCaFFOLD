using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core.Abstract;
using Scaffold.Core.Static;

namespace Scaffold.Core.CalcValues;

public sealed class CalcInertia : CalcQuantity<AreaMomentOfInertia>
{
    public CalcInertia(AreaMomentOfInertia AreaMomentOfInertia, string name, string symbol = "")
        : base(AreaMomentOfInertia, name, symbol) { }

    public CalcInertia(double value, AreaMomentOfInertiaUnit unit, string name, string symbol)
        : base(new AreaMomentOfInertia(value, unit), name, symbol) { }


    public static implicit operator double(CalcInertia value) => value.Value;
    public static implicit operator AreaMomentOfInertia(CalcInertia value) => value.Quantity;

    public static CalcInertia operator +(CalcInertia x, CalcInertia y)
    {
        (string name, string symbol, AreaMomentOfInertiaUnit unit) = OperatorMetadataHelper(x, y, '+');
        return new CalcInertia(new AreaMomentOfInertia(x.Value + y.Value, unit), name, symbol);
    }

    public static CalcInertia operator -(CalcInertia x, CalcInertia y)
    {
        (string name, string symbol, AreaMomentOfInertiaUnit unit) = OperatorMetadataHelper(x, y, '-');
        return new CalcInertia(new AreaMomentOfInertia(x.Value - y.Value, unit), name, symbol);
    }

    public static CalcVolume operator /(CalcInertia x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}/{y.DisplayName}";
        AreaMomentOfInertiaUnit unit = x.Quantity.Unit;
        return new CalcVolume(new Volume(x.Quantity.As(unit) / y.Quantity.As(unit.GetEquivilantLengthUnit()),
            unit.GetEquivilantVolumeUnit()), name, "");
    }

    public static CalcArea operator /(CalcInertia x, CalcArea y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}/{y.DisplayName}";
        AreaMomentOfInertiaUnit unit = x.Quantity.Unit;
        AreaUnit areaUnit = unit.GetEquivilantAreaUnit();
        return new CalcArea(new Area(x.Quantity.As(unit) / y.Quantity.As(areaUnit), areaUnit), name, "");
    }

    public static CalcLength operator /(CalcInertia x, CalcVolume y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}/{y.DisplayName}";
        AreaMomentOfInertiaUnit unit = x.Quantity.Unit;
        return new CalcLength(new Length(x.Quantity.As(unit) / y.Quantity.As(unit.GetEquivilantVolumeUnit()),
            unit.GetEquivilantLengthUnit()), name, "");
    }

    public static CalcDouble operator /(CalcInertia x, CalcInertia y)
    {
        (string name, string symbol, AreaMomentOfInertiaUnit unit) = OperatorMetadataHelper(x, y, '/');
        return new CalcDouble(name, symbol, x.Quantity / y.Quantity);
    }

    private static (string name, string symbol, AreaMomentOfInertiaUnit unit) OperatorMetadataHelper(CalcInertia x, CalcInertia y, char operation)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}{operation}{y.DisplayName}";
        string symbol = x.Symbol == y.Symbol ? x.Symbol : string.Empty;
        AreaMomentOfInertiaUnit unit = x.Quantity.Unit;
        return (name, symbol, unit);
    }
}
