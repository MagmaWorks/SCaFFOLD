using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcValues;

public sealed class CalcVolume : CalcQuantity<Volume>
{
    public CalcVolume(Volume Volume, string name, string symbol = "")
        : base(Volume, name, symbol) { }

    public CalcVolume(double value, VolumeUnit unit, string name, string symbol)
        : base(new Volume(value, unit), name, symbol) { }


    public static implicit operator double(CalcVolume value) => value.Value;
    public static implicit operator Volume(CalcVolume value) => value.Quantity;

    public static CalcVolume operator +(CalcVolume x, CalcVolume y)
    {
        (string name, string symbol, VolumeUnit unit) = OperatorMetadataHelper(x, y, '+');
        return new CalcVolume(new Volume(x.Value + y.Value, unit), name, symbol);
    }

    public static CalcVolume operator -(CalcVolume x, CalcVolume y)
    {
        (string name, string symbol, VolumeUnit unit) = OperatorMetadataHelper(x, y, '-');
        return new CalcVolume(new Volume(x.Value - y.Value, unit), name, symbol);
    }

    public static CalcInertia operator *(CalcVolume x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}·{y.DisplayName}";
        VolumeUnit unit = x.Quantity.Unit;
        AreaMomentOfInertia.TryParse($"0 {Volume.GetAbbreviation(unit).Replace("³", "⁴")}", out AreaMomentOfInertia res);
        Length.TryParse($"0 {Volume.GetAbbreviation(unit).Replace("³", string.Empty)}", out Length length);
        return new CalcInertia(new AreaMomentOfInertia(x.Quantity.As(unit) * y.Quantity.As(length.Unit), res.Unit), name, "");
    }

    public static CalcArea operator /(CalcVolume x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}/{y.DisplayName}";
        VolumeUnit unit = x.Quantity.Unit;
        Area.TryParse($"0 {Volume.GetAbbreviation(unit).Replace("³", "²")}", out Area area);
        Length.TryParse($"0 {Volume.GetAbbreviation(unit).Replace("³", string.Empty)}", out Length length);
        return new CalcArea(new Area(x.Quantity.As(unit) / y.Quantity.As(length.Unit), area.Unit), name, "");
    }

    public static CalcDouble operator /(CalcVolume x, CalcVolume y)
    {
        (string name, string symbol, VolumeUnit unit) = OperatorMetadataHelper(x, y, '/');
        return new CalcDouble(name, symbol, x.Quantity / y.Quantity);
    }

    private static (string name, string symbol, VolumeUnit unit) OperatorMetadataHelper(CalcVolume x, CalcVolume y, char operation)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}{operation}{y.DisplayName}";
        string symbol = x.Symbol == y.Symbol ? x.Symbol : string.Empty;
        VolumeUnit unit = x.Quantity.Unit == y.Quantity.Unit ? x.Quantity.Unit : VolumeUnit.CubicMeter;
        return (name, symbol, unit);
    }
}
