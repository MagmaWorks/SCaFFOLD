using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcEllipseHollowProfile : EllipseHollow, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcEllipseHollowProfile>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcEllipseHollowProfile(Length width, Length height, Length thickness, string name, string symbol = "")
        : base(width, height, thickness)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcEllipseHollowProfile(double width, double height, double thickness, LengthUnit unit, string name, string symbol = "")
        : base(new Length(width, unit), new Length(height, unit), new Length(thickness, unit))
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcEllipseHollowProfile CreateFromDescription(string descripiton)
    {
        return ProfileDescription.ProfileFromDescription<CalcEllipseHollowProfile>(descripiton);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcEllipseHollowProfile result)
    {
        try
        {
            result = s.FromJson<CalcEllipseHollowProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcEllipseHollowProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcEllipseHollowProfile>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcEllipseHollowProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
