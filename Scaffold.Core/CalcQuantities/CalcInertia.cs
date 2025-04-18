﻿using Scaffold.Core.Abstract;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Static;

namespace Scaffold.Core.CalcQuantities;

public sealed class CalcInertia : CalcQuantity<AreaMomentOfInertia>
{
    public CalcInertia(AreaMomentOfInertia areaMomentOfInertia, string name, string symbol = "")
        : base(areaMomentOfInertia, name, symbol) { }

    public CalcInertia(double value, AreaMomentOfInertiaUnit unit, string name, string symbol)
        : base(new AreaMomentOfInertia(value, unit), name, symbol) { }

    public static CalcInertia operator +(CalcInertia x, CalcInertia y)
    {
        (string name, string symbol, AreaMomentOfInertiaUnit unit)
            = OperatorMetadataHelper<AreaMomentOfInertiaUnit>(x, y, '+');
        return new CalcInertia(new AreaMomentOfInertia(x.Quantity.As(unit) + y.Quantity.As(unit),
            unit), name, symbol);
    }

    public static CalcInertia operator -(CalcInertia x, CalcInertia y)
    {
        (string name, string symbol, AreaMomentOfInertiaUnit unit)
            = OperatorMetadataHelper<AreaMomentOfInertiaUnit>(x, y, '-');
        return new CalcInertia(new AreaMomentOfInertia(x.Quantity.As(unit) - y.Quantity.As(unit),
            unit), name, symbol);
    }

    public static CalcDouble operator /(CalcInertia x, CalcInertia y)
    {
        (string name, _, AreaMomentOfInertiaUnit _) = OperatorMetadataHelper<AreaMomentOfInertiaUnit>(x, y, '/');
        return new CalcDouble((AreaMomentOfInertia)x.Quantity / (AreaMomentOfInertia)y.Quantity, name, string.Empty);
    }

    public static CalcVolume operator /(CalcInertia x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}\u2009/\u2009{y.DisplayName}";
        AreaMomentOfInertiaUnit unit = (AreaMomentOfInertiaUnit)x.Quantity.Unit;
        return new CalcVolume(new Volume(x.Quantity.As(unit) / y.Quantity.As(unit.GetEquivilantLengthUnit()),
            unit.GetEquivilantVolumeUnit()), name, "");
    }

    public static CalcArea operator /(CalcInertia x, CalcArea y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}\u2009/\u2009{y.DisplayName}";
        AreaMomentOfInertiaUnit unit = (AreaMomentOfInertiaUnit)x.Quantity.Unit;
        AreaUnit areaUnit = unit.GetEquivilantAreaUnit();
        return new CalcArea(new Area(x.Quantity.As(unit) / y.Quantity.As(areaUnit), areaUnit), name, "");
    }

    public static CalcLength operator /(CalcInertia x, CalcVolume y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}\u2009/\u2009{y.DisplayName}";
        AreaMomentOfInertiaUnit unit = (AreaMomentOfInertiaUnit)x.Quantity.Unit;
        return new CalcLength(new Length(x.Quantity.As(unit) / y.Quantity.As(unit.GetEquivilantVolumeUnit()),
            unit.GetEquivilantLengthUnit()), name, "");
    }

    public static CalcInertia operator +(CalcInertia x, double y)
    {
        return new CalcInertia(x.Value + y, (AreaMomentOfInertiaUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcInertia operator +(double x, CalcInertia y)
    {
        return y + x;
    }

    public static CalcInertia operator -(CalcInertia x, double y)
    {
        return new CalcInertia(x.Value - y, (AreaMomentOfInertiaUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcInertia operator *(CalcInertia x, double y)
    {
        return new CalcInertia(x.Value * y, (AreaMomentOfInertiaUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcInertia operator *(double x, CalcInertia y)
    {
        return y * x;
    }

    public static CalcInertia operator /(CalcInertia x, double y)
    {
        return new CalcInertia(x.Value / y, (AreaMomentOfInertiaUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }
}
