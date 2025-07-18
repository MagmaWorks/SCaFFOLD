using MagmaWorks.Taxonomy.Loads;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Loads;
public sealed class CalcDesignSituation : DesignSituation, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcDesignSituation>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcDesignSituation(string name, string symbol = "")
        : base()
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcDesignSituation result)
    {
        try
        {
            result = s.FromJson<CalcDesignSituation>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcDesignSituation Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcDesignSituation>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcDesignSituation result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
