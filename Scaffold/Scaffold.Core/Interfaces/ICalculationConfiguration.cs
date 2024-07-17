using Scaffold.Core.Models;

namespace Scaffold.Core.Interfaces;

public interface ICalculationConfiguration<T> where T : class, ICalculation
{
    void Configure(CalculationConfigurationBuilder<T> builder);
}