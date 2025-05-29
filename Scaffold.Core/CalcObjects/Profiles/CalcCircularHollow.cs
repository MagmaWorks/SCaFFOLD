using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcCircularHollow : CircularHollow, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcCircularHollow>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcCircularHollow(Length diameter, Length thickness, string name, string symbol = "")
        : base(diameter, thickness)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcCircularHollow result)
    {
        try
        {
            result = s.FromJson<CalcCircularHollow>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcCircularHollow Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcCircularHollow>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcCircularHollow result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
