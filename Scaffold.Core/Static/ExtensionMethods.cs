﻿using System.Text.RegularExpressions;

namespace Scaffold.Core.Static;

public static class ExtensionMethods
{
    internal static bool IsAcceptedPrimitive(this Type type)
        => type == typeof(double)
           || type == typeof(int) || type == typeof(long) || type == typeof(short)
           || type == typeof(float) || type == typeof(decimal) || type == typeof(bool);

    internal static string SplitPascalCaseToString(this string pascalCaseStr)
    {
        var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

        string modelWithSpaces = r.Replace(pascalCaseStr, " ");
        return modelWithSpaces;
    }
}
