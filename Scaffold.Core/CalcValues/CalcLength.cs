using OasysUnits;
using OasysUnits.Units;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcValues;

public sealed class CalcLength : CalcQuantity<Length>
{
    public CalcLength(Length length, string name, string symbol = "")
        : base(length, name, symbol) { }

    public CalcLength(double value, LengthUnit unit, string name, string symbol)
        : base(new Length(value, unit), name, symbol) { }


    public static implicit operator double(CalcLength value) => value.Value;
    public static implicit operator Length(CalcLength value) => value.Quantity;

    public static CalcLength operator +(CalcLength x, CalcLength y)
    {
        (string name, string symbol, LengthUnit unit) = OperatorMetadataHelper(x, y, '+');
        return new CalcLength(new Length(x.Quantity.As(unit) + y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcLength operator -(CalcLength x, CalcLength y)
    {
        (string name, string symbol, LengthUnit unit) = OperatorMetadataHelper(x, y, '-');
        return new CalcLength(new Length(x.Quantity.As(unit) - y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcArea operator *(CalcLength x, CalcLength y)
    {
        (string name, string symbol, LengthUnit unit) = OperatorMetadataHelper(x, y, '·');
        Area.TryParse($"0 {Length.GetAbbreviation(unit)}²", out Area res);
        return new CalcArea(new Area(x.Quantity.As(unit) * y.Quantity.As(unit), res.Unit), name, symbol);
    }

    public static CalcVolume operator *(CalcLength x, CalcArea y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}·{y.DisplayName}";
        LengthUnit unit = x.Quantity.Unit;
        Area.TryParse($"0 {Length.GetAbbreviation(unit)}²", out Area area);
        Volume.TryParse($"0 {Length.GetAbbreviation(unit)}³", out Volume vol);
        return new CalcVolume(new Volume(x.Quantity.As(unit) * y.Quantity.As(area.Unit), vol.Unit), name, "");
    }

    public static CalcInertia operator *(CalcLength x, CalcVolume y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}·{y.DisplayName}";
        LengthUnit unit = x.Quantity.Unit;
        AreaMomentOfInertia.TryParse($"0 {Length.GetAbbreviation(unit)}⁴", out AreaMomentOfInertia res);
        Volume.TryParse($"0 {Length.GetAbbreviation(unit)}³", out Volume vol);
        return new CalcInertia(new AreaMomentOfInertia(x.Quantity.As(unit) * y.Quantity.As(vol.Unit), res.Unit), name, "");
    }

    public static CalcDouble operator /(CalcLength x, CalcLength y)
    {
        (string name, string _, LengthUnit unit) = OperatorMetadataHelper(x, y, '/');
        return new CalcDouble(name, string.Empty, x.Quantity / y.Quantity);
    }

    private static (string name, string symbol, LengthUnit unit) OperatorMetadataHelper(CalcLength x, CalcLength y, char operation)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}{operation}{y.DisplayName}";
        string symbol = x.Symbol == y.Symbol ? x.Symbol : string.Empty;
        LengthUnit unit = x.Quantity.Unit;
        return (name, symbol, unit);
    }
}
