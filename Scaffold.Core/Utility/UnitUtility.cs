﻿namespace Scaffold.Core.Utility;

public static partial class UnitUtility
{
    #region Geometry / Length conversions
    internal static ReciprocalLengthUnit GetEquivilantReciprocalLengthUnit(this LengthUnit unit)
        => (ReciprocalLengthUnit)UnitsNetSetup.Default.UnitParser.Parse(Length.GetAbbreviation(unit)
            + "⁻¹", typeof(ReciprocalLengthUnit));

    internal static LengthUnit GetEquivilantLengthUnit(this ReciprocalLengthUnit unit)
        => (LengthUnit)UnitsNetSetup.Default.UnitParser.Parse(ReciprocalLength.GetAbbreviation(unit)
            .Replace("⁻¹", string.Empty), typeof(LengthUnit));

    internal static LengthUnit GetEquivilantLengthUnit(this AreaUnit unit)
        => (LengthUnit)UnitsNetSetup.Default.UnitParser.Parse(Area.GetAbbreviation(unit)
            .Replace("²", string.Empty), typeof(LengthUnit));

    internal static LengthUnit GetEquivilantLengthUnit(this VolumeUnit unit)
        => (LengthUnit)UnitsNetSetup.Default.UnitParser.Parse(Volume.GetAbbreviation(unit)
            .Replace("³", string.Empty), typeof(LengthUnit));

    internal static LengthUnit GetEquivilantLengthUnit(this AreaMomentOfInertiaUnit unit)
        => (LengthUnit)UnitsNetSetup.Default.UnitParser.Parse(AreaMomentOfInertia.GetAbbreviation(unit)
            .Replace("⁴", string.Empty), typeof(LengthUnit));

    internal static AreaUnit GetEquivilantAreaUnit(this LengthUnit unit)
        => (AreaUnit)UnitsNetSetup.Default.UnitParser.Parse(
            Length.GetAbbreviation(unit) + "²", typeof(AreaUnit));

    internal static AreaUnit GetEquivilantAreaUnit(this VolumeUnit unit)
        => (AreaUnit)UnitsNetSetup.Default.UnitParser.Parse(Volume.GetAbbreviation(unit)
            .Replace("³", "²"), typeof(AreaUnit));

    internal static AreaUnit GetEquivilantAreaUnit(this AreaMomentOfInertiaUnit unit)
        => (AreaUnit)UnitsNetSetup.Default.UnitParser.Parse(AreaMomentOfInertia.GetAbbreviation(unit)
            .Replace("⁴", "²"), typeof(AreaUnit));

    internal static VolumeUnit GetEquivilantVolumeUnit(this LengthUnit unit)
        => (VolumeUnit)UnitsNetSetup.Default.UnitParser.Parse(
            Length.GetAbbreviation(unit) + "³", typeof(VolumeUnit));

    internal static VolumeUnit GetEquivilantVolumeUnit(this AreaUnit unit)
        => (VolumeUnit)UnitsNetSetup.Default.UnitParser.Parse(Area.GetAbbreviation(unit)
            .Replace("²", "³"), typeof(VolumeUnit));

    internal static VolumeUnit GetEquivilantVolumeUnit(this AreaMomentOfInertiaUnit unit)
        => (VolumeUnit)UnitsNetSetup.Default.UnitParser.Parse(AreaMomentOfInertia.GetAbbreviation(unit)
            .Replace("⁴", "³"), typeof(VolumeUnit));

    internal static AreaMomentOfInertiaUnit GetEquivilantInertiaUnit(this LengthUnit unit)
        => (AreaMomentOfInertiaUnit)UnitsNetSetup.Default.UnitParser.Parse(
            Length.GetAbbreviation(unit) + "⁴", typeof(AreaMomentOfInertiaUnit));

    internal static AreaMomentOfInertiaUnit GetEquivilantInertiaUnit(this AreaUnit unit)
        => (AreaMomentOfInertiaUnit)UnitsNetSetup.Default.UnitParser.Parse(Area.GetAbbreviation(unit)
            .Replace("²", "⁴"), typeof(AreaMomentOfInertiaUnit));

    internal static AreaMomentOfInertiaUnit GetEquivilantInertiaUnit(this VolumeUnit unit)
        => (AreaMomentOfInertiaUnit)UnitsNetSetup.Default.UnitParser.Parse(Volume.GetAbbreviation(unit)
            .Replace("³", "⁴"), typeof(AreaMomentOfInertiaUnit));
    #endregion

