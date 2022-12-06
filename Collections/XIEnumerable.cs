using System.Collections;

namespace AstroMultimedia.Core.Collections;

/// <summary>
/// Extension methods for List.
/// </summary>
public static class XIEnumerable
{
    public static bool IsEmpty<T>(this IEnumerable<T> list) =>
        list switch
        {
            null => throw new ArgumentNullException(nameof(list)),
            ICollection<T> genericCollection => genericCollection.Count == 0,
            ICollection nonGenericCollection => nonGenericCollection.Count == 0,
            _ => !list.Any()
        };

    /// <summary>
    /// Return list1 with values from list2 removed.
    /// Supports duplicates, so this is not the same as set difference.
    /// For example, if list1 has 2 instances of "cat" and list2 has one instance of "cat", the
    /// result will have one instance of "cat".
    /// </summary>
    /// <param name="list1"></param>
    /// <param name="list2"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
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
    /// Convert an IEnumerable<T> into a dictionary with the dictionary's keys set to zero-based
    /// index, same as an array.
    /// </summary>
    public static Dictionary<int, T> ToDictionary<T>(this IEnumerable<T> list)
    {
        int index = 0;
        return new Dictionary<int, T>(list
            .Select(item => new KeyValuePair<int, T>(index++, item)));
    }
}
