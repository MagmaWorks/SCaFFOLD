using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcCruciform : Cruciform, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcCruciform>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcCruciform(Length height, Length width, Length flangeThickness, Length webThickness, string name, string symbol = "")
        : base(height, width, flangeThickness, webThickness)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcCruciform CreateFromDescription(string descripiton)
    {
        return ProfileDescription.ProfileFromDescription<CalcCruciform>(descripiton);
    }
    public static bool TryParse(string s, IFormatProvider provider, out CalcCruciform result)
    {
        try
        {
            result = s.FromJson<CalcCruciform>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcCruciform Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcCruciform>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcCruciform result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
