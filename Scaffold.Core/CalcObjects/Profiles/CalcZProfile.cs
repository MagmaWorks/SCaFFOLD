using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcZProfile : Z, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcZProfile>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcZProfile(Length height, Length topFlangeWidth, Length bottomFlangeWidth, Length thickness, Length topLip, Length bottomLip, string name, string symbol = "")
        : base(height, topFlangeWidth, bottomFlangeWidth, thickness, topLip, bottomLip)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcZProfile CreateFromDescription(string descripiton)
    {
        return ProfileDescription.ProfileFromDescription<CalcZProfile>(descripiton);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcZProfile result)
    {
        try
        {
            result = s.FromJson<CalcZProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcZProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcZProfile>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcZProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
