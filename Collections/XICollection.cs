using System.Collections;

namespace AstroMultimedia.Core.Collections;

/// <summary>
/// Extension methods for ICollection and ICollection[T].
/// </summary>
public static class XICollection
{
    /// <summary>
    /// Check if an ICollection (array, list, etc.) is null or empty.
    /// </summary>
    public static bool IsEmpty(this ICollection? list) =>
        list is null || list.Count == 0;
}
