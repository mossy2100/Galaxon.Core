using System.Numerics;

namespace AstroMultimedia.Core.Numbers;

/// <summary>
/// Extension class for INumberBase.
/// </summary>
public static class XInt32
{
    /// <summary>
    /// Get the int closest to the square root of the given int.
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public static int Sqrt(int n) =>
        (int)Round(Math.Sqrt(n));

    /// <summary>
    /// Format an int as a superscript.
    /// </summary>
    public static string ToSuperscriptString(this int n) =>
        ((BigInteger)n).ToSuperscriptString();

    /// <summary>
    /// Format an int as a subscript.
    /// </summary>
    public static string ToSubscriptString(this int n) =>
        ((BigInteger)n).ToSubscriptString();
}
