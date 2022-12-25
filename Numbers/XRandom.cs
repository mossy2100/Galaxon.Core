namespace Galaxon.Core.Numbers;

public static class XRandom
{
    /// <summary>
    /// Returns a random int.
    /// </summary>
    public static int NextInt32(this Random rng) =>
        rng.Next(2) == 1 ? -rng.Next() - 1 : rng.Next();

    /// <summary>
    /// Returns a random decimal.
    /// </summary>
    public static decimal NextDecimal(this Random rng) =>
        new(rng.NextInt32(), rng.NextInt32(), rng.NextInt32(), rng.Next(2) == 1,
            (byte)rng.Next(29));

    /// <summary>
    /// Generates a random double.
    /// </summary>
    /// <param name="rng"></param>
    /// <returns></returns>
    public static double NextDoubleFullRange(this Random rng)
    {
        while (true)
        {
            double significand = rng.NextDouble();
            if (significand == 0)
            {
                return 0;
            }
            int sign = rng.Next(2) == 1 ? -1 : 1;
            int mantissa = rng.Next(-324, 309);
            double result = sign * significand * Pow(10, mantissa);

            // Check we didn't generate a number too small or too large.
            if (result != 0 && double.IsFinite(result))
            {
                return result;
            }
        }
    }
}
