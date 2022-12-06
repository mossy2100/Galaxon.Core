namespace AstroMultimedia.Core.Numbers;

public static class XInt64
{
    /// <summary>
    /// Get the long value closest to x^y.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static long Pow(long x, long y) =>
        (long)Round(Math.Pow(x, y));

    /// <summary>
    /// Get the long value closest to √x.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static long Sqrt(long x) =>
        (long)Round(Math.Sqrt(x));
}
