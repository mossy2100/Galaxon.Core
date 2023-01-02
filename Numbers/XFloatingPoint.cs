using System.Numerics;

namespace Galaxon.Core.Numbers;

/// <summary>
/// Extension methods for IFloatingPoint{T}.
/// </summary>
public static class XFloatingPoint
{
    /// <summary>
    /// Get the bitwise structure of the value.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the type is unsupported.</exception>
    public static (byte nExpBits, byte nFracBits, ushort expOffset) GetStructure<T>(this T x)
        where T : IFloatingPoint<T> =>
        x switch
        {
            double => (11, 52, 1023),
            float => (8, 23, 127),
            Half => (5, 10, 15),
            _ => throw new InvalidOperationException("Unsupported type.")
        };

    /// <summary>
    /// Disassemble the floating point value into its bitwise components.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Single-precision_floating-point_format" />
    /// <see href="https://en.wikipedia.org/wiki/Double-precision_floating-point_format" />
    /// <see href="https://en.wikipedia.org/wiki/Half-precision_floating-point_format" />
    public static (byte signBit, ushort expBits, ulong fracBits) Disassemble<T>(this T x)
        where T : IFloatingPoint<T>
    {
        byte signBit;
        ushort expBits;
        ulong fracBits;

        (byte nExpBits, byte nFracBits, ushort expOffset) = x.GetStructure();

        switch (x)
        {
            case double xDouble:
            {
                ulong xBits = BitConverter.DoubleToUInt64Bits(xDouble);
                signBit = (byte)((xBits
                    & 0b10000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000)
                    >> (nExpBits + nFracBits));
                expBits = (ushort)((xBits
                    & 0b01111111_11110000_00000000_00000000_00000000_00000000_00000000_00000000)
                    >> nFracBits);
                fracBits = (ulong)(xBits
                    & 0b00000000_00001111_11111111_11111111_11111111_11111111_11111111_11111111);
                break;
            }

            case float xSingle:
            {
                uint xBits = BitConverter.SingleToUInt32Bits(xSingle);
                signBit = (byte)((xBits & 0b10000000_00000000_00000000_00000000)
                    >> (nExpBits + nFracBits));
                expBits = (ushort)((xBits & 0b01111111_10000000_00000000_00000000) >> nFracBits);
                fracBits = (ulong)(xBits & 0b00000000_01111111_11111111_11111111);
                break;
            }

            case Half xHalf:
            {
                ushort xBits = BitConverter.HalfToUInt16Bits(xHalf);
                signBit = (byte)((xBits & 0b10000000_00000000) >> (nExpBits + nFracBits));
                expBits = (ushort)((xBits & 0b01111100_00000000) >> nFracBits);
                fracBits = (ulong)(xBits & 0b00000011_11111111);
                break;
            }

            default:
                throw new InvalidOperationException("Unsupported type.");
        }

        return (signBit, expBits, fracBits);
    }
}
