namespace AstroMultimedia.Core.Collections;

/// <summary>
/// Extension methods for IEnumerable[T].
/// </summary>
public static class XDictionary
{
    /// <summary>
    /// Create a new dictionary from the instance with keys and values flipped.
    /// If the values aren't unique, the method will throw an exception.
    /// </summary>
    /// <param name="dict"></param>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public static Dictionary<TValue, TKey> Flip<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        where TKey : notnull
        where TValue : notnull =>
        dict.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
}
