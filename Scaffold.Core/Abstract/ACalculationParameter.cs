using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.Abstract;

public abstract class ACalculationParameter<T> : ICalculationParameter<T>
{
    public string DisplayName { get; set; }
    public T Value { get; set; }
    public virtual string Symbol { get; set; } = string.Empty;
    public virtual string Unit { get; set; } = string.Empty;
    public abstract bool TryParse(string strValue);
    public string ValueAsString() => $"{Value}{Unit}";

    protected ACalculationParameter(string name)
    {
        DisplayName = name;
    }
}
