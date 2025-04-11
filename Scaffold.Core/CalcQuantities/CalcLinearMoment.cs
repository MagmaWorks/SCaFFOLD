using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core.Abstract;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Static;

namespace Scaffold.Core.CalcQuantities;

public sealed class CalcLinearMoment : CalcQuantity<MomentPerLength>
{
    public CalcLinearMoment(MomentPerLength linearMoment, string name, string symbol = "")
        : base(linearMoment, name, symbol) { }

    public CalcLinearMoment(double value, MomentPerLengthUnit unit, string name, string symbol)
        : base(new MomentPerLength(value, unit), name, symbol) { }

    public CalcLinearMoment(double value, string name, string symbol)
        : base(new MomentPerLength(value, MomentPerLengthUnit.KilonewtonMeterPerMeter), name, symbol) { }

    public static CalcLinearMoment operator +(CalcLinearMoment x, CalcLinearMoment y)
    {
        (string name, string symbol, MomentPerLengthUnit unit) = OperatorMetadataHelper<MomentPerLengthUnit>(x, y, '+');
        return new CalcLinearMoment(new MomentPerLength(x.Quantity.As(unit) + y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcLinearMoment operator -(CalcLinearMoment x, CalcLinearMoment y)
    {
        (string name, string symbol, MomentPerLengthUnit unit) = OperatorMetadataHelper<MomentPerLengthUnit>(x, y, '-');
        return new CalcLinearMoment(new MomentPerLength(x.Quantity.As(unit) - y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcMoment operator *(CalcLinearMoment x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
           ? string.Empty : $"{x.DisplayName}\u2009·\u2009{y.DisplayName}";
        MomentPerLengthUnit unit = (MomentPerLengthUnit)x.Quantity.Unit;
        return new CalcMoment(new Moment(x.Quantity.As(unit) * y.Quantity.As(unit.GetEquivilantLengthUnit()),
            unit.GetEquivilantMomentUnit()), name, "");
    }

    public static CalcMoment operator *(CalcLength x, CalcLinearMoment y)
    {
        return y * x;
    }

    public static CalcDouble operator /(CalcLinearMoment x, CalcLinearMoment y)
    {
        (string name, string _, MomentPerLengthUnit _) = OperatorMetadataHelper<MomentPerLengthUnit>(x, y, '/');
        return new CalcDouble((MomentPerLength)x.Quantity / (MomentPerLength)y.Quantity, name, string.Empty);
    }

    public static CalcLinearMoment operator +(CalcLinearMoment x, double y)
    {
        return new CalcLinearMoment(x.Value + y, (MomentPerLengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcLinearMoment operator +(double x, CalcLinearMoment y)
    {
        return y + x;
    }

    public static CalcLinearMoment operator -(CalcLinearMoment x, double y)
    {
        return new CalcLinearMoment(x.Value - y, (MomentPerLengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcLinearMoment operator *(CalcLinearMoment x, double y)
    {
        return new CalcLinearMoment(x.Value * y, (MomentPerLengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcLinearMoment operator *(double x, CalcLinearMoment y)
    {
        return y * x;
    }

    public static CalcLinearMoment operator /(CalcLinearMoment x, double y)
    {
        return new CalcLinearMoment(x.Value / y, (MomentPerLengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }
}
