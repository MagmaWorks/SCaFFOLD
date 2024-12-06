﻿using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcValues;

public sealed class CalcArea : CalcQuantity<Area>
{
    public CalcArea(Area Area, string name, string symbol = "")
        : base(Area, name, symbol) { }

    public CalcArea(double value, AreaUnit unit, string name, string symbol)
        : base(new Area(value, unit), name, symbol) { }


    public static implicit operator double(CalcArea value) => value.Value;
    public static implicit operator Area(CalcArea value) => value.Quantity;

    public static CalcArea operator +(CalcArea x, CalcArea y)
    {
        (string name, string symbol, AreaUnit unit) = OperatorMetadataHelper(x, y, '+');
        return new CalcArea(new Area(x.Value + y.Value, unit), name, symbol);
    }

    public static CalcArea operator -(CalcArea x, CalcArea y)
    {
        (string name, string symbol, AreaUnit unit) = OperatorMetadataHelper(x, y, '-');
        return new CalcArea(new Area(x.Value - y.Value, unit), name, symbol);
    }

    public static CalcVolume operator *(CalcArea x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}·{y.DisplayName}";
        AreaUnit unit = x.Quantity.Unit;
        Length.TryParse($"0 {Area.GetAbbreviation(unit).Replace("²", string.Empty)}", out Length length);
        Volume.TryParse($"0 {Area.GetAbbreviation(unit).Replace("²", "³")}", out Volume vol);
        return new CalcVolume(new Volume(x.Quantity.As(unit) * y.Quantity.As(length.Unit), vol.Unit), name, "");
    }

    public static CalcInertia operator *(CalcArea x, CalcArea y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}·{y.DisplayName}";
        AreaUnit unit = x.Quantity.Unit;
        AreaMomentOfInertia.TryParse($"0 {Area.GetAbbreviation(unit).Replace("²", "⁴")}", out AreaMomentOfInertia res);
        return new CalcInertia(new AreaMomentOfInertia(x.Quantity.As(unit) * y.Quantity.As(unit), res.Unit), name, "");
    }

    public static CalcLength operator /(CalcArea x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}/{y.DisplayName}";
        AreaUnit unit = x.Quantity.Unit;
        Length.TryParse($"0 {Area.GetAbbreviation(unit).Replace("²", string.Empty)}", out Length res);
        return new CalcLength(new Length(x.Quantity.As(unit) / y.Quantity.As(res.Unit), res.Unit), name, "");
    }

    public static CalcDouble operator /(CalcArea x, CalcArea y)
    {
        (string name, string symbol, AreaUnit unit) = OperatorMetadataHelper(x, y, '/');
        return new CalcDouble(name, symbol, x.Quantity / y.Quantity);
    }

    private static (string name, string symbol, AreaUnit unit) OperatorMetadataHelper(CalcArea x, CalcArea y, char operation)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}{operation}{y.DisplayName}";
        string symbol = x.Symbol == y.Symbol ? x.Symbol : string.Empty;
        AreaUnit unit = x.Quantity.Unit == y.Quantity.Unit ? x.Quantity.Unit : AreaUnit.SquareMeter;
        return (name, symbol, unit);
    }
}
