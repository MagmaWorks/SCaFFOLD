﻿namespace Scaffold.Core.Internals;

internal static class InternalExtensionMethods
{
    private static string UniqueDisplayName(List<ICalcValue> collection, string displayName)
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

    internal static void InsertCalcValue(this List<ICalcValue> collection, ICalcValue calcValue)
    {
        calcValue.DisplayName = UniqueDisplayName(collection, calcValue.DisplayName);
        collection.Add(calcValue);
    }
}
