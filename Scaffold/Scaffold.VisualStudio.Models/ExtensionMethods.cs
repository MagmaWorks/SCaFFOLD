using System.Reflection;

namespace Scaffold.VisualStudio.Models;

public static class ExtensionMethods
{
    public static List<Type> GetCalculationTypes(this Assembly assembly)
    {
        var matchingTypes = assembly.GetTypes().Where(x => x.GetInterface("Scaffold.Core.Interfaces.ICalculation") != null);
        return matchingTypes.ToList();
    }
}