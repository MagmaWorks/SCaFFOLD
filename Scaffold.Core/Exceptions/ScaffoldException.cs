namespace Scaffold.Core.Exceptions;
public class ScaffoldException : Exception
{
    /// <summary>
    /// Inherit from this method to throw Scaffold Exceptions to be caught by the main programming.
    /// </summary>
    /// <param name="message"></param>
    public ScaffoldException(string message) : base(message) { }
}
