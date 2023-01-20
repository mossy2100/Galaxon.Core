namespace Galaxon.Core.Numbers;

/// <summary>Extension methods for int.</summary>
public static class XInt
{
    /// <summary>
    /// Get the int closest to the square root of the given int.
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public static int Sqrt(int n) =>
        (int)Math.Round(Math.Sqrt(n));

    /// <summary>
    /// Return the absolute value of an int as a uint.
    /// This addresses an issue with int.Abs(), which is that Abs(int.MinValue) can't be expressed
    /// as a int, and so wrap-around occurs.
    /// Unsigned integer types don't have an Abs() method, so there's no collision.
    /// </summary>
    /// <param name="n">A int value.</param>
    /// <returns>The absolute value as a uint.</returns>
    public static uint Abs(int n) =>
        n switch
        {
            int.MinValue => int.MaxValue + 1u,
            >= 0 => (uint)n,
            _ => (uint)-n
        };

    /// <summary>
    /// Get a random int.
    /// </summary>
    public static int GetRandom()
    {
        Random rnd = new ();

        // Get a random value in the range 0..int.MaxValue.
        // It's non-negative, so the most significant bit will always be 0.
        int i = rnd.Next();

        // Get a random sign bit.
        byte signBit = (byte)rnd.Next(2);

        return (signBit << 31) | i;
    }
}
