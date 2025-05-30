using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcTee : Tee, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcTee>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcTee(Length height, Length width, Length flangeThickness, Length webThickness, string name, string symbol = "")
        : base(height, width, flangeThickness, webThickness)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcTee CreateFromDescription(string descripiton)
    {
        return ProfileDescription.ProfileFromDescription<CalcTee>(descripiton);
    }
    public static bool TryParse(string s, IFormatProvider provider, out CalcTee result)
    {
        try
        {
            result = s.FromJson<CalcTee>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcTee Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcTee>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcTee result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
