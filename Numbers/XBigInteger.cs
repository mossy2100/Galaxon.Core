using System.Numerics;

namespace Galaxon.Core.Numbers;

/// <summary>
/// Extension methods for BigInteger.
/// </summary>
public static class XBigInteger
{
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

    #region Other methods

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

    #endregion Other methods

    #region Extension methods for IEnumerable<BigInteger>

    /// <summary>
    /// Sum all the values in a list of BigIntegers.
    /// </summary>
    public static BigInteger Sum(this IEnumerable<BigInteger> nums) =>
        nums.Aggregate((BigInteger)0, (sum, num) => sum + num);

    #endregion Extension methods for IEnumerable<BigInteger>
}
