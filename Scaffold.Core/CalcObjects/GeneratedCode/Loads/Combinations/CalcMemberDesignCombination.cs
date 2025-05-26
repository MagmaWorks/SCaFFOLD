using MagmaWorks.Taxonomy.Loads.Combinations;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Loads.Combinations;
public sealed class CalcMemberDesignCombination : CalcTaxonomyObject<MemberDesignCombination>
#if NET7_0_OR_GREATER
    , IParsable<CalcMemberDesignCombination>
#endif
{
    public CalcMemberDesignCombination(MemberDesignCombination memberdesigncombination, string name, string symbol = "")
        : base(memberdesigncombination, name, symbol) { }

    public CalcMemberDesignCombination(string name, string symbol = "")
        : base(new MemberDesignCombination(), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcMemberDesignCombination result)
    {
        try
        {
            result = s.FromJson<CalcMemberDesignCombination>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcMemberDesignCombination Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcMemberDesignCombination>();
    }
}
