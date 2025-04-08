namespace Scaffold.Core.Exceptions;
public class UnitsNotSameException : Exception
{
    public UnitsNotSameException(string displayName1, string displayName2, string unit1, string unit2) :
    base($"Cannot compare two CalcValues with different units ({displayName1}: {unit1} <> {displayName2}: {unit2})")
    { }
}
