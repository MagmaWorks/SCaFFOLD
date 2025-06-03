using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcTrapezoidProfile : Trapezoid, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcTrapezoidProfile>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcTrapezoidProfile(Length topWidth, Length bottomWidth, Length height, string name, string symbol = "")
        : base(topWidth, bottomWidth, height)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcTrapezoidProfile(double topWidth, double bottomWidth, double height, LengthUnit unit, string name, string symbol = "")
        : base(new Length(topWidth, unit), new Length(bottomWidth, unit), new Length(height, unit))
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcTrapezoidProfile CreateFromDescription(string descripiton)
    {
        return ProfileDescription.ProfileFromDescription<CalcTrapezoidProfile>(descripiton);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcTrapezoidProfile result)
    {
        try
        {
            result = s.FromJson<CalcTrapezoidProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcTrapezoidProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcTrapezoidProfile>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcTrapezoidProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
