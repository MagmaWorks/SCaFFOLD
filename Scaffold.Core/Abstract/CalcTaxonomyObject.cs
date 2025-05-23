using MagmaWorks.Taxonomy.Serialization;
namespace Scaffold.Core.Abstract;

public abstract class CalcTaxonomyObject<T> : ICalcValue, ITaxonomySerializable
    where T : ITaxonomySerializable
{
    public string DisplayName { get; set; }
    public string Symbol { get; set; }
    public CalcStatus Status { get; set; } = CalcStatus.None;
    public T Value { get; set; }

    public CalcTaxonomyObject(T typeValue, string name, string symbol)
    {
        Value = typeValue;
        DisplayName = name;
        Symbol = symbol;
    }

    public static implicit operator T(CalcTaxonomyObject<T> value) => value.Value;

    public string ValueAsString() => Value.ToJson();
    public bool TryParse(string strValue)
    {
        try
        {
            Value = strValue.FromJson<T>();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
