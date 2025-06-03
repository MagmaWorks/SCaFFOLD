using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcCustomIProfile : CustomI, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcCustomIProfile>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcCustomIProfile(Length height, Length topFlangeWidth, Length bottomFlangeWidth, Length topFlangeThickness, Length bottomFlangeThickness, Length webThickness, string name, string symbol = "")
        : base(height, topFlangeWidth, bottomFlangeWidth, topFlangeThickness, bottomFlangeThickness, webThickness)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcCustomIProfile CreateFromDescription(string descripiton)
    {
        return ProfileDescription.ProfileFromDescription<CalcCustomIProfile>(descripiton);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcCustomIProfile result)
    {
        try
        {
            result = s.FromJson<CalcCustomIProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcCustomIProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcCustomIProfile>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcCustomIProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
