using MagmaWorks.Taxonomy.Loads;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Loads;
public sealed class CalcPointForce : PointForce, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcPointForce>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcPointForce(Force x, Force y, Force z, string name, string symbol = "")
        : base(x, y, z)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcPointForce result)
    {
        try
        {
            result = s.FromJson<CalcPointForce>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcPointForce Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcPointForce>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcPointForce result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
