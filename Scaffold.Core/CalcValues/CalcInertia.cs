using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcValues;

public sealed class CalcInertia : CalcQuantity<AreaMomentOfInertia>
{
    public CalcInertia(AreaMomentOfInertia AreaMomentOfInertia, string name, string symbol = "")
        : base(AreaMomentOfInertia, name, symbol) { }

    public CalcInertia(double value, AreaMomentOfInertiaUnit unit, string name, string symbol)
        : base(new AreaMomentOfInertia(value, unit), name, symbol) { }


    public static implicit operator double(CalcInertia value) => value.Value;
    public static implicit operator AreaMomentOfInertia(CalcInertia value) => value.Quantity;

    public static CalcInertia operator +(CalcInertia x, CalcInertia y)
    {
        (string name, string symbol, AreaMomentOfInertiaUnit unit) = OperatorMetadataHelper(x, y, '+');
        return new CalcInertia(new AreaMomentOfInertia(x.Value + y.Value, unit), name, symbol);
    }

    public static CalcInertia operator -(CalcInertia x, CalcInertia y)
    {
        (string name, string symbol, AreaMomentOfInertiaUnit unit) = OperatorMetadataHelper(x, y, '-');
        return new CalcInertia(new AreaMomentOfInertia(x.Value - y.Value, unit), name, symbol);
    }

    public static CalcVolume operator /(CalcInertia x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}/{y.DisplayName}";
        AreaMomentOfInertiaUnit unit = x.Quantity.Unit;
        Length.TryParse($"0 {AreaMomentOfInertia.GetAbbreviation(unit).Replace("⁴", string.Empty)}", out Length length);
        Volume.TryParse($"0 {AreaMomentOfInertia.GetAbbreviation(unit).Replace("⁴", "³")}", out Volume vol);
        return new CalcVolume(new Volume(x.Quantity.As(unit) / y.Quantity.As(length.Unit), vol.Unit), name, "");
    }

    public static CalcArea operator /(CalcInertia x, CalcArea y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}/{y.DisplayName}";
        AreaMomentOfInertiaUnit unit = x.Quantity.Unit;
        Area.TryParse($"0 {AreaMomentOfInertia.GetAbbreviation(unit).Replace("⁴", "²")}", out Area area);
        return new CalcArea(new Area(x.Quantity.As(unit) / y.Quantity.As(area.Unit), area.Unit), name, "");
    }

    public static CalcLength operator /(CalcInertia x, CalcVolume y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}/{y.DisplayName}";
        AreaMomentOfInertiaUnit unit = x.Quantity.Unit;
        Length.TryParse($"0 {AreaMomentOfInertia.GetAbbreviation(unit).Replace("⁴", string.Empty)}", out Length length);
        Volume.TryParse($"0 {AreaMomentOfInertia.GetAbbreviation(unit).Replace("⁴", "³")}", out Volume vol);
        return new CalcLength(new Length(x.Quantity.As(unit) / y.Quantity.As(vol.Unit), length.Unit), name, "");
    }

    public static CalcDouble operator /(CalcInertia x, CalcInertia y)
    {
        (string name, string symbol, AreaMomentOfInertiaUnit unit) = OperatorMetadataHelper(x, y, '/');
        return new CalcDouble(name, symbol, x.Quantity / y.Quantity);
    }

    private static (string name, string symbol, AreaMomentOfInertiaUnit unit) OperatorMetadataHelper(CalcInertia x, CalcInertia y, char operation)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}{operation}{y.DisplayName}";
        string symbol = x.Symbol == y.Symbol ? x.Symbol : string.Empty;
        AreaMomentOfInertiaUnit unit = x.Quantity.Unit == y.Quantity.Unit ? x.Quantity.Unit : AreaMomentOfInertiaUnit.MeterToTheFourth;
        return (name, symbol, unit);
    }
}
