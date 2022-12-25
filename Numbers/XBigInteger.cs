using System.Numerics;

namespace Galaxon.Core.Numbers;

/// <summary>
/// Extension methods for BigInteger.
/// </summary>
public static class XBigInteger
{
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
    /// Sum all the values in a list of BigIntegers.
    /// </summary>
    public static BigInteger Sum(this IEnumerable<BigInteger> nums) =>
        nums.Aggregate((BigInteger)0, (sum, num) => sum + num);

    /// <summary>
    /// Get the BigInteger closest to the square root of the given BigInteger.
    /// Will fail for numbers greater than double.MaxValue or less than double.MinValue.
    /// </summary>
    public static BigInteger Sqrt(BigInteger n) =>
        (BigInteger)Round(Math.Sqrt((double)n));

    /// <summary>
    /// Get the number of digits in the BigInteger.
    /// </summary>
    public static int NumDigits(this BigInteger n)
    {
        // Avoid logarithm of 0.
        if (n == 0)
        {
            return 1;
        }

        // Avoid logarithm of a negative number.
        n = BigInteger.Abs(n);

        // Get the logarithm, which will be within 1 of the answer.
        double log = BigInteger.Log10(n);

        // Account for fuzziness in the double representation of the logarithm.
        double floor = Floor(log);
        double round = Round(log);
        double nDigits = floor + (round > floor && log.FuzzyEquals(round) ? 2 : 1);

        return (int)nDigits;
    }
}
