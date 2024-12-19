using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core.Abstract;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Static;

namespace Scaffold.Core.CalcQuantities;

public sealed class CalcMoment : CalcQuantity<Moment>
{
    public CalcMoment(Moment moment, string name, string symbol = "")
        : base(moment, name, symbol) { }

    public CalcMoment(double value, MomentUnit unit, string name, string symbol)
        : base(new Moment(value, unit), name, symbol) { }

    public CalcMoment(double value, string name, string symbol)
        : base(new Moment(value, MomentUnit.KilonewtonMeter), name, symbol) { }

    public static CalcMoment operator +(CalcMoment x, CalcMoment y)
    {
        (string name, string symbol, MomentUnit unit) = OperatorMetadataHelper<MomentUnit>(x, y, '+');
        return new CalcMoment(new Moment(x.Quantity.As(unit) + y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcMoment operator -(CalcMoment x, CalcMoment y)
    {
        (string name, string symbol, MomentUnit unit) = OperatorMetadataHelper<MomentUnit>(x, y, '-');
        return new CalcMoment(new Moment(x.Quantity.As(unit) - y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcForce operator /(CalcMoment x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
           ? string.Empty : $"{x.DisplayName}\u2009/\u2009{y.DisplayName}";
        MomentUnit unit = x.Quantity.Unit;
        return new CalcForce(new Force(x.Quantity.As(unit) / y.Quantity.As(unit.GetEquivilantLengthUnit()),
            unit.GetEquivilantForceUnit()), name, "");
    }

    public static CalcLength operator /(CalcMoment x, CalcForce y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
           ? string.Empty : $"{x.DisplayName}\u2009/\u2009{y.DisplayName}";
        MomentUnit unit = x.Quantity.Unit;
        return new CalcLength(new Length(x.Quantity.As(unit) / y.Quantity.As(unit.GetEquivilantForceUnit()),
            unit.GetEquivilantLengthUnit()), name, "");
    }

    public static CalcDouble operator /(CalcMoment x, CalcMoment y)
    {
        (string name, string _, MomentUnit _) = OperatorMetadataHelper<MomentUnit>(x, y, '/');
        return new CalcDouble(x.Quantity / y.Quantity, name, string.Empty);
    }
}
