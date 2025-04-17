using Scaffold.Core.Abstract;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Static;

namespace Scaffold.Core.CalcQuantities;

public sealed class CalcLinearForce : CalcQuantity<ForcePerLength>
{
    public CalcLinearForce(ForcePerLength linearForce, string name, string symbol = "")
        : base(linearForce, name, symbol) { }

    public CalcLinearForce(double value, ForcePerLengthUnit unit, string name, string symbol)
        : base(new ForcePerLength(value, unit), name, symbol) { }

    public CalcLinearForce(double value, string name, string symbol)
        : base(new ForcePerLength(value, ForcePerLengthUnit.KilonewtonPerMeter), name, symbol) { }

    public static CalcLinearForce operator +(CalcLinearForce x, CalcLinearForce y)
    {
        (string name, string symbol, ForcePerLengthUnit unit) = OperatorMetadataHelper<ForcePerLengthUnit>(x, y, '+');
        return new CalcLinearForce(new ForcePerLength(x.Quantity.As(unit) + y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcLinearForce operator -(CalcLinearForce x, CalcLinearForce y)
    {
        (string name, string symbol, ForcePerLengthUnit unit) = OperatorMetadataHelper<ForcePerLengthUnit>(x, y, '-');
        return new CalcLinearForce(new ForcePerLength(x.Quantity.As(unit) - y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcForce operator *(CalcLinearForce x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
           ? string.Empty : $"{x.DisplayName}\u2009·\u2009{y.DisplayName}";
        ForcePerLengthUnit unit = (ForcePerLengthUnit)x.Quantity.Unit;
        return new CalcForce(new Force(x.Quantity.As(unit) * y.Quantity.As(unit.GetEquivilantLengthUnit()),
            unit.GetEquivilantForceUnit()), name, "");
    }

    public static CalcForce operator *(CalcLength x, CalcLinearForce y)
    {
        return y * x;
    }

    public static CalcStress operator /(CalcLinearForce x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
           ? string.Empty : $"{x.DisplayName}\u2009/\u2009{y.DisplayName}";
        ForcePerLengthUnit unit = (ForcePerLengthUnit)x.Quantity.Unit;
        return new CalcStress(new Pressure(x.Quantity.As(unit) / y.Quantity.As(unit.GetEquivilantLengthUnit()),
            unit.GetEquivilantPressureUnit()), name, "");
    }

    public static CalcDouble operator /(CalcLinearForce x, CalcLinearForce y)
    {
        (string name, string _, ForcePerLengthUnit _) = OperatorMetadataHelper<ForcePerLengthUnit>(x, y, '/');
        return new CalcDouble((ForcePerLength)x.Quantity / (ForcePerLength)y.Quantity, name, string.Empty);
    }

    public static CalcLinearForce operator +(CalcLinearForce x, double y)
    {
        return new CalcLinearForce(x.Value + y, (ForcePerLengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcLinearForce operator +(double x, CalcLinearForce y)
    {
        return y + x;
    }

    public static CalcLinearForce operator -(CalcLinearForce x, double y)
    {
        return new CalcLinearForce(x.Value - y, (ForcePerLengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcLinearForce operator *(CalcLinearForce x, double y)
    {
        return new CalcLinearForce(x.Value * y, (ForcePerLengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcLinearForce operator *(double x, CalcLinearForce y)
    {
        return y * x;
    }

    public static CalcLinearForce operator /(CalcLinearForce x, double y)
    {
        return new CalcLinearForce(x.Value / y, (ForcePerLengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }
}
