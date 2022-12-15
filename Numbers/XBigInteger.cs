using System.Numerics;

namespace AstroMultimedia.Core.Numbers;

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
    /// Returns an integer that indicates the sign of the BigInteger.
    ///   -1 if the value is less than 0
    ///   0 if the value is == 0
    ///   1 if the value is greater than 0
    /// Same as Sign() method for int, double, etc.
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public static BigInteger Sign(BigInteger n) =>
        BigInteger.CopySign(1, n);
}
