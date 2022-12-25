namespace Galaxon.Core.Numbers;

public static class XUlong
{
    #region Methods for IEnumerable<ulong>

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

    /// <summary>
    /// Get the product of all values in the collection.
    /// </summary>
    public static ulong Product(this IEnumerable<ulong> source) =>
        source.Aggregate(1ul, (prod, value) => prod * value);

    /// <summary>
    /// Get the product of all values in the collection, transformed by the supplied function.
    /// </summary>
    public static ulong Product(this IEnumerable<ulong> source, Func<ulong, ulong> func) =>
        source.Aggregate(1ul, (prod, value) => prod * func(value));

    #endregion Methods for IEnumerable<ulong>

    /// <summary>
    /// Return the absolute value of a long as a ulong.
    /// This addresses an issue with long.Abs(), which is that Abs(long.MinValue) can't be expressed
    /// as a long, and so wrap-around occurs.
    /// Unsigned integer types don't have an Abs() method, so there's no collision.
    /// </summary>
    /// <param name="n">A long value.</param>
    /// <returns>The absolute value as a ulong.</returns>
    public static ulong Abs(long n) =>
        n switch
        {
            long.MinValue => long.MaxValue + 1ul,
            >= 0 => (ulong)n,
            _ => (ulong)-n
        };
}
