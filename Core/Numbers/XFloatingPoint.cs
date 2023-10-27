using System.Numerics;
using Galaxon.Core.Types;

namespace Galaxon.Core.Numbers;

/// <summary>Extension methods for IFloatingPoint{T}.</summary>
public static class XFloatingPoint
{
    /// <summary>
    /// Get the Galaxon extension type for this floating point type.
    /// </summary>
    /// <typeparam name="T">The standard floating point type.</typeparam>
    /// <returns>The corresponding extension type.</returns>
    public static Type? GetExtensionType<T>() where T : IFloatingPointIeee754<T>
    {
        var xtypeName = "";
        var type = typeof(T);
        if (type == typeof(Half))
        {
            xtypeName = "XHalf";
        }
        else if (type == typeof(float))
        {
            xtypeName = "XFloat";
        }
        else if (type == typeof(double))
        {
            xtypeName = "XDouble";
        }
        return Type.GetType($"Galaxon.Core.Numbers.{xtypeName}");
    }

    /// <summary>
    /// Get the total number of bits in values of this type.
    /// </summary>
    public static byte GetTotalNumBits<T>() where T : IFloatingPointIeee754<T>
    {
        if (GetExtensionType<T>() is Type xtype
            && XReflection.GetStaticFieldValue(xtype, "TotalNumBits") is byte b)
        {
            return b;
        }

        throw new InvalidOperationException("Unsupported type.");
    }

    /// <summary>
    /// Get the number of bits in the fraction part of values of this type.
    /// </summary>
    public static byte GetNumFracBits<T>() where T : IFloatingPointIeee754<T>
    {
        if (GetExtensionType<T>() is Type xtype
            && XReflection.GetStaticFieldValue(xtype, "NumFracBits") is byte b)
        {
            return b;
        }

        throw new InvalidOperationException("Unsupported type.");
    }

    /// <summary>
    /// Get the number of bits in the exponent part of values of this type.
    /// </summary>
    public static byte GetNumExpBits<T>() where T : IFloatingPointIeee754<T>
    {
        return (byte)(GetTotalNumBits<T>() - GetNumFracBits<T>() - 1);
    }

    /// <summary>
    /// Get the exponent bias for this type.
    /// </summary>
    public static short GetExpBias<T>() where T : IFloatingPointIeee754<T>
    {
        return (short)(1 << (GetNumExpBits<T>() - 1) - 1);
    }

    /// <summary>
    /// Get the minimum binary exponent supported by the type.
    /// </summary>
    public static short GetMinExp<T>() where T : IFloatingPointIeee754<T>
    {
        return (short)(1 - GetExpBias<T>());
    }

    /// <summary>
    /// Get the maximum binary exponent supported by the type.
    /// </summary>
    public static short GetMaxExp<T>() where T : IFloatingPointIeee754<T>
    {
        return GetExpBias<T>();
    }

    /// <summary>
    /// Get the sign bit mask for this type.
    /// </summary>
    public static ulong GetSignBitMask<T>() where T : IFloatingPointIeee754<T>
    {
        return 1ul << (GetTotalNumBits<T>() - 1);
    }

    /// <summary>
    /// Get the exponent bit mask for this type.
    /// </summary>
    public static ulong GetExpBitMask<T>() where T : IFloatingPointIeee754<T>
    {
        return (1ul << GetNumExpBits<T>()) - 1 << GetNumFracBits<T>();
    }

    /// <summary>
    /// Get the fraction bit mask for this type.
    /// </summary>
    public static ulong GetFracBitMask<T>() where T : IFloatingPointIeee754<T>
    {
        return (1ul << GetNumFracBits<T>()) - 1;
    }

    /// <summary>
    /// Get the minimum positive subnormal value for this type.
    /// </summary>
    public static T GetMinPosSubnormalValue<T>() where T : IFloatingPointIeee754<T>
    {
        if (XReflection.GetStaticFieldOrPropertyValue<T>("Epsilon") is T result)
        {
            return result;
        }

        throw new InvalidOperationException(
            $"Could not find MinPosSubnormalValue for {typeof(T).Name}.");
    }

