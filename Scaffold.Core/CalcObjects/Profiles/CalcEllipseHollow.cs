using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcEllipseHollow : EllipseHollow, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcEllipseHollow>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcEllipseHollow(Length width, Length height, Length thickness, string name, string symbol = "")
        : base(width, height, thickness)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcEllipseHollow CreateFromDescription(string descripiton)
    {
        return ProfileDescription.ProfileFromDescription<CalcEllipseHollow>(descripiton);
    }
    public static bool TryParse(string s, IFormatProvider provider, out CalcEllipseHollow result)
    {
        try
        {
            result = s.FromJson<CalcEllipseHollow>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcEllipseHollow Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcEllipseHollow>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcEllipseHollow result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
