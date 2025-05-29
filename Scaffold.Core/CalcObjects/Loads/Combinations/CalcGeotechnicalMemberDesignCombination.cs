using MagmaWorks.Taxonomy.Loads.Combinations;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Loads.Combinations;
public sealed class CalcGeotechnicalMemberDesignCombination : GeotechnicalMemberDesignCombination, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcGeotechnicalMemberDesignCombination>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcGeotechnicalMemberDesignCombination(string name, string symbol = "")
        : base()
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcGeotechnicalMemberDesignCombination result)
    {
        try
        {
            result = s.FromJson<CalcGeotechnicalMemberDesignCombination>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcGeotechnicalMemberDesignCombination Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcGeotechnicalMemberDesignCombination>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcGeotechnicalMemberDesignCombination result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