    /// <summary>
    /// Get the maximum positive subnormal value for this type.
    /// </summary>
    public static T GetMaxPosSubnormalValue<T>() where T : IFloatingPointIeee754<T>
    {
        return Assemble<T>(0, 0, GetFracBitMask<T>());
    }

    /// <summary>
    /// Get the minimum positive normal value for this type.
    /// </summary>
    public static T GetMinPosNormalValue<T>() where T : IFloatingPointIeee754<T>
    {
        return Assemble<T>(0, 1, 0);
    }

    /// <summary>
    /// Get the minimum positive subnormal value for this type.
    /// </summary>
    public static T GetMaxPosNormalValue<T>() where T : IFloatingPointIeee754<T>
    {
        if (XReflection.GetStaticFieldOrPropertyValue<T>("MaxValue") is T result)
        {
            return result;
        }

        throw new InvalidOperationException(
            $"Could not find MaxPosNormalValue for {typeof(T).Name}.");
    }

    /// <summary>
    /// Disassemble the floating point value into its bitwise components.
    /// </summary>
    public static (byte signBit, ushort expBits, ulong fracBits) Disassemble<T>(this T x)
        where T : IFloatingPointIeee754<T>
    {
        // Get the bits.
        ulong bits = x switch
        {
            Half h => BitConverter.HalfToUInt16Bits(h),
            float f => BitConverter.SingleToUInt32Bits(f),
            double d => BitConverter.DoubleToUInt64Bits(d),
            _ => throw new InvalidOperationException("Unsupported type."),
        };

        // Get some info about the type.
        var nTotalBits = GetTotalNumBits<T>();
        var nFracBits = GetNumFracBits<T>();
        var signBitMask = GetSignBitMask<T>();
        var expBitMask = GetExpBitMask<T>();
        var fracBitMask = GetFracBitMask<T>();

        // Extract the parts.
        var signBit = (byte)(bits & signBitMask >> (nTotalBits - 1));
        var expBits = (ushort)(bits & expBitMask >> nFracBits);
        var fracBits = bits & fracBitMask;

        return (signBit, expBits, fracBits);
    }

    /// <summary>
    /// Assemble a new floating point value from parts.
    /// </summary>
    /// <param name="signBit">The sign bit (1 or 0).</param>
    /// <param name="expBits">The exponent bits.</param>
    /// <param name="fracBits">The fraction bits.</param>
    /// <typeparam name="T">A floating point type.</typeparam>
    /// <returns>The new floating point value.</returns>
    /// <exception cref="InvalidOperationException">If the type is unsupported.</exception>
    public static T Assemble<T>(byte signBit, ushort expBits, ulong fracBits)
        where T : IFloatingPointIeee754<T>
    {
        var nFracBits = GetNumFracBits<T>();
        var nExpBits = GetNumExpBits<T>();
        var nSignBitShift = (byte)(nFracBits + nExpBits);

        // Check values are within valid ranges for the type.
        if (signBit > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(signBit), "Must be 0 or 1.");
        }
        var expBitsMax = (ushort)((1 << nExpBits) - 1);
        if (expBits > expBitsMax)
        {
            throw new ArgumentOutOfRangeException(nameof(expBits),
                $"Must be less than or equal to {expBitsMax}.");
        }
        var fracBitsMax = GetFracBitMask<T>();
        if (fracBits > fracBitsMax)
        {
            throw new ArgumentOutOfRangeException(nameof(fracBits),
                $"Must be less than or equal to {fracBitsMax}.");
        }

        // Construct the sequence of bits.
        var bits = ((ulong)signBit << nSignBitShift) | ((ulong)expBits << nFracBits) | fracBits;

        // Return a value of the correct type.
        var t = typeof(T);
        if (t == typeof(Half))
        {
            return (T)(object)BitConverter.UInt16BitsToHalf((ushort)bits);
        }
        if (t == typeof(float))
        {
            return (T)(object)BitConverter.UInt32BitsToSingle((uint)bits);
        }
        if (t == typeof(double))
        {
            return (T)(object)BitConverter.UInt64BitsToDouble(bits);
        }

        throw new InvalidOperationException("Unsupported type.");
    }
}
