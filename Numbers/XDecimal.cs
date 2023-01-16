using DecimalMath;

namespace Galaxon.Core.Numbers;

/// <summary>Extension methods for decimal.</summary>
public static class XDecimal
{
    #region Exponentiation methods

    /// <summary>
    /// Calculate the natural logarithm of a decimal.
    /// The algorithm is from:
    /// <see href="https://en.wikipedia.org/wiki/Natural_logarithm" />
    /// I found that DecimalEx.Log() hangs for very small values so I made this version.
    /// It's tested, fast, and doesn't break with the largest or smallest decimal values.
    /// <see cref="Math.Log(double)" />
    /// </summary>
    /// <param name="m">A decimal value.</param>
    /// <returns>The natural logarithm of the given value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the argument is less than or equal to 0.
    /// </exception>
    public static decimal Log(decimal m)
    {
        switch (m)
        {
            // Guards.
            case 0:
                throw new ArgumentOutOfRangeException(nameof(m),
                    "The logarithm of 0 is undefined. Math.Log(0) returns -Infinity, but the decimal type doesn't provide a way to represent this.");

            case < 0:
                throw new ArgumentOutOfRangeException(nameof(m),
                    "The logarithm of a negative value is a complex number. Use DecimalComplex.Ln().");

            // Optimizations.
            case 1:
                return 0;

            case 2:
                return DecimalEx.Ln2;

            case 10:
                return DecimalEx.Ln10;

            case DecimalEx.E:
                return 1;
        }

        // Scale the value to the range (0..1) so the Taylor series converges quickly and to avoid
        // overflow.
        int scale = (int)Floor(Math.Log10((double)m)) + 1;
        decimal x;

        // Some cleverness to avoid overflow if scale == 29.
        if (scale <= 28)
        {
            x = m / Exp10(scale);
        }
        else
        {
            x = m / 1e28m / Exp10(scale - 28);
        }

        // Use the Taylor series.
        x--;
        decimal xx = x;
        int n = 1;
        int s = 1;
        decimal oldValue = 0;
        decimal newValue = 0;
        while (true)
        {
            // Calculate the next term in the series.
            decimal term = s * xx / n;

            // Check if done.
            if (term == 0m)
            {
                break;
            }

            // Add the term.
            newValue = oldValue + term;

            // Prepare to calculate the next term.
            s = -s;
            xx *= x;
            n++;
            oldValue = newValue;
        }

        // Scale back.
        return newValue + scale * DecimalEx.Ln10;
    }

    /// <summary>
    /// Logarithm of a decimal in a specified base.
    /// <see cref="Math.Log(double, double)" />
    /// <see cref="Log(decimal, decimal)" />
    /// </summary>
    /// <param name="m">The decimal value.</param>
    /// <param name="b">The base.</param>
    /// <returns>The logarithm of z in base b.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the number is less
    /// than or equal to 0.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the base is less than
    /// or equal to 0, or equal to 1.
    /// </exception>
    public static decimal Log(decimal m, decimal b)
    {
        if (b == 1)
        {
            throw new ArgumentOutOfRangeException(nameof(b),
                "Logarithms are undefined for a base of 1.");
        }

        // 0^0 == 1. Mimics Math.Log().
        if (m == 1 && b == 0)
        {
            return 0;
        }

        // This will throw if m <= 0 || b <= 0.
        return Log(m) / Log(b);
    }

    /// <summary>
    /// Logarithm of a decimal in base 10.
    /// <see cref="Math.Log10" />
    /// <see cref="Log10" />
    /// </summary>
    /// <param name="m">The decimal value.</param>
    /// <returns>The logarithm of the number in base 10.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the number is less than or equal to 0.
    /// </exception>
    public static decimal Log10(decimal m) =>
        Log(m, 10);

    /// <summary>
    /// Calculate 10 raised to a decimal power.
    /// </summary>
    /// <param name="m">A decimal value.</param>
    /// <returns>10^d</returns>
    public static decimal Exp10(decimal m) =>
        DecimalEx.Pow(10, m);

