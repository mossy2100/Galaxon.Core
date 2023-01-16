namespace Galaxon.Core.Numbers;

/// <summary>Extension methods for ulong.</summary>
public static class XUlong
{
    /// <summary>
    /// Get a random ulong.
    /// </summary>
    public static ulong GetRandom() =>
        (ulong)XInt.GetRandom();

    #region Extension methods for IEnumerable<ulong>

    /// <summary>
    /// Get the sum of all values in the collection.
    /// </summary>
    public static ulong Sum(this IEnumerable<ulong> source) =>
        source.Aggregate(0ul, (sum, value) => sum + value);

    /// <summary>
    /// Get the sum of all values in the collection, transformed by the supplied function.
    /// </summary>
    public static ulong Sum(this IEnumerable<ulong> source, Func<ulong, ulong> func) =>
        source.Aggregate(0ul, (sum, value) => sum + func(value));

    #endregion Extension methods for IEnumerable<ulong>
}
