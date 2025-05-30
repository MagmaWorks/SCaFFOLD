using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcI : I, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcI>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcI(Length height, Length width, Length flangeThickness, Length webThickness, string name, string symbol = "")
        : base(height, width, flangeThickness, webThickness)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcI CreateFromDescription(string descripiton)
    {
        return ProfileDescription.ProfileFromDescription<CalcI>(descripiton);
    }
    public static bool TryParse(string s, IFormatProvider provider, out CalcI result)
    {
        try
        {
            result = s.FromJson<CalcI>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcI Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcI>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcI result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
