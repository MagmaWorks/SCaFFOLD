using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcRectangle : Rectangle, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcRectangle>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcRectangle(Length width, Length height, string name, string symbol = "")
        : base(width, height)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcRectangle CreateFromDescription(string descripiton)
    {
        return ProfileDescription.ProfileFromDescription<CalcRectangle>(descripiton);
    }
    public static bool TryParse(string s, IFormatProvider provider, out CalcRectangle result)
    {
        try
        {
            result = s.FromJson<CalcRectangle>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcRectangle Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcRectangle>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcRectangle result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
