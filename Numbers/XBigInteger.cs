using System.Numerics;

namespace AstroMultimedia.Core.Numbers;

/// <summary>
/// Extension methods for BigInteger.
/// </summary>
public static class XBigInteger
{
    #region Superscript and subscript members

    /// <summary>
    /// Superscript versions of characters that can appear in int.ToString().
    /// </summary>
    internal static readonly Dictionary<char, char> SuperscriptChars = new()
    {
        { '-', '⁻' },
        { '0', '⁰' },
        { '1', '¹' },
        { '2', '²' },
        { '3', '³' },
        { '4', '⁴' },
        { '5', '⁵' },
        { '6', '⁶' },
        { '7', '⁷' },
        { '8', '⁸' },
        { '9', '⁹' }
    };

    /// <summary>
    /// Subscript versions of characters that can appear in int.ToString().
    /// </summary>
    internal static readonly Dictionary<char, char> SubscriptChars = new()
    {
        { '-', '₋' },
        { '0', '₀' },
        { '1', '₁' },
        { '2', '₂' },
        { '3', '₃' },
        { '4', '₄' },
        { '5', '₅' },
        { '6', '₆' },
        { '7', '₇' },
        { '8', '₈' },
        { '9', '₈' }
    };

    /// <summary>
    /// Format a BigInteger as a superscript.
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public static string ToSuperscriptString(this BigInteger n) =>
        new(n.ToString().Select(c => SuperscriptChars[c]).ToArray());

    /// <summary>
    /// Format a BigInteger as a subscript.
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public static string ToSubscriptString(this BigInteger n) =>
        new(n.ToString().Select(c => SubscriptChars[c]).ToArray());

    #endregion Superscript and subscript members

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
    public static BigInteger Sum(this List<BigInteger> nums) =>
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
