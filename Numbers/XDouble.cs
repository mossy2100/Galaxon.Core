namespace AstroMultimedia.Core.Numbers;

/// <summary>
/// Extension methods for Double.
/// </summary>
public static class XDouble
{
    /// <summary>
    /// The default maximum difference between 2 double values being compared for equality.
    /// </summary>
    public const double Delta = 1e-9;

    #region Methods for rounding

    /// <summary>
    /// Round off a value to a given number of significant figures.
    /// </summary>
    /// <param name="d">The number to round.</param>
    /// <param name="digits">The number of significant figures.</param>
    /// <returns>The rounded number.</returns>
    /// TODO TEST. If digits is too high, an exception will be thrown, so this needs to be checked.
    public static double RoundSigFigs(this double d, int digits)
    {
        if (d == 0)
        {
            return 0;
        }

        double scale = Pow(10, Floor(Log10(Abs(d))) + 1);
        return scale * Round(d / scale, digits);
    }

    #endregion Methods for rounding

    #region Methods for checking if doubles are integers

    /// <summary>
    /// Check if a double is a positive integer.
    /// </summary>
    public static bool IsPositiveInteger(double d) =>
        (d > 0) && double.IsInteger(d);

    /// <summary>
    /// Check if a double is a negative integer.
    /// </summary>
    public static bool IsNegativeInteger(double d) =>
        (d < 0) && double.IsInteger(d);

    /// <summary>
    /// Check if a value is a perfect square.
    /// </summary>
    public static bool IsPerfectSquare(double d) =>
        double.IsPositive(d) && double.IsInteger(Sqrt(d));

    #endregion Methods for checking if doubles are integers

    #region Methods for fuzzy equals

    /// <summary>
    /// Check if 2 double values are equal for practical purposes.
    ///
    /// If two double values differ only by the least significant bit, this is more likely
    /// due to inaccuracies in floating point representations than actual inequality.
    ///
    /// This code is copied/adapted from Google Guava DoubleMath.fuzzyEquals().
    /// <see href="https://github.com/google/guava/blob/master/guava/src/com/google/common/math/DoubleMath.java#L360" />
    ///
    /// I initially tried the algorithm from the Microsoft documentation, it didn't work in all cases.
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.double.equals?view=net-7.0#system-double-equals(system-double)" />
    /// </summary>
    /// <param name="a">First number.</param>
    /// <param name="b">Second number.</param>
    /// <param name="tolerance">The maximum allowable difference between them.</param>
    /// <returns>If close enough to equal.</returns>
    public static bool FuzzyEquals(this double a, double b, double tolerance = Delta)
    {
        // Ensure tolerance is non-negative.
        if (tolerance < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(tolerance), "Cannot be negative.");
        }

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        // Handle NaN separately so the method behaves the same as double.Equals().
        // The equality operator will return false when comparing 2 NaN values.
        // The equality operator will return true when comparing infinities.
        return (double.IsNaN(a) && double.IsNaN(b)) || (a == b) || Abs(a - b) <= tolerance;
    }

    /// <summary>
    /// Compare two nullable doubles for fuzzy equality.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="tolerance"></param>
    /// <returns></returns>
    public static bool FuzzyEquals(this double? a, double? b, double tolerance = Delta)
    {
        // If they're both null then they're equal.
        if (!a.HasValue && !b.HasValue)
        {
            return true;
        }

        // If only one of them is null they're unequal.
        if (!a.HasValue || !b.HasValue)
        {
            return false;
        }

        // Check for fuzzy equality.
        return a.Value.FuzzyEquals(b.Value, tolerance);
    }

    /// <summary>
    /// IsInteger() can be a bit strict. This method allows for some fuzziness.
    /// </summary>
    public static bool FuzzyIsInteger(double d, double tolerance = Delta) =>
        d.FuzzyEquals(Round(d), tolerance);

    /// <summary>
    /// Check if a double is a positive integer, with some fuzziness.
    /// </summary>
    public static bool FuzzyIsPositiveInteger(double d, double tolerance = Delta) =>
        (d > 0) && FuzzyIsInteger(d, tolerance);

    /// <summary>
    /// Check if a double is a negative integer, with some fuzziness.
    /// </summary>
    public static bool FuzzyIsNegativeInteger(double d, double tolerance = Delta) =>
        (d < 0) && FuzzyIsInteger(d, tolerance);

    #endregion Methods for fuzzy equals

    #region Methods for IEnumerable<double>

    /// <summary>
    /// Similar to Sum(), this extension method generates the product of all values in a collection
    /// of doubles.
    /// I'd love to make this method generic but I haven't figured out how yet.
    /// </summary>
    public static double Product(this IEnumerable<double> source) =>
        source.Aggregate(1.0, (prod, value) => prod * value);

    /// <summary>
    /// Get a product of all values in the collection, transformed by the supplied function.
    /// </summary>
    public static double Product(this IEnumerable<double> source, Func<double, double> func) =>
        source.Aggregate(1.0, (prod, value) => prod * func(value));

    #endregion Methods for IEnumerable<double>
}
