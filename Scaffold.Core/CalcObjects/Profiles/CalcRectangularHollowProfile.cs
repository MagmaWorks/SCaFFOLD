using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcRectangularHollowProfile : RectangularHollow, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcRectangularHollowProfile>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcRectangularHollowProfile(Length width, Length height, Length thickness, string name, string symbol = "")
        : base(width, height, thickness)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcRectangularHollowProfile(double width, double height, double thickness, LengthUnit unit, string name, string symbol = "")
        : base(new Length(width, unit), new Length(height, unit), new Length(thickness, unit))
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcRectangularHollowProfile CreateFromDescription(string descripiton)
    {
        return ProfileDescription.ProfileFromDescription<CalcRectangularHollowProfile>(descripiton);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcRectangularHollowProfile result)
    {
        try
        {
            result = s.FromJson<CalcRectangularHollowProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcRectangularHollowProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcRectangularHollowProfile>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcRectangularHollowProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
