using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcDoubleAngularProfile : DoubleAngle, ICalcProfile<CalcDoubleAngularProfile>, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcDoubleAngularProfile>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcDoubleAngularProfile(Length height, Length width, Length webThickness, Length flangeThickness, Length backToBackDistance, string name, string symbol = "")
        : base(height, width, webThickness, flangeThickness, backToBackDistance)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcDoubleAngularProfile(double height, double width, double webThickness, double flangeThickness, double backToBackDistance, LengthUnit unit, string name, string symbol = "")
        : base(new Length(height, unit), new Length(width, unit), new Length(webThickness, unit), new Length(flangeThickness, unit), new Length(backToBackDistance, unit))
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcDoubleAngularProfile CreateFromDescription(string description)
    {
        return ProfileDescription.ProfileFromDescription<CalcDoubleAngularProfile>(description);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcDoubleAngularProfile result)
    {
        try
        {
            result = s.FromJson<CalcDoubleAngularProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcDoubleAngularProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcDoubleAngularProfile>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcDoubleAngularProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
