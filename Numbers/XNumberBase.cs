using System.Numerics;

namespace Galaxon.Core.Numbers;

/// <summary>
/// Extension methods for INumberBase and INumberBase{T}.
/// </summary>
/// <remarks>
/// TODO: Sort out methods to check for implementation of generic interfaces.
/// </remarks>
public static class XNumberBase
{
    #region Inspection methods

    /// <summary>
    /// Check if a type implements a generic interface.
    /// </summary>
    /// <returns></returns>
    public static bool Implements(object obj, Type genericInterface)
    {
        return obj.GetType().GetInterfaces().Any(x =>
            x.IsGenericType &&
            x.GetGenericTypeDefinition() == genericInterface);
    }

    public static bool IsSignedInteger(object? obj) =>
        obj != null && Implements(obj, typeof(IBinaryInteger<>));

    public static bool IsUnsignedInteger(object? obj) =>
        obj is byte or uint or ushort or ulong or UInt128;

    public static bool IsFloatingPoint(object? obj) =>
        obj is Half or float or double or decimal;

    public static bool IsInteger(object? obj) =>
        IsSignedInteger(obj) || IsUnsignedInteger(obj);

    public static bool IsReal(object? obj) =>
        IsInteger(obj) || IsFloatingPoint(obj);

    public static bool IsComplex(object? obj) =>
        obj is Complex;

    public static bool IsNumber(object? obj) =>
        IsReal(obj) || IsComplex(obj);

    #endregion Inspection methods

    #region Division-related methods

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

    #endregion Division-related methods

    #region Methods for IEnumerable<INumberBase<T>>

    /// <summary>
    /// Similar to Sum(), this extension method generates the product of all values in a collection
    /// of numbers.
    /// </summary>
    public static T Product<T>(this IEnumerable<T> source) where T : INumberBase<T> =>
        source.Aggregate(T.One, (prod, value) => prod * value);

    /// <summary>
    /// Similar to Sum(), get a product of all values in the collection, transformed by the supplied
    /// function.
    /// </summary>
    public static T Product<T>(this IEnumerable<T> source, Func<T, T> func)
        where T : INumberBase<T> =>
        source.Aggregate(T.One, (prod, value) => prod * func(value));

    #endregion Methods for IEnumerable<INumberBase<T>>
}
