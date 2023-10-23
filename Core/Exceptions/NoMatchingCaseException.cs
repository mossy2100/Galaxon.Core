namespace Galaxon.Core.Exceptions;

/// <summary>
/// This exception type is similar to ArgumentOutOfRangeException, except intended for use in switch
/// statements and expressions, when the value being tested matches none of the case patterns.
/// </summary>
public class NoMatchingCaseException : Exception
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public NoMatchingCaseException()
    {
    }

    /// <summary>
    /// Initializes a new instance with an error message, and (optionally) a reference to the inner
    /// exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public NoMatchingCaseException(string? message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
