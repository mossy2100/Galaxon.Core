namespace Galaxon.Core.Numbers;

public static class XUint
{
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
}
