using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;
using Angle = MagmaWorks.Taxonomy.Profiles.Angle;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcAngle : Angle, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcAngle>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcAngle(Length height, Length width, Length webThickness, Length flangeThickness, string name, string symbol = "")
        : base(height, width, webThickness, flangeThickness)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcAngle CreateFromDescription(string descripiton)
    {
        return ProfileDescription.ProfileFromDescription<CalcAngle>(descripiton);
    }
    public static bool TryParse(string s, IFormatProvider provider, out CalcAngle result)
    {
        try
        {
            result = s.FromJson<CalcAngle>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcAngle Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcAngle>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcAngle result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