    /// <summary>
    /// Logarithm of a decimal in base 2.
    /// </summary>
    /// <param name="m">The decimal value.</param>
    /// <returns>The logarithm of the number in base 2.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the number is less than or equal to 0.
    /// </exception>
    public static decimal Log2(decimal m) =>
        Log(m, 2);

    /// <summary>
    /// Calculate 2 raised to a decimal power.
    /// </summary>
    /// <param name="m">A decimal value.</param>
    /// <returns>2^d</returns>
    public static decimal Exp2(decimal m) =>
        DecimalEx.Pow(2, m);

    #endregion Exponentiation methods

    #region Trigonometry methods

    /// <summary>
    /// Returns the cotangent of the specified angle.
    /// </summary>
    /// <param name="alpha">An angle, measured in radians.</param>
    public static decimal Cot(decimal alpha)
    {
        // Calculating sin(𝛼) first to avoid calculating cos(𝛼) if unnecessary.
        decimal s = DecimalEx.Sin(alpha);
        if (s == 0)
        {
            throw new NotFiniteNumberException("Cotangent is undefined at this angle.");
        }
        return DecimalEx.Cos(alpha) / s;
    }

    /// <summary>
    /// Returns the secant of the specified angle.
    /// </summary>
    /// <param name="alpha">An angle, measured in radians.</param>
    public static decimal Sec(decimal alpha)
    {
        try
        {
            return 1 / DecimalEx.Cos(alpha);
        }
        catch (Exception)
        {
            throw new NotFiniteNumberException("Secant is undefined at this angle.");
        }
    }

    /// <summary>
    /// Returns the cosecant of the specified angle.
    /// </summary>
    /// <param name="alpha">An angle, measured in radians.</param>
    public static decimal Csc(decimal alpha)
    {
        try
        {
            return 1 / DecimalEx.Sin(alpha);
        }
        catch (Exception)
        {
            throw new NotFiniteNumberException("Cosecant is undefined at this angle.");
        }
    }

    /// <summary>
    /// Returns the angle whose cotangent is the specified number.
    /// </summary>
    /// <param name="x">A number representing a cotangent.</param>
    public static decimal Acot(decimal x) =>
        DecimalEx.PiHalf - DecimalEx.ATan(x);

    /// <summary>
    /// Returns the angle whose secant is the specified number.
    /// </summary>
    /// <param name="x">A number representing a secant.</param>
    public static decimal Asec(decimal x)
    {
        if (Abs(x) < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(x),
                "The absolute value of the argument must be greater than or equal to 1.");
        }

