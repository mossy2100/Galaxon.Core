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
}
