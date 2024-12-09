using OasysUnits;
using OasysUnits.Units;

namespace Scaffold.Core.Static;

public static class UnitUtility
{
    internal static LengthUnit GetEquivilantLengthUnit(this AreaUnit unit)
        => (LengthUnit)UnitParser.Default.Parse(Area.GetAbbreviation(unit)
            .Replace("²", string.Empty), typeof(LengthUnit));

    internal static LengthUnit GetEquivilantLengthUnit(this VolumeUnit unit)
        => (LengthUnit)UnitParser.Default.Parse(Volume.GetAbbreviation(unit)
            .Replace("³", string.Empty), typeof(LengthUnit));

    internal static LengthUnit GetEquivilantLengthUnit(this AreaMomentOfInertiaUnit unit)
        => (LengthUnit)UnitParser.Default.Parse(AreaMomentOfInertia.GetAbbreviation(unit)
            .Replace("⁴", string.Empty), typeof(LengthUnit));

    internal static AreaUnit GetEquivilantAreaUnit(this LengthUnit unit)
        => (AreaUnit)UnitParser.Default.Parse(
            Length.GetAbbreviation(unit) + "²", typeof(AreaUnit));

    internal static AreaUnit GetEquivilantAreaUnit(this VolumeUnit unit)
        => (AreaUnit)UnitParser.Default.Parse(Volume.GetAbbreviation(unit)
            .Replace("³", "²"), typeof(AreaUnit));

    internal static AreaUnit GetEquivilantAreaUnit(this AreaMomentOfInertiaUnit unit)
        => (AreaUnit)UnitParser.Default.Parse(AreaMomentOfInertia.GetAbbreviation(unit)
            .Replace("⁴", "²"), typeof(AreaUnit));

    internal static VolumeUnit GetEquivilantVolumeUnit(this LengthUnit unit)
        => (VolumeUnit)UnitParser.Default.Parse(
            Length.GetAbbreviation(unit) + "³", typeof(VolumeUnit));

    internal static VolumeUnit GetEquivilantVolumeUnit(this AreaUnit unit)
        => (VolumeUnit)UnitParser.Default.Parse(Area.GetAbbreviation(unit)
            .Replace("²", "³"), typeof(VolumeUnit));

    internal static VolumeUnit GetEquivilantVolumeUnit(this AreaMomentOfInertiaUnit unit)
        => (VolumeUnit)UnitParser.Default.Parse(AreaMomentOfInertia.GetAbbreviation(unit)
            .Replace("⁴", "³"), typeof(VolumeUnit));

    internal static AreaMomentOfInertiaUnit GetEquivilantInertiaUnit(this LengthUnit unit)
        => (AreaMomentOfInertiaUnit)UnitParser.Default.Parse(
            Length.GetAbbreviation(unit) + "⁴", typeof(AreaMomentOfInertiaUnit));

    internal static AreaMomentOfInertiaUnit GetEquivilantInertiaUnit(this AreaUnit unit)
        => (AreaMomentOfInertiaUnit)UnitParser.Default.Parse(Area.GetAbbreviation(unit)
            .Replace("²", "⁴"), typeof(AreaMomentOfInertiaUnit));

    internal static AreaMomentOfInertiaUnit GetEquivilantInertiaUnit(this VolumeUnit unit)
        => (AreaMomentOfInertiaUnit)UnitParser.Default.Parse(Volume.GetAbbreviation(unit)
            .Replace("³", "⁴"), typeof(AreaMomentOfInertiaUnit));
}

