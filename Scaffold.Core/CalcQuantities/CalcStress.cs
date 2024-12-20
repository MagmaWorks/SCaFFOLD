using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core.Abstract;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Static;

namespace Scaffold.Core.CalcQuantities;

public sealed class CalcStress : CalcQuantity<Pressure>
{
    public CalcStress(Pressure linearForce, string name, string symbol = "")
        : base(linearForce, name, symbol) { }

    public CalcStress(double value, PressureUnit unit, string name, string symbol)
        : base(new Pressure(value, unit), name, symbol) { }

    public static CalcStress operator +(CalcStress x, CalcStress y)
    {
        (string name, string symbol, PressureUnit unit) = OperatorMetadataHelper<PressureUnit>(x, y, '+');
        return new CalcStress(new Pressure(x.Quantity.As(unit) + y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcStress operator -(CalcStress x, CalcStress y)
    {
        (string name, string symbol, PressureUnit unit) = OperatorMetadataHelper<PressureUnit>(x, y, '-');
        return new CalcStress(new Pressure(x.Quantity.As(unit) - y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcLinearForce operator *(CalcStress x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
           ? string.Empty : $"{x.DisplayName}\u2009·\u2009{y.DisplayName}";
        PressureUnit unit = x.Quantity.Unit.GetKnownUnit();
        return new CalcLinearForce(new ForcePerLength(x.Quantity.As(unit) * y.Quantity.As(unit.GetEquivilantLengthUnit()),
            unit.GetEquivilantForcePerLengthUnit()), name, "");
    }

    public static CalcLinearForce operator *(CalcLength x, CalcStress y)
    {
        return y * x;
    }

    public static CalcForce operator *(CalcStress x, CalcArea y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
           ? string.Empty : $"{x.DisplayName}\u2009·\u2009{y.DisplayName}";
        PressureUnit unit = x.Quantity.Unit.GetKnownUnit();
        return new CalcForce(new Force(x.Quantity.As(unit) * y.Quantity.As(unit.GetEquivilantAreaUnit()),
            unit.GetEquivilantForceUnit()), name, "");
    }

    public static CalcForce operator *(CalcArea x, CalcStress y)
    {
        return y * x;
    }

    public static CalcDouble operator /(CalcStress x, CalcStress y)
    {
        (string name, string _, PressureUnit _) = OperatorMetadataHelper<PressureUnit>(x, y, '/');
        return new CalcDouble(x.Quantity / y.Quantity, name, string.Empty);
    }
}
