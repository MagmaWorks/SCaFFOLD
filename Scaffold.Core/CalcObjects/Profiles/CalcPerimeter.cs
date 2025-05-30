using MagmaWorks.Geometry;
using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcPerimeter : Perimeter, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcPerimeter>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcPerimeter(ILocalPolyline2d edge, string name, string symbol = "")
        : base(edge)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcPerimeter(IList<ILocalPoint2d> edgePoints, string name, string symbol = "")
        : base(edgePoints)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcPerimeter(ILocalPolyline2d outerEdge, IList<ILocalPolyline2d> voidEdges, string name, string symbol = "")
        : base(outerEdge, voidEdges)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcPerimeter(IProfile profile, string name, string symbol = "")
        : base(profile)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcPerimeter(IProfile profile, Length tolerance, string name, string symbol = "")
        : base(profile, tolerance)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcPerimeter(IProfile profile, int divisions, string name, string symbol = "")
        : base(profile, divisions)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcPerimeter CreateFromDescription(string descripiton)
    {
        return ProfileDescription.ProfileFromDescription<CalcPerimeter>(descripiton);
    }
    public static bool TryParse(string s, IFormatProvider provider, out CalcPerimeter result)
    {
        try
        {
            result = s.FromJson<CalcPerimeter>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcPerimeter Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcPerimeter>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcPerimeter result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
