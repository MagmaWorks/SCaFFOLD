using System.Reflection;
using Scaffold.Core.Interfaces;

namespace Scaffold.VisualStudio.Calculator;

public static class ExtensionMethods
{
    public static List<Type> GetCalculationTypes(this Assembly assembly)
    {
        var types = assembly.GetTypes();
        var matchingTypes = types.Where(x => x.GetInterface(nameof(ICalculation)) != null);
        return matchingTypes.ToList();
    }
}