using System.Data;

namespace AstroMultimedia.Core.Exceptions;

/// <summary>
/// This exception is often needed when working with databases, and a bit more informative than the
/// base DataException.
/// </summary>
public class DataNotFoundException : DataException
{
    /// <summary>
    /// Constructor.
    /// All arguments are optional.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public DataNotFoundException(
        string? message = null,
        Exception? innerException = null
    ) : base(message, innerException)
    {
    }
}
