using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Attributes;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.Abstract;

public abstract class CalculationObjectInput<T> : ICalcObjectInput<T>, ITaxonomySerializable where T : ICalcValue
{
    public string CalculationName { get; set; } = _calculationName;
    public string ReferenceName { get; set; }
    public string DisplayName { get; set; } = _calculationName;
    public string Symbol { get; set; } = string.Concat(_calculationName.Where(c => c >= 'A' && c <= 'Z'));
    public CalcStatus Status { get; set; } = CalcStatus.None;

    private static string _calculationName = typeof(T).Name.SplitPascalCaseToString();

    [OutputCalcValue]
    public T Output
    {
        get { return GetOutput(); }
        set { SetOutput(value); }
    }

    public static implicit operator T(CalculationObjectInput<T> value) => value.Output;
    public virtual void Calculate() { }
    protected abstract T GetOutput();
    public virtual IList<IFormula> GetFormulae() => new List<IFormula>();
    protected virtual void SetOutput(T value)
    {
        Output = value;
    }

    public bool TryParse(string strValue)
    {
        try
        {
            var obj = strValue.FromJson<CalculationObjectInput<T>>();
            Output = obj.Output;
            if (obj.Symbol != null)
            {
                Symbol = obj.Symbol;
            }

            if (obj.DisplayName != null)
            {
                DisplayName = obj.DisplayName;
            }

            if (obj.ReferenceName != null)
            {
                ReferenceName = obj.ReferenceName;
            }

            Status = obj.Status;
            return true;
        }
        catch
        {
            return false;
        }
    }
    public string ValueAsString() => this.ToJson();
}
