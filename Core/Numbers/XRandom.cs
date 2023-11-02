namespace Galaxon.Core.Numbers;

/// <summary>
/// Extension methods for Random class.
/// </summary>
public static class XRandom
{
    /// <summary>
    /// Get a random signed short.
    /// </summary>
    public static short NextShort(this Random rnd)
    {
        // Get a random non-negative int and cast to short.
        // The int value will wrap to a short within the valid range for the type, without overflow.
        return (short)rnd.Next();
    }

    /// <summary>
    /// Get a random unsigned short.
    /// </summary>
    public static ushort NextUshort(this Random rnd)
    {
        // Get a random non-negative int and cast to ushort.
        // The value will wrap to a ushort within the valid range for the type, without overflow.
        return (ushort)rnd.Next();
    }

    /// <summary>
    /// Get a random signed int.
    /// The Random.Next() method will only return an int in the range 0..int.MaxValue. This method
    /// will return a signed int with a value anywhere in the valid range for that type.
    /// </summary>
    public static int NextInt(this Random rnd)
    {
        // Get a random value in the range 0..int.MaxValue.
        // It's non-negative, so the most significant bit will always be 0.
        var i = rnd.Next();

        // Get a random sign bit.
        var signBit = (byte)rnd.Next(2);

        return (signBit << 31) | i;
    }

    /// <summary>
    /// Get a random unsigned int.
    /// The Random.Next() method will only return an int in the range 0..int.MaxValue. This method
    /// will return an unsigned int with a value anywhere in the valid range for that type.
    /// </summary>
    public static uint NextUint(this Random rnd) => (uint)rnd.NextInt();

    /// <summary>
    /// Get a random signed long.
    /// </summary>
    public static long NextLong(this Random rnd)
    {
        // Get a random value in the range 0..long.MaxValue.
        // It's non-negative, so the most significant bit will always be 0.
        var n = rnd.NextInt64();

        // Get a random sign bit.
        var signBit = (byte)rnd.Next(2);

        return ((long)signBit << 63) | n;
    }

    /// <summary>
    /// Get a random unsigned long.
    /// </summary>
    public static ulong NextUlong(this Random rnd) => (ulong)rnd.NextLong();

    /// <summary>
    /// Get a random Half.
    /// Will not return -0, -±∞, or NaN.
    /// </summary>
    public static Half NextHalf(this Random rnd)
    {
        // expBits can range from 00000..11111, which is 0..31.
        // We'll ignore 111111, because this means ±infinity or NaN.
        var expBits = (ushort)rnd.Next(31);

        // If 0, we're done.
        if (expBits == 0) return Half.Zero;

        // Get a random sign bit.
        var signBit = (byte)rnd.Next(2);

        // Get the random fraction bits.
        var fracBits = (ulong)rnd.Next(1024);

        return XFloatingPoint.Assemble<Half>(signBit, expBits, fracBits);
    }

    /// <summary>
    /// Get a random decimal.
    /// </summary>
    public static decimal NextDecimal(this Random rnd)
    {
        var lo = rnd.NextInt();
        var mid = rnd.NextInt();
        var hi = rnd.NextInt();
        var isNegative = rnd.Next(2) == 1;
        var scale = (byte)rnd.Next(29);
        return new decimal(lo, mid, hi, isNegative, scale);
    }
}