        // Todo check for division of zero here.
        return DecimalEx.ACos(1 / x);
    }

    /// <summary>
    /// Returns the angle whose cosecant is the specified number.
    /// </summary>
    /// <param name="x">A number representing a cosecant.</param>
    public static decimal Acsc(decimal x)
    {
        if (Abs(x) < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(x),
                "The absolute value of the argument must be greater than or equal to 1.");
        }

        return DecimalEx.ASin(1 / x);
    }

    /// <summary>
    /// Hyperbolic sine.
    /// </summary>
    /// <param name="x">The hyperbolic angle.</param>
    /// <returns>The hyperbolic sine of the given angle.</returns>
    public static decimal Sinh(decimal x) =>
        (DecimalEx.Exp(x) - DecimalEx.Exp(-x)) / 2;

    /// <summary>
    /// Hyperbolic cosine.
    /// </summary>
    /// <param name="x">The hyperbolic angle.</param>
    /// <returns>The hyperbolic cosine of the given angle.</returns>
    public static decimal Cosh(decimal x) =>
        (DecimalEx.Exp(x) + DecimalEx.Exp(-x)) / 2;

    /// <summary>
    /// Hyperbolic tangent.
    /// </summary>
    /// <param name="x">The hyperbolic angle.</param>
    /// <returns>The hyperbolic tangent of the given angle.</returns>
    public static decimal Tanh(decimal x)
    {
        decimal f = DecimalEx.Exp(2 * x);
        return (f - 1) / (f + 1);
    }

    /// <summary>
    /// Hyperbolic cotangent.
    /// </summary>
    /// <param name="x">The hyperbolic angle.</param>
    /// <returns>The hyperbolic cotangent of the given angle.</returns>
    public static decimal Coth(decimal x)
    {
        decimal f = DecimalEx.Exp(2 * x);
        return (f + 1) / (f - 1);
    }

    /// <summary>
    /// Hyperbolic secant.
    /// </summary>
    /// <param name="x">The hyperbolic angle.</param>
    /// <returns>The hyperbolic secant of the given angle.</returns>
    public static decimal Sech(decimal x) =>
        2 / (DecimalEx.Exp(x) + DecimalEx.Exp(-x));

    /// <summary>
    /// Hyperbolic cosecant.
    /// </summary>
    /// <param name="x">The hyperbolic angle.</param>
    /// <returns>The hyperbolic cosecant of the given angle.</returns>
    public static decimal Csch(decimal x) =>
        2 / (DecimalEx.Exp(x) - DecimalEx.Exp(-x));

    /// <summary>
    /// Inverse hyperbolic sine.
    /// </summary>
    /// <param name="x">The hyperbolic sine of an angle.</param>
    /// <returns>The angle.</returns>
    public static decimal Asinh(decimal x) =>
        Log(x + DecimalEx.Sqrt(x * x + 1));

    /// <summary>
    /// Inverse hyperbolic cosine.
    /// </summary>
    /// <param name="x">The hyperbolic cosine of an angle.</param>
    /// <returns>The angle.</returns>
    public static decimal Acosh(decimal x) =>
        Log(x + DecimalEx.Sqrt(x * x - 1));

    /// <summary>
    /// Inverse hyperbolic tangent.
    /// </summary>
    /// <param name="x">The hyperbolic tangent of an angle.</param>
    /// <returns>The angle.</returns>
    public static decimal Atanh(decimal x) =>
        Log((1 + x) / (1 - x)) / 2;

    /// <summary>
    /// Inverse hyperbolic cotangent.
    /// </summary>
    /// <param name="x">The hyperbolic cotangent of an angle.</param>
    /// <returns>The angle.</returns>
    public static decimal Acoth(decimal x) =>
        Log((x + 1) / (x - 1)) / 2;

    /// <summary>
    /// Inverse hyperbolic secant.
    /// </summary>
    /// <param name="x">The hyperbolic secant of an angle.</param>
    /// <returns>The angle.</returns>
    public static decimal Asech(decimal x) =>
        Log(1 / x + DecimalEx.Sqrt(1 / (x * x) - 1));

    /// <summary>
    /// Inverse hyperbolic cosecant.
    /// </summary>
    /// <param name="x">The hyperbolic cosecant of an angle.</param>
    /// <returns>The angle.</returns>
    public static decimal Acsch(decimal x) =>
        Log(1 / x + DecimalEx.Sqrt(1 / (x * x) + 1));

    #endregion Trigonometry methods

    #region Miscellaneous methods

    public static bool IsInteger(decimal m) =>
        m == decimal.Truncate(m);

    /// <summary>
    /// Round off a value to a given number of significant figures.
    /// </summary>
    /// <param name="m">The number to round.</param>
    /// <param name="n">The number of significant figures.</param>
    /// <returns>The rounded number.</returns>
    /// TODO: Test. If digits is too high an exception will be thrown, so this needs to be checked.
    public static decimal RoundSigFigs(decimal m, int n)
    {
        if (m == 0)
        {
            return 0;
        }
        decimal scale = Exp10(Floor(Log10(Abs(m))) + 1);
        return scale * Round(m / scale, n);
    }

    /// <summary>
    /// Returns a random decimal.
    /// </summary>
    public static decimal GetRandom()
    {
        Random rnd = new ();
        return
            new decimal(rnd.Next(), rnd.Next(), rnd.Next(), rnd.Next(2) == 1, (byte)rnd.Next(29));
    }

    #endregion Miscellaneous methods
}
