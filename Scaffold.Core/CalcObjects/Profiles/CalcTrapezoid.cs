using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcTrapezoid : Trapezoid, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcTrapezoid>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcTrapezoid(Length topWidth, Length bottomWidth, Length height, string name, string symbol = "")
        : base(topWidth, bottomWidth, height)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcTrapezoid CreateFromDescription(string descripiton)
    {
        return ProfileDescription.ProfileFromDescription<CalcTrapezoid>(descripiton);
    }
    public static bool TryParse(string s, IFormatProvider provider, out CalcTrapezoid result)
    {
        try
        {
            result = s.FromJson<CalcTrapezoid>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcTrapezoid Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcTrapezoid>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcTrapezoid result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
