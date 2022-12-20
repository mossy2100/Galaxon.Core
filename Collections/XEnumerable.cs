namespace AstroMultimedia.Core.Collections;

/// <summary>
/// Extension methods for IEnumerable and IEnumerable[T].
/// </summary>
public static class XEnumerable
{
    /// <summary>
    /// Return list1 with values from list2 removed.
    /// Supports duplicates, so this is not the same as set difference.
    /// For example, if list1 has 2 instances of "cat" and list2 has one instance of "cat", the
    /// result will have one instance of "cat".
    /// </summary>
    public static IEnumerable<T> Diff<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
    {
        List<T> result = list1.ToList();
        foreach (T item in list2)
        {
            result.Remove(item);
        }
        return result;
    }

    /// <summary>
    /// Convert an IEnumerable[T] into a dictionary with the dictionary's keys set to zero-based
    /// index, same as an array or list. This can be useful when the index is meaningful and you
    /// want to filter on it.
    /// </summary>
    public static Dictionary<int, T> ToDictionary<T>(this IEnumerable<T> list)
    {
        int index = 0;
        return new Dictionary<int, T>(list
            .Select(item => new KeyValuePair<int, T>(index++, item)));
    }
}
