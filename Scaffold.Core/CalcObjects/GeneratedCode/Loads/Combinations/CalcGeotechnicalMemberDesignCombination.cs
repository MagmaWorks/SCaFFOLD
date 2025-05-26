using MagmaWorks.Taxonomy.Loads.Combinations;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Loads.Combinations;
public sealed class CalcGeotechnicalMemberDesignCombination : CalcTaxonomyObject<GeotechnicalMemberDesignCombination>
#if NET7_0_OR_GREATER
    , IParsable<CalcGeotechnicalMemberDesignCombination>
#endif
{
    public CalcGeotechnicalMemberDesignCombination(GeotechnicalMemberDesignCombination geotechnicalmemberdesigncombination, string name, string symbol = "")
        : base(geotechnicalmemberdesigncombination, name, symbol) { }

    public CalcGeotechnicalMemberDesignCombination(string name, string symbol = "")
        : base(new GeotechnicalMemberDesignCombination(), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcGeotechnicalMemberDesignCombination result)
    {
        try
        {
            result = s.FromJson<CalcGeotechnicalMemberDesignCombination>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcGeotechnicalMemberDesignCombination Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcGeotechnicalMemberDesignCombination>();
    }
}
