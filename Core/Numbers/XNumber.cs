using System.Numerics;
using Galaxon.Core.Types;

namespace Galaxon.Core.Numbers;

/// <summary>Extension methods for numbers (INumber{T} and INumberBase{T}).</summary>
/// <remarks>
/// TODO: Sort out methods to check for implementation of generic interfaces.
/// </remarks>
public static class XNumber
{
    #region Methods for inspecting the number type

    /// <summary>
    /// Check if a type is a standard numerical type in .NET.
    /// Excludes char, vector, and matrix types.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type is a standard numerical type.</returns>
    public static bool IsStandardNumberType(Type type)
    {
        return type == typeof(sbyte)
            || type == typeof(byte)
            || type == typeof(short)
            || type == typeof(ushort)
            || type == typeof(int)
            || type == typeof(uint)
            || type == typeof(long)
            || type == typeof(ulong)
            || type == typeof(Int128)
            || type == typeof(UInt128)
            || type == typeof(Half)
            || type == typeof(float)
            || type == typeof(double)
            || type == typeof(decimal)
            || type == typeof(BigInteger)
            || type == typeof(Complex);
    }

    /// <summary>
    /// Check if a type is a number type.
    /// </summary>
    /// <param name="type">Some type.</param>
    /// <returns>If the type implements INumberBase{TSelf}.</returns>
    public static bool IsNumberType(Type type)
    {
        return XReflection.ImplementsSelfReferencingGenericInterface(type, typeof(INumberBase<>));
    }

    /// <summary>
    /// Check if a type is a signed number type.
    /// </summary>
    /// <param name="type">Some type.</param>
    /// <returns>If the type implements ISignedNumber{TSelf}.</returns>
    public static bool IsSignedNumberType(Type type)
    {
        return XReflection.ImplementsSelfReferencingGenericInterface(type, typeof(ISignedNumber<>));
    }

    /// <summary>
    /// Check if a type is an unsigned number type.
    /// </summary>
    /// <param name="type">Some type.</param>
    /// <returns>If the type implements IUnsignedNumber{TSelf}.</returns>
    public static bool IsUnsignedNumberType(Type type)
    {
        return XReflection.ImplementsSelfReferencingGenericInterface(type,
            typeof(IUnsignedNumber<>));
    }

    /// <summary>
    /// Check if a type is an integer type.
    /// </summary>
    /// <param name="type">Some type.</param>
    /// <returns>If the type implements IBinaryInteger{TSelf}.</returns>
    public static bool IsIntegerType(Type type)
    {
        return XReflection.ImplementsSelfReferencingGenericInterface(type,
            typeof(IBinaryInteger<>));
    }

    /// <summary>
    /// Check if a type is a floating point type.
    /// </summary>
    /// <param name="type">Some type.</param>
    /// <returns>If the type implements IFloatingPoint{TSelf}.</returns>
    public static bool IsFloatingPointType(Type type)
    {
        return XReflection.ImplementsSelfReferencingGenericInterface(type,
            typeof(IFloatingPoint<>));
    }

    /// <summary>
    /// Check if a type is a signed integer type.
    /// </summary>
    /// <param name="type">Some type.</param>
    /// <returns></returns>
    public static bool IsSignedIntegerType(Type type)
    {
        return IsSignedNumberType(type) && IsIntegerType(type);
    }

    /// <summary>
    /// Check if a type is an unsigned integer type.
    /// </summary>
    /// <param name="type">Some type.</param>
    /// <returns></returns>
    public static bool IsUnsignedIntegerType(Type type)
    {
        return IsUnsignedNumberType(type) && IsIntegerType(type);
    }

    /// <summary>
    /// Check if a type is a real (non-complex) number type.
    /// </summary>
    /// <param name="type">Some type.</param>
    /// <returns></returns>
    public static bool IsRealNumberType(Type type)
    {
        return IsIntegerType(type) || IsFloatingPointType(type);
    }

    /// <summary>
    /// Check if a type is a complex number type.
    /// TODO Make it support BigComplex without actually referencing it.
    /// </summary>
    /// <param name="type">Some type.</param>
    /// <returns></returns>
    public static bool IsComplexNumberType(Type type)
    {
        return type == typeof(Complex);
    }

    #endregion Inspection methods

    #region Division-related methods

