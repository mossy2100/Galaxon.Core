namespace AstroMultimedia.Core.Numbers;

public static class XUInt64
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
}
