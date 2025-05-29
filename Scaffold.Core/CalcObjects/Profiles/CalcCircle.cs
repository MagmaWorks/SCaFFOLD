using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcCircle : Circle, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcCircle>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcCircle(Length diameter, string name, string symbol = "")
        : base(diameter)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcCircle result)
    {
        try
        {
            result = s.FromJson<CalcCircle>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcCircle Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcCircle>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcCircle result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
