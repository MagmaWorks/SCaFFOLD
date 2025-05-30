using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcCustomI : CustomI, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcCustomI>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcCustomI(Length height, Length topFlangeWidth, Length bottomFlangeWidth, Length topFlangeThickness, Length bottomFlangeThickness, Length webThickness, string name, string symbol = "")
        : base(height, topFlangeWidth, bottomFlangeWidth, topFlangeThickness, bottomFlangeThickness, webThickness)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcCustomI CreateFromDescription(string descripiton)
    {
        return ProfileDescription.ProfileFromDescription<CalcCustomI>(descripiton);
    }
    public static bool TryParse(string s, IFormatProvider provider, out CalcCustomI result)
    {
        try
        {
            result = s.FromJson<CalcCustomI>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcCustomI Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcCustomI>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcCustomI result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
