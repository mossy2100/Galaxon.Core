namespace Galaxon.Core.Numbers;

/// <summary>Extension methods for long.</summary>
public static class XLong
{
    /// <summary>
    /// Get the long value closest to x^y.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static long Pow(long x, long y) =>
        (long)Math.Round(Math.Pow(x, y));

    /// <summary>
    /// Get the long value closest to √x.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static long Sqrt(long x) =>
        (long)Math.Round(Math.Sqrt(x));

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

    /// <summary>
    /// Get a random long.
    /// </summary>
    public static long GetRandom()
    {
        Random rnd = new ();

        // Get a random value in the range 0..long.MaxValue.
        // It's non-negative, so the most significant bit will always be 0.
        var n = rnd.NextInt64();

        // Get a random sign bit.
        var signBit = (byte)rnd.Next(2);

        return (long)signBit << 63 | n;
    }
}
