using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcC : C, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcC>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcC(Length height, Length width, Length webThickness, Length flangeThickness, Length lip, string name, string symbol = "")
        : base(height, width, webThickness, flangeThickness, lip)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcC result)
    {
        try
        {
            result = s.FromJson<CalcC>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcC Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcC>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcC result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
