using System.Numerics;

namespace AstroMultimedia.Core.Numbers;

public static class XNumber
{
    public static bool IsSignedInteger(object? obj) =>
        obj is sbyte or int or short or long or Int128 or BigInteger;

    public static bool IsUnsignedInteger(object? obj) =>
        obj is byte or uint or ushort or ulong or UInt128;

    public static bool IsFloatingPoint(object? obj) =>
        obj is Half or float or double;

    public static bool IsFixedPoint(object? obj) =>
        obj is decimal;

    public static bool IsInteger(object? obj) =>
        IsSignedInteger(obj) || IsUnsignedInteger(obj);

    public static bool IsNonInteger(object? obj) =>
        IsFloatingPoint(obj) || IsFixedPoint(obj);

    public static bool IsReal(object? obj) =>
        IsInteger(obj) || IsNonInteger(obj);

    public static bool IsComplex(object? obj) =>
        obj is Complex;

    public static bool IsNumber(object? obj) =>
        IsReal(obj) || IsComplex(obj);

    /// <summary>
    /// Integer division and modulo operation using floored division.
    /// The modulus will always have the same sign as the divisor.
    ///
    /// Unlike the truncated division and modulo provided by C#'s operators, floored division
    /// produces a regular cycling pattern through both negative and positive values of the divisor.
    ///
    /// It permits things like:
    ///   bool isOdd = Mod(num, 2) == 1;
    ///
    /// Trying to do this using the % operator will fail for negative divisors, however. e.g.
    ///   bool isOdd = num % 2 == 1;
    /// In this case, if num is negative 0, num % 2 == -1
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Modulo_operation" />
    public static (T div, T mod) DivMod<T>(T a, T b) where T : INumberBase<T>,
        IModulusOperators<T, T, T>, IComparisonOperators<T, T, bool>
    {
        T d = a / b;
        T m = a % b;
        if (m < T.Zero && b > T.Zero || m > T.Zero && b < T.Zero)
        {
            m += b;
            d--;
        }
        return (d, m);
    }

    /// <summary>
    /// Corrected integer division operation.
    /// </summary>
    /// <see cref="DivMod{T}" />
    public static T Div<T>(T a, T b) where T : INumberBase<T>, IModulusOperators<T, T, T>,
        IComparisonOperators<T, T, bool>
    {
        (T d, T m) = DivMod(a, b);
        return d;
    }

    /// <summary>
    /// Corrected modulo operation.
    /// </summary>
    /// <see cref="DivMod{T}" />
    public static T Mod<T>(T a, T b) where T : INumberBase<T>, IModulusOperators<T, T, T>,
        IComparisonOperators<T, T, bool>
    {
        (T d, T m) = DivMod(a, b);
        return m;
    }
}
