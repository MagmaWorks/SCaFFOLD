using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcDoubleAngle : DoubleAngle, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcDoubleAngle>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcDoubleAngle(Length height, Length width, Length webThickness, Length flangeThickness, Length backToBackDistance, string name, string symbol = "")
        : base(height, width, webThickness, flangeThickness, backToBackDistance)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcDoubleAngle CreateFromDescription(string descripiton)
    {
        return ProfileDescription.ProfileFromDescription<CalcDoubleAngle>(descripiton);
    }
    public static bool TryParse(string s, IFormatProvider provider, out CalcDoubleAngle result)
    {
        try
        {
            result = s.FromJson<CalcDoubleAngle>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcDoubleAngle Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcDoubleAngle>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcDoubleAngle result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
