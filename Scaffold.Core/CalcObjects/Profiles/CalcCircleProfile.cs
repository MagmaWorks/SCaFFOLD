using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcCircleProfile : Circle, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcCircleProfile>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcCircleProfile(Length diameter, string name, string symbol = "")
        : base(diameter)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcCircleProfile(double diameter, LengthUnit unit, string name, string symbol = "")
        : base(new Length(diameter, unit))
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcCircleProfile CreateFromDescription(string descripiton)
    {
        return ProfileDescription.ProfileFromDescription<CalcCircleProfile>(descripiton);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcCircleProfile result)
    {
        try
        {
            result = s.FromJson<CalcCircleProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcCircleProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcCircleProfile>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcCircleProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
