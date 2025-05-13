﻿#if NET7_0_OR_GREATER
using System.Numerics;
#endif
using Scaffold.Core.Abstract;
using Scaffold.Core.Static;

namespace Scaffold.Core.CalcQuantities;

public sealed class CalcLength : CalcQuantity<Length>
#if NET7_0_OR_GREATER
    , IParsable<CalcLength>
    , IAdditionOperators<CalcLength, CalcLength, CalcLength>
    , IAdditionOperators<CalcLength, double, CalcLength>
    , IAdditiveIdentity<CalcLength, CalcLength>
    , ISubtractionOperators<CalcLength, CalcLength, CalcLength>
    , ISubtractionOperators<CalcLength, double, CalcLength>
    , IMultiplyOperators<CalcLength, CalcLength, CalcArea>
    , IMultiplyOperators<CalcLength, double, CalcLength>
    , IMultiplyOperators<CalcLength, CalcArea, CalcVolume>
    , IMultiplyOperators<CalcLength, CalcVolume, CalcInertia>
    , IDivisionOperators<CalcLength, CalcLength, CalcStrain>
    , IDivisionOperators<CalcLength, double, CalcLength>
    , IUnaryNegationOperators<CalcLength, CalcLength>
    , IComparisonOperators<CalcLength, CalcLength, bool>
#endif
{
    public CalcLength(Length length, string name, string symbol = "")
        : base(length, name, symbol) { }

    public CalcLength(double value, LengthUnit unit, string name, string symbol)
        : base(new Length(value, unit), name, symbol) { }

    #region AdditionOperators
    public static CalcLength operator +(CalcLength x, CalcLength y)
    {
        (string name, string symbol, LengthUnit unit) = OperatorMetadataHelper<LengthUnit>(x, y, '+');
        return new CalcLength(new Length(x.Quantity.As(unit) + y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcLength operator +(CalcLength x, double y)
    {
        return new CalcLength(x.Value + y, (LengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcLength operator +(double x, CalcLength y) => y + x;
    #endregion

    #region SubtractionOperators
    public static CalcLength operator -(CalcLength x)
    {
        return new CalcLength(-(Length)x.Quantity, $"-{x.DisplayName}", x.Symbol);
    }

    public static CalcLength operator -(CalcLength x, CalcLength y)
    {
        (string name, string symbol, LengthUnit unit) = OperatorMetadataHelper<LengthUnit>(x, y, '-');
        return new CalcLength(new Length(x.Quantity.As(unit) - y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcLength operator -(CalcLength x, double y)
    {
        return new CalcLength(x.Value - y, (LengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }
    #endregion

    #region MultiplicationOperators
    public static CalcArea operator *(CalcLength x, CalcLength y)
    {
        (string name, string symbol, LengthUnit unit) = OperatorMetadataHelper<LengthUnit>(x, y, '·');
        return new CalcArea(new Area(x.Quantity.As(unit) * y.Quantity.As(unit),
            unit.GetEquivilantAreaUnit()), name, symbol);
    }
    public static CalcLength operator *(CalcLength x, double y)
    {
        return new CalcLength(x.Value * y, (LengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcLength operator *(double x, CalcLength y) => y * x;

    public static CalcVolume operator *(CalcLength x, CalcArea y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}\u2009·\u2009{y.DisplayName}";
        LengthUnit unit = (LengthUnit)x.Quantity.Unit;
        return new CalcVolume(new Volume(x.Quantity.As(unit) * y.Quantity.As(unit.GetEquivilantAreaUnit()),
            unit.GetEquivilantVolumeUnit()), name, "");
    }

    public static CalcInertia operator *(CalcLength x, CalcVolume y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}\u2009·\u2009{y.DisplayName}";
        LengthUnit unit = (LengthUnit)x.Quantity.Unit;
        return new CalcInertia(new AreaMomentOfInertia(
            x.Quantity.As(unit) * y.Quantity.As(unit.GetEquivilantVolumeUnit()),
            unit.GetEquivilantInertiaUnit()), name, "");
    }

    public static CalcLength operator *(CalcStrain x, CalcLength y)
    {
        return new CalcLength(x.Quantity.As(StrainUnit.DecimalFraction) * (Length)y.Quantity, string.Empty, string.Empty);
    }

    public static CalcLength operator *(CalcLength x, CalcStrain y) => y * x;
    #endregion

    #region DivisionOperators
    public static CalcStrain operator /(CalcLength x, CalcLength y)
    {
        (string name, string _, LengthUnit unit) = OperatorMetadataHelper<LengthUnit>(x, y, '/');
        return new CalcStrain((Length)x.Quantity / (Length)y.Quantity, StrainUnit.DecimalFraction, name, string.Empty);
    }

    public static CalcLength operator /(CalcLength x, double y)
    {
        return new CalcLength(x.Value / y, (LengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }
    #endregion

    #region PowerOperators
    public static ICalcQuantity operator ^(CalcLength x, double y)
    {
        LengthUnit unit = (LengthUnit)x.Quantity.Unit;
        string name;
        switch (y)
        {
            case 2:
                name = string.IsNullOrEmpty(x.DisplayName) ? string.Empty : $"{x.DisplayName}²";
                return new CalcArea(Math.Pow(x.Value, y), unit.GetEquivilantAreaUnit(), name, "");

            case 3:
                name = string.IsNullOrEmpty(x.DisplayName) ? string.Empty : $"{x.DisplayName}³";
                return new CalcVolume(Math.Pow(x.Value, y), unit.GetEquivilantVolumeUnit(), name, "");

            case 4:
                name = string.IsNullOrEmpty(x.DisplayName) ? string.Empty : $"{x.DisplayName}⁴";
                return new CalcInertia(Math.Pow(x.Value, y), unit.GetEquivilantInertiaUnit(), name, "");
        }

        throw new MathException("CalcLength can only be raised by the power of 2, 3 or 4");
    }
    #endregion

    #region ComparisonOperators
    public static bool operator >(CalcLength left, CalcLength right) => GreaterThan(left, right);
    public static bool operator >=(CalcLength left, CalcLength right) => GreaterOrEqualThan(left, right);
    public static bool operator <(CalcLength left, CalcLength right) => LessThan(left, right);
    public static bool operator <=(CalcLength left, CalcLength right) => LessOrEqualThan(left, right);
    public static bool operator ==(CalcLength left, CalcLength right) => left.Equals(right);
    public static bool operator !=(CalcLength left, CalcLength right) => !left.Equals(right);
    #endregion

    public static bool TryParse(string str, IFormatProvider provider, out CalcLength result)
    {
        if (Length.TryParse(str, provider, out Length quantity))
        {
            result = new CalcLength(quantity, string.Empty);
            return true;
        }

        result = null;
        return false;
    }

    public static CalcLength Parse(string str, IFormatProvider provider)
    {
        return new CalcLength(Length.Parse(str, provider), string.Empty);
    }

    internal static CalcLength OneMeter => new CalcLength(Length.FromMeters(1), "Unit Length");
    public static CalcLength Zero => new CalcLength(Length.Zero, string.Empty);
    public static CalcLength AdditiveIdentity => Zero;
    public override bool Equals(object obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();
}
