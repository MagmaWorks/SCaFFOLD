using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcChannel : Channel, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcChannel>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcChannel(Length height, Length width, Length webThickness, Length flangeThickness, string name, string symbol = "")
        : base(height, width, webThickness, flangeThickness)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcChannel CreateFromDescription(string descripiton)
    {
        return ProfileDescription.ProfileFromDescription<CalcChannel>(descripiton);
    }
    public static bool TryParse(string s, IFormatProvider provider, out CalcChannel result)
    {
        try
        {
            result = s.FromJson<CalcChannel>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcChannel Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcChannel>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcChannel result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
