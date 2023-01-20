using System.Numerics;

namespace Galaxon.Core.Numbers;

/// <summary>Extension methods for BigInteger.</summary>
public static class XBigInteger
{
    #region Fields

    /// <summary>
    /// Cache for GreatestCommonDivisor().
    /// </summary>
    private static readonly Dictionary<string, BigInteger> s_gcdCache = new ();

    #endregion Fields

    #region Digit-related methods

    /// <summary>
    /// Reverse a BigInteger.
    /// e.g. 123 becomes 321.
    /// </summary>
    public static BigInteger Reverse(this BigInteger n) =>
        BigInteger.Parse(n.ToString().Reverse().ToArray());

    /// <summary>
    /// Check if a number is palindromic.
    /// </summary>
    public static bool IsPalindromic(this BigInteger n) =>
        n == n.Reverse();

    /// <summary>
    /// Sum of the digits in a BigInteger.
    /// If present, a negative sign is ignored.
    /// </summary>
    public static BigInteger DigitSum(this BigInteger n) =>
        BigInteger.Abs(n).ToString().Sum(c => c - '0');

    /// <summary>
    /// Get the number of digits in the BigInteger.
    /// The result will be the same for a positive or negative value.
    /// I tried doing this with double.Log() but because double is imprecise it gives wrong results
    /// for values close to but less than powers of 10.
    /// </summary>
    public static int NumDigits(this BigInteger n) =>
        BigInteger.Abs(n).ToString().Length;

    #endregion Digit-related methods

    #region Miscellaneous other methods

    /// <summary>
    /// Get the unsigned, 2s-complement version of the value, containing the fewest number of bytes.
    /// </summary>
    public static BigInteger ToUnsigned(this BigInteger n)
    {
        // Check if anything to do.
        if (n >= 0)
        {
            return n;
        }

        // Get the bytes.
        List<byte> bytes = n.ToByteArray().ToList();

        // Check the most-significant bit, and, if it's 1, add a zero byte to ensure the bytes are
        // interpreted as a positive value when constructing the result BigInteger.
        if ((bytes[^1] & 0b10000000) != 0)
        {
            bytes.Add(0);
        }

        // Construct a new unsigned value.
        return new BigInteger(bytes.ToArray());
    }

    #endregion Miscellaneous other methods

    #region Methods relating to greatest common divisor

    public static BigInteger LeastCommonMultiple(BigInteger a, BigInteger b)
    {
        // Special case.
        if (a == 0 || b == 0)
        {
            return 0;
        }

        return a * (b / GreatestCommonDivisor(a, b));
    }

    /// <summary>
    /// Determine the greatest common divisor of two integers.
    /// Synonyms: greatest common factor, highest common factor.
    /// </summary>
    public static BigInteger GreatestCommonDivisor(BigInteger a, BigInteger b)
    {
        // Make a and b non-negative, since the result will be the same for negative values.
        a = BigInteger.Abs(a);
        b = BigInteger.Abs(b);

        // Make a < b, to reduce the cache size by half and simplify terminating conditions.
        if (a > b)
        {
            (a, b) = (b, a);
        }

        // Optimization/terminating conditions.
        if (a == b || a == 0)
        {
            return b;
        }
        if (a == 1)
        {
            return 1;
        }

        BigInteger gcd;

        // Check the cache.
        string key = $"{a}/{b}";
        if (s_gcdCache.TryGetValue(key, out gcd))
        {
            return gcd;
        }

        // Get the result by recursion.
        gcd = GreatestCommonDivisor(a, b % a);

        // Store the result in the cache.
        s_gcdCache[key] = gcd;

        return gcd;
    }

    #endregion Methods relating to greatest common divisor

    #region Extension methods for IEnumerable<BigInteger>

    /// <summary>
    /// Get the sum of all values in the collection.
    /// </summary>
    public static BigInteger Sum(this IEnumerable<BigInteger> nums) =>
        nums.Aggregate((BigInteger)0, (sum, num) => sum + num);

    /// <summary>
    /// Get the sum of all values in the collection, transformed by the supplied function.
    /// </summary>
    public static BigInteger Sum(this IEnumerable<BigInteger> source,
        Func<BigInteger, BigInteger> func) =>
        source.Aggregate((BigInteger)0, (sum, value) => sum + func(value));

    #endregion Extension methods for IEnumerable<BigInteger>
}
