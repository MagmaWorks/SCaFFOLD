using System.Reflection;

namespace Scaffold.VisualStudio.Models.Main;

public static class ExtensionMethods
{
    public static List<Type> GetCalculationTypes(this Assembly assembly)
    {
        var types = assembly.GetTypes();
        var matchingTypes = types.Where(x => x.GetInterface($"{nameof(Scaffold)}.Core.Interfaces.ICalculation") != null);
        return matchingTypes.ToList();
    }
}