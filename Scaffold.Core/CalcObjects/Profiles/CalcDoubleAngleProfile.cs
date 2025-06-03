using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcDoubleAngleProfile : DoubleAngle, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcDoubleAngleProfile>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcDoubleAngleProfile(Length height, Length width, Length webThickness, Length flangeThickness, Length backToBackDistance, string name, string symbol = "")
        : base(height, width, webThickness, flangeThickness, backToBackDistance)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcDoubleAngleProfile(double height, double width, double webThickness, double flangeThickness, double backToBackDistance, LengthUnit unit, string name, string symbol = "")
        : base(new Length(height, unit), new Length(width, unit), new Length(webThickness, unit), new Length(flangeThickness, unit), new Length(backToBackDistance, unit))
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcDoubleAngleProfile CreateFromDescription(string descripiton)
    {
        return ProfileDescription.ProfileFromDescription<CalcDoubleAngleProfile>(descripiton);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcDoubleAngleProfile result)
    {
        try
        {
            result = s.FromJson<CalcDoubleAngleProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcDoubleAngleProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcDoubleAngleProfile>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcDoubleAngleProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
