using System.Collections;

namespace AstroMultimedia.Core.Collections;

/// <summary>
/// Extension methods for ICollection and ICollection[T].
/// </summary>
public static class XICollection
{
    /// <summary>
    /// Check if an ICollection is empty.
    /// </summary>
    public static bool IsEmpty(this ICollection list) =>
        list.Count == 0;

    /// <summary>
    /// Check if an ICollection[T] is empty.
    /// </summary>
    public static bool IsEmpty<T>(this ICollection<T> list) =>
        list.Count == 0;
}
