using Galaxon.Core.Exceptions;

namespace Galaxon.Core.Collections;

/// <summary>
/// Extension methods for Dictionary.
/// </summary>
public static class XDictionary
{
    /// <summary>
    /// Create a new dictionary from the instance with keys and values flipped.
    /// </summary>
    /// <param name="dict">The instance.</param>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <returns>The flipped dictionary.</returns>
    /// <exception cref="ArgumentInvalidException">
    /// If the instance contains duplicate values.
    /// </exception>
    public static Dictionary<TValue, TKey> Flip<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        where TKey : notnull
        where TValue : notnull
    {
        try
        {
            return dict.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentInvalidException(nameof(dict),
                "Cannot flip a dictionary with duplicate values.", ex);
        }
    }
}
