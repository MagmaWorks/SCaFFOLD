using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcRectangleProfile : Rectangle, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcRectangleProfile>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcRectangleProfile(Length width, Length height, string name, string symbol = "")
        : base(width, height)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcRectangleProfile CreateFromDescription(string descripiton)
    {
        return ProfileDescription.ProfileFromDescription<CalcRectangleProfile>(descripiton);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcRectangleProfile result)
    {
        try
        {
            result = s.FromJson<CalcRectangleProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcRectangleProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcRectangleProfile>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcRectangleProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
