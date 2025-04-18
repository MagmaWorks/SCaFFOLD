using Scaffold.Core.Abstract;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Static;

namespace Scaffold.Core.CalcQuantities;

public sealed class CalcForce : CalcQuantity<Force>
{
    public CalcForce(Force force, string name, string symbol = "")
        : base(force, name, symbol) { }

    public CalcForce(double value, ForceUnit unit, string name, string symbol)
        : base(new Force(value, unit), name, symbol) { }

    public CalcForce(double value, string name, string symbol)
        : base(new Force(value, ForceUnit.Kilonewton), name, symbol) { }

    public static CalcForce operator +(CalcForce x, CalcForce y)
    {
        (string name, string symbol, ForceUnit unit) = OperatorMetadataHelper<ForceUnit>(x, y, '+');
        return new CalcForce(new Force(x.Quantity.As(unit) + y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcForce operator -(CalcForce x, CalcForce y)
    {
        (string name, string symbol, ForceUnit unit) = OperatorMetadataHelper<ForceUnit>(x, y, '-');
        return new CalcForce(new Force(x.Quantity.As(unit) - y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcMoment operator *(CalcForce x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}\u2009·\u2009{y.DisplayName}";
        ForceUnit unit = (ForceUnit)x.Quantity.Unit;
        return new CalcMoment(new Moment(x.Quantity.As(unit) * y.Value,
            unit.GetEquivilantMomentUnit((LengthUnit)y.Quantity.Unit)), name, "");
    }

    public static CalcMoment operator *(CalcLength x, CalcForce y)
    {
        return y * x;
    }

    public static CalcLinearForce operator /(CalcForce x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}\u2009/\u2009{y.DisplayName}";
        ForceUnit unit = (ForceUnit)x.Quantity.Unit;
        return new CalcLinearForce(new ForcePerLength(x.Quantity.As(unit) / y.Value,
            unit.GetEquivilantForcePerLengthUnit((LengthUnit)y.Quantity.Unit)), name, "");
    }

    public static CalcStress operator /(CalcForce x, CalcArea y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}\u2009/\u2009{y.DisplayName}";
        ForceUnit unit = (ForceUnit)x.Quantity.Unit;
        return new CalcStress(new Pressure(x.Quantity.As(unit) / y.Value,
            unit.GetEquivilantPressureUnit((AreaUnit)y.Quantity.Unit)), name, "");
    }

    public static CalcDouble operator /(CalcForce x, CalcForce y)
    {
        (string name, string _, ForceUnit _) = OperatorMetadataHelper<ForceUnit>(x, y, '/');
        return new CalcDouble((Force)x.Quantity / (Force)y.Quantity, name, string.Empty);
    }

    public static CalcForce operator +(CalcForce x, double y)
    {
        return new CalcForce(x.Value + y, (ForceUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcForce operator +(double x, CalcForce y)
    {
        return y + x;
    }

    public static CalcForce operator -(CalcForce x, double y)
    {
        return new CalcForce(x.Value - y, (ForceUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcForce operator *(CalcForce x, double y)
    {
        return new CalcForce(x.Value * y, (ForceUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcForce operator *(double x, CalcForce y)
    {
        return y * x;
    }

    public static CalcForce operator /(CalcForce x, double y)
    {
        return new CalcForce(x.Value / y, (ForceUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }
}
