using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcTeeProfile : Tee, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcTeeProfile>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcTeeProfile(Length height, Length width, Length flangeThickness, Length webThickness, string name, string symbol = "")
        : base(height, width, flangeThickness, webThickness)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcTeeProfile CreateFromDescription(string descripiton)
    {
        return ProfileDescription.ProfileFromDescription<CalcTeeProfile>(descripiton);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcTeeProfile result)
    {
        try
        {
            result = s.FromJson<CalcTeeProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcTeeProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcTeeProfile>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcTeeProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
