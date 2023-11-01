namespace Galaxon.Core.Collections;

/// <summary>
/// Extension methods for IEnumerable and IEnumerable{T}.
/// </summary>
public static class XEnumerable
{
    /// <summary>
    /// Return list1 with values from list2 removed.
    /// Supports duplicates, so this is not the same as set difference.
    /// For example, if list1 has two instances of "cat" and list2 has one instance of "cat", the
    /// result will have one instance of "cat".
    /// </summary>
    public static IEnumerable<T> Diff<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
    {
        var result = list1.ToList();
        foreach (var item in list2)
        {
            result.Remove(item);
        }
        return result;
    }

    /// <summary>
    /// Convert an IEnumerable{T} into a dictionary with the dictionary's keys set to the index.
    /// This can be useful when the index is meaningful and you want to filter on it.
    /// </summary>
    public static Dictionary<int, T> ToDictionary<T>(this IEnumerable<T> enumerable) =>
        new (enumerable.Select((item, index) =>
            new KeyValuePair<int, T>(index, item)));
}
