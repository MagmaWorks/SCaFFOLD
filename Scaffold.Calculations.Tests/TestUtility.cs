// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Scaffold.Calculations.Tests
{
    internal static class TestUtility
    {
        internal static int Precision(this double d)
        {
            return d.ToString().Replace(",", ".").Split('.').LastOrDefault().Length;
        }
    }
}
