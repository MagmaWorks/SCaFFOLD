using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcRoundedRectangleProfile : RoundedRectangle, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcRoundedRectangleProfile>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcRoundedRectangleProfile(Length width, Length height, Length flatWidth, Length flatHeight, string name, string symbol = "")
        : base(width, height, flatWidth, flatHeight)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcRoundedRectangleProfile CreateFromDescription(string descripiton)
    {
        return ProfileDescription.ProfileFromDescription<CalcRoundedRectangleProfile>(descripiton);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcRoundedRectangleProfile result)
    {
        try
        {
            result = s.FromJson<CalcRoundedRectangleProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcRoundedRectangleProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcRoundedRectangleProfile>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcRoundedRectangleProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
