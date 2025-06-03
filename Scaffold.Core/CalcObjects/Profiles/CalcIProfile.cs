using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcIProfile : I, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcIProfile>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcIProfile(Length height, Length width, Length flangeThickness, Length webThickness, string name, string symbol = "")
        : base(height, width, flangeThickness, webThickness)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcIProfile CreateFromDescription(string descripiton)
    {
        return ProfileDescription.ProfileFromDescription<CalcIProfile>(descripiton);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcIProfile result)
    {
        try
        {
            result = s.FromJson<CalcIProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcIProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcIProfile>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcIProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
