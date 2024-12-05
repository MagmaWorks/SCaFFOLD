using Scaffold.Core.Enums;

namespace Scaffold.Core.Interfaces;

public interface ICalculationParameter<T>
{
    string DisplayName { get; internal set; }
    string Symbol { get; internal set; }
    string Unit { get; }
    T Value { get; }
    bool TryParse(string strValue);
    string ValueAsString();
}