    #region Force / stress conversions
    public static MomentUnit GetEquivilantMomentUnit(this ForceUnit unit, LengthUnit length)
        => (MomentUnit)UnitsNetSetup.Default.UnitParser.Parse(Force.GetAbbreviation(unit)
            + "·" + Length.GetAbbreviation(length), typeof(MomentUnit));
    public static ForcePerLengthUnit GetEquivilantForcePerLengthUnit(this ForceUnit unit, LengthUnit length)
        => (ForcePerLengthUnit)UnitsNetSetup.Default.UnitParser.Parse(Force.GetAbbreviation(unit)
            + "/" + Length.GetAbbreviation(length), typeof(ForcePerLengthUnit));
    public static PressureUnit GetEquivilantPressureUnit(this ForceUnit unit, AreaUnit area)
        => (PressureUnit)UnitsNetSetup.Default.UnitParser.Parse(Force.GetAbbreviation(unit)
            + "/" + Area.GetAbbreviation(area), typeof(PressureUnit));
    public static ForceUnit GetEquivilantForceUnit(this MomentUnit unit)
        => (ForceUnit)UnitsNetSetup.Default.UnitParser.Parse(Moment.GetAbbreviation(unit)
            .Split('·')[0], typeof(ForceUnit));
    public static LengthUnit GetEquivilantLengthUnit(this MomentUnit unit)
        => (LengthUnit)UnitsNetSetup.Default.UnitParser.Parse(Moment.GetAbbreviation(unit)
            .Split('·')[1], typeof(LengthUnit));
    public static MomentPerLengthUnit GetEquivilantMomentPerLengthUnit(this MomentUnit unit, LengthUnit length)
        => (MomentPerLengthUnit)UnitsNetSetup.Default.UnitParser.Parse(Moment.GetAbbreviation(unit)
            + "/" + Length.GetAbbreviation(length), typeof(MomentPerLengthUnit));
    public static MomentUnit GetEquivilantMomentUnit(this MomentPerLengthUnit unit)
        => (MomentUnit)UnitsNetSetup.Default.UnitParser.Parse(MomentPerLength.GetAbbreviation(unit)
            .Split('/')[0], typeof(MomentUnit));
    public static LengthUnit GetEquivilantLengthUnit(this MomentPerLengthUnit unit)
        => (LengthUnit)UnitsNetSetup.Default.UnitParser.Parse(MomentPerLength.GetAbbreviation(unit)
            .Split('/')[1], typeof(LengthUnit));
    public static ForceUnit GetEquivilantForceUnit(this ForcePerLengthUnit unit)
        => (ForceUnit)UnitsNetSetup.Default.UnitParser.Parse(ForcePerLength.GetAbbreviation(unit)
            .Split('/')[0], typeof(ForceUnit));
    public static LengthUnit GetEquivilantLengthUnit(this ForcePerLengthUnit unit)
        => (LengthUnit)UnitsNetSetup.Default.UnitParser.Parse(ForcePerLength.GetAbbreviation(unit)
            .Split('/')[1], typeof(LengthUnit));
    public static PressureUnit GetEquivilantPressureUnit(this ForcePerLengthUnit unit)
        => (PressureUnit)UnitsNetSetup.Default.UnitParser.Parse(ForcePerLength.GetAbbreviation(unit)
            + "²", typeof(PressureUnit));
    public static ForceUnit GetEquivilantForceUnit(this PressureUnit unit)
        => (ForceUnit)UnitsNetSetup.Default.UnitParser.Parse(Pressure.GetAbbreviation(unit)
            .Split('/')[0], typeof(ForceUnit));
    public static ForcePerLengthUnit GetEquivilantForcePerLengthUnit(this PressureUnit unit)
        => (ForcePerLengthUnit)UnitsNetSetup.Default.UnitParser.Parse(Pressure.GetAbbreviation(unit)
            .Replace("²", string.Empty), typeof(ForcePerLengthUnit));
    public static AreaUnit GetEquivilantAreaUnit(this PressureUnit unit)
        => (AreaUnit)UnitsNetSetup.Default.UnitParser.Parse(Pressure.GetAbbreviation(unit)
            .Split('/')[1], typeof(AreaUnit));
    public static LengthUnit GetEquivilantLengthUnit(this PressureUnit unit)
        => unit.GetEquivilantAreaUnit().GetEquivilantLengthUnit();

    internal static PressureUnit GetKnownUnit(this PressureUnit unit)
    {
        switch (unit)
        {
            case PressureUnit.Kilopascal:
                return PressureUnit.KilonewtonPerSquareMeter;

            case PressureUnit.Megapascal:
                return PressureUnit.NewtonPerSquareMillimeter;
        }

        return unit;
    }
    #endregion
}
