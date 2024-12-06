using Scaffold.Core.Enums;

namespace Scaffold.Core.Interfaces;

public interface ICalculationStatus
{
    CalcStatus Status { get; }
}
