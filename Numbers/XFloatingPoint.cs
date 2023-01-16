using System.Numerics;

namespace Galaxon.Core.Numbers;

/// <summary>Extension methods for IFloatingPoint{T}.</summary>
public static class XFloatingPoint
{
    /// <summary>
    /// Get the number of exponent bits.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the type is unsupported.</exception>
    public static byte GetNumExpBits<T>() where T : IFloatingPoint<T> =>
        T.Zero switch
        {
            Half => XHalf.NumExpBits,
            float => XFloat.NumExpBits,
            double => XDouble.NumExpBits,
            _ => throw new InvalidOperationException("Unsupported type.")
        };

    /// <summary>
    /// Get the number of fraction bits.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the type is unsupported.</exception>
    public static byte GetNumFracBits<T>() where T : IFloatingPoint<T> =>
        T.Zero switch
        {
            Half => XHalf.NumFracBits,
            float => XFloat.NumFracBits,
            double => XDouble.NumFracBits,
            _ => throw new InvalidOperationException("Unsupported type.")
        };

    /// <summary>
    /// Get the minimum exponent for the type.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the type is unsupported.</exception>
    public static short GetMinExp<T>() where T : IFloatingPoint<T> =>
        (short)(1 - GetMaxExp<T>());

    /// <summary>
    /// Get the maximum exponent for the type.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the type is unsupported.</exception>
    public static short GetMaxExp<T>() where T : IFloatingPoint<T> =>
        (short)(Pow(2, GetNumExpBits<T>() - 1) - 1);

    /// <summary>
    /// Get the minimum positive normal value for the type.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the type is unsupported.</exception>
    public static T GetMinPosNormalValue<T>() where T : IFloatingPoint<T> =>
        T.Zero switch
        {
            Half => (T)(object)XHalf.MinPosNormalValue,
            float => (T)(object)XFloat.MinPosNormalValue,
            double => (T)(object)XDouble.MinPosNormalValue,
            _ => throw new InvalidOperationException("Unsupported type.")
        };

    /// <summary>
    /// Disassemble the floating point value into its bitwise components.
    /// </summary>
    public static (byte signBit, ushort expBits, ulong fracBits) Disassemble<T>(this T x)
        where T : IFloatingPoint<T>
    {
        byte signBit;
        ushort expBits;
        ulong fracBits;

        // (byte nExpBits, byte nFracBits, ushort expOffset) = x.GetStructure();
        byte nExpBits = GetNumExpBits<T>();
        byte nFracBits = GetNumFracBits<T>();
        byte nSignBitShift = (byte)(nFracBits + nExpBits);

        switch (x)
        {
            case Half h:
            {
                ushort bits = BitConverter.HalfToUInt16Bits(h);
                signBit = (byte)((bits & 0b10000000_00000000) >> nSignBitShift);
                expBits = (ushort)((bits & 0b01111100_00000000) >> nFracBits);
                fracBits = (ulong)(bits & 0b00000011_11111111);
                break;
            }

            case float f:
            {
                uint bits = BitConverter.SingleToUInt32Bits(f);
                signBit = (byte)((bits & 0b10000000_00000000_00000000_00000000) >> nSignBitShift);
                expBits = (ushort)((bits & 0b01111111_10000000_00000000_00000000) >> nFracBits);
                fracBits = bits & 0b00000000_01111111_11111111_11111111;
                break;
            }

            case double d:
            {
                ulong bits = BitConverter.DoubleToUInt64Bits(d);
                signBit = (byte)((bits
                    & 0b10000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000)
                    >> nSignBitShift);
                expBits = (ushort)((bits
                    & 0b01111111_11110000_00000000_00000000_00000000_00000000_00000000_00000000)
                    >> nFracBits);
                fracBits = bits
                    & 0b00000000_00001111_11111111_11111111_11111111_11111111_11111111_11111111;
                break;
            }

            default:
                throw new InvalidOperationException("Unsupported type.");
        }

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
        where T : IFloatingPoint<T>
    {
        byte nFracBits = GetNumFracBits<T>();
        byte nExpBits = GetNumExpBits<T>();
        byte nSignBitShift = (byte)(nFracBits + nExpBits);

        // Check values are within valid ranges for the type.
        if (signBit > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(signBit), "Must be 0 or 1.");
        }
        ushort expBitsMax = (ushort)((1 << nExpBits) - 1);
        if (expBits > expBitsMax)
        {
            throw new ArgumentOutOfRangeException(nameof(expBits),
                $"Must be less than or equal to {expBitsMax}.");
        }
        ulong fracBitsMax = (1ul << nFracBits) - 1;
        if (fracBits > fracBitsMax)
        {
            throw new ArgumentOutOfRangeException(nameof(fracBits),
                $"Must be less than or equal to {fracBitsMax}.");
        }

        // Get the bits.
        ulong bits = (ulong)(signBit << nSignBitShift) | (ulong)(expBits << nFracBits) | fracBits;

        switch (T.Zero)
        {
            case Half:
                return (T)(object)BitConverter.UInt16BitsToHalf((ushort)bits);

            case float:
                return (T)(object)BitConverter.UInt32BitsToSingle((uint)bits);

            case double:
                return (T)(object)BitConverter.UInt64BitsToDouble(bits);
        }

        throw new InvalidOperationException("Unsupported type.");
    }
}
