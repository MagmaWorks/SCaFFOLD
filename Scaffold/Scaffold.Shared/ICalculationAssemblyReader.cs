namespace Scaffold.Console;

public interface ICalculationAssemblyReader 
{
    CalculationPackage Get(Stream fileStream);
}