using MagmaWorks.Taxonomy.Loads.Combinations;
using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcObjects.Loads.Combinations;
public sealed class CalcCharacteristicCombination : CalcTaxonomyObject<CharacteristicCombination>
#if NET7_0_OR_GREATER
    , IParsable<CalcCharacteristicCombination>
#endif
{
    public CalcCharacteristicCombination(CharacteristicCombination characteristiccombination, string name, string symbol = "")
        : base(characteristiccombination, name, symbol) { }

    public CalcCharacteristicCombination(string name, string symbol = "")
        : base(new CharacteristicCombination(), name, symbol) { }

    public static bool TryParse(string s, IFormatProvider provider, out CalcCharacteristicCombination result)
    {
        try
        {
            result = s.FromJson<CalcCharacteristicCombination>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcCharacteristicCombination Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcCharacteristicCombination>();
    }
}