    /// <summary>
    /// Integer division and modulo operation using floored division.
    /// The modulus will always have the same sign as the divisor.
    /// Unlike the truncated division and modulo provided by C#'s operators, floored division
    /// produces a regular cycling pattern through both negative and positive values of the divisor.
    /// It permits things like:
    /// bool isOdd = Mod(num, 2) == 1;
    /// Trying to do this using the % operator will fail for negative divisors, however. e.g.
    /// bool isOdd = num % 2 == 1;
    /// In this case, if num is negative 0, num % 2 == -1
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Modulo_operation" />
    public static (T div, T mod) DivMod<T>(T a, T b) where T : INumberBase<T>,
        IModulusOperators<T, T, T>, IComparisonOperators<T, T, bool>
    {
        var d = a / b;
        var m = a % b;
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
        var (d, m) = DivMod(a, b);
        return d;
    }

    /// <summary>
    /// Corrected modulo operation.
    /// </summary>
    /// <see cref="DivMod{T}" />
    public static T Mod<T>(T a, T b) where T : INumberBase<T>, IModulusOperators<T, T, T>,
        IComparisonOperators<T, T, bool>
    {
        var (d, m) = DivMod(a, b);
        return m;
    }

    #endregion Division-related methods

    #region Methods for IEnumerable<INumberBase<T>>

    /// <summary>
    /// Similar to Sum(), this extension method generates the product of all values in a collection
    /// of numbers.
    /// </summary>
    public static T Product<T>(this IEnumerable<T> source) where T : INumberBase<T>
    {
        return source.Aggregate(T.One, (prod, value) => prod * value);
    }

    /// <summary>
    /// Similar to Sum(), get a product of all values in the collection, transformed by the supplied
    /// function.
    /// </summary>
    public static T Product<T>(this IEnumerable<T> source, Func<T, T> func)
        where T : INumberBase<T>
    {
        return source.Aggregate(T.One, (prod, value) => prod * func(value));
    }

    #endregion Methods for IEnumerable<INumberBase<T>>

    #region Methods related to static properties

    /// <summary>
    /// Get the min value for a specified number type, if specified.
    /// </summary>
    /// <typeparam name="T">The number type.</typeparam>
    /// <returns>The value of the MinValue property, or null if unspecified.</returns>
    public static T? GetMinValue<T>() where T : INumberBase<T>
    {
        return (T?)XReflection.GetStaticFieldOrPropertyValue<T>("MinValue");
    }

    /// <summary>
    /// Get the max value for a specified number type, if specified.
    /// </summary>
    /// <typeparam name="T">The number type.</typeparam>
    /// <returns>The value of the MaxValue property, or null if unspecified.</returns>
    public static T? GetMaxValue<T>() where T : INumberBase<T>
    {
        return (T?)XReflection.GetStaticFieldOrPropertyValue<T>("MaxValue");
    }

    /// <summary>
    /// Get the min and max values for a specified number type, if specified.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>The value of the MinValue and MaxValue properties, or two nulls if
    /// unspecified.</returns>
    public static (T? min, T? max) GetRange<T>() where T : INumberBase<T>
    {
        return (GetMinValue<T>(), GetMaxValue<T>());
    }

    /// <summary>
    /// Get the positive infinity value for a specified number type, if specified.
    /// </summary>
    /// <typeparam name="T">The number type.</typeparam>
    /// <returns>The value of the PositiveInfinity property, or null if unspecified.</returns>
    public static T? GetPositiveInfinity<T>() where T : INumberBase<T>
    {
        return (T?)XReflection.GetStaticFieldOrPropertyValue<T>("PositiveInfinity");
    }

    /// <summary>
    /// Get the negative infinity value for a specified number type, if specified.
    /// </summary>
    /// <typeparam name="T">The number type.</typeparam>
    /// <returns>The value of the NegativeInfinity property, or null if unspecified.</returns>
    public static T? GetNegativeInfinity<T>() where T : INumberBase<T>
    {
        return (T?)XReflection.GetStaticFieldOrPropertyValue<T>("NegativeInfinity");
    }

    /// <summary>
    /// Get the positive and negative infinity values for a specified number type, if specified.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>The value of the PositiveInfinity and NegativeInfinity properties, or two nulls if
    /// unspecified.</returns>
    public static (T? min, T? max) GetInfinities<T>() where T : INumberBase<T>
    {
        return (GetPositiveInfinity<T>(), GetNegativeInfinity<T>());
    }

    #endregion Methods related to static properties
}
