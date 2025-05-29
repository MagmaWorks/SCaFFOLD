using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcDoubleChannel : DoubleChannel, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcDoubleChannel>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcDoubleChannel(Length height, Length width, Length webThickness, Length flangeThickness, Length backToBackDistance, string name, string symbol = "")
        : base(height, width, webThickness, flangeThickness, backToBackDistance)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcDoubleChannel result)
    {
        try
        {
            result = s.FromJson<CalcDoubleChannel>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcDoubleChannel Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcDoubleChannel>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcDoubleChannel result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
