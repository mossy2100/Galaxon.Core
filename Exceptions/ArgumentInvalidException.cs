namespace Galaxon.Core.Exceptions;

/// <summary>
/// This exception type is useful for an ArgumentException when the argument is invalid for a
/// reason other than being null, out of range, or having an invalid format.
///
/// I'm using the same parameter order here as used by
/// <see cref="ArgumentNullException" />
/// <see cref="ArgumentOutOfRangeException" />
/// i.e. (paramName, message), for consistency. The base class
/// <see cref="ArgumentException" />
/// has the parameters in a different order, i.e. (message, paramName), which can be confusing.
///
/// I've resolved not to use ArgumentException anymore but to treat it as an abstract base class.
/// This will avoid the issue with the arguments being out of order, and the lack of specificity.
///
/// Normally for any ArgumentException we want to identify the argument causing the exception (which
/// should be done using nameof()), unless there are several, in which case the offending arguments
/// can be identified in the message.
///
/// If you don't want to specify the parameter name, set the paramName argument to null or use named
/// arguments.
/// </summary>
public class ArgumentInvalidException : ArgumentException
{
    /// <summary>
    /// Constructor. All parameters are optional.
    /// </summary>
    /// <param name="paramName">The parameter name.</param>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public ArgumentInvalidException(
        string? paramName = null,
        string? message = null,
        Exception? innerException = null
    ) : base(message, paramName, innerException)
    {
    }
}
