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
}
