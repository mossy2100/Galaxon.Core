namespace AstroMultimedia.Core.Collections;

/// <summary>
/// Extension methods for Dictionary.
/// </summary>
public static class XDictionary
{
    /// <summary>
    /// Create a new dictionary from the instance with keys and values flipped.
    /// If the values aren't unique, the method will throw an exception.
    /// </summary>
    public static Dictionary<TValue, TKey> Flip<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        where TKey : notnull
        where TValue : notnull =>
        dict.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
}
