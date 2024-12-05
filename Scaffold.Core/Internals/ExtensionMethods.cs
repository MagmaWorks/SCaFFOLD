using Scaffold.Core.Interfaces;

namespace Scaffold.Core.Internals;

internal static class InternalExtensionMethods
{
    private static string UniqueDisplayName<T>(List<ICalculationParameter<T>> collection, string displayName)
    {
        var rootName = displayName;
        var i = 0;

        while (collection.FirstOrDefault(x => x.DisplayName == displayName) != null)
        {
            i++;
            displayName = $"{rootName} ({i})";
        }

        return displayName;
    }

    internal static void InsertCalcValue<T>(this List<ICalculationParameter<T>> collection, ICalculationParameter<T> calcValue)
    {
        calcValue.DisplayName = UniqueDisplayName(collection, calcValue.DisplayName);
        collection.Add(calcValue);
    }
}
