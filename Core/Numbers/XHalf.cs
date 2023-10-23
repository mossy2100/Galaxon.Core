namespace Galaxon.Core.Numbers;

/// <summary>Extension methods and other bonus stuff for Half.</summary>
public static class XHalf
{
    /// <summary>The number of bits in the exponent.</summary>
    public const byte NumExpBits = 5;

    /// <summary>The number of bits in the fraction.</summary>
    public const byte NumFracBits = 10;

    /// <summary>The minimum binary exponent supported by the type.</summary>
    public const short MinExp = -14;

    /// <summary>The maximum binary exponent supported by the type.</summary>
    public const short MaxExp = 15;

    /// <summary>
    /// The maximum positive subnormal value.
    /// </summary>
    public static Half MaxPosSubnormalValue => Assemble(0, 0, 0x3ff);

    /// <summary>
    /// The minimum positive normal value.
    /// </summary>
    public static Half MinPosNormalValue => Assemble(0, 1, 0);

    /// <summary>
    /// Disassemble the Half into its bitwise components.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Half-precision_floating-point_format" />
    public static (byte signBit, ushort expBits, ulong fracBits) Disassemble(this Half x)
    {
        return x.Disassemble<Half>();
    }

    /// <summary>
    /// Assemble a new Half from parts.
    /// </summary>
    /// <param name="signBit">The sign bit (1 or 0).</param>
    /// <param name="expBits">The exponent bits.</param>
    /// <param name="fracBits">The fraction bits.</param>
    /// <returns>The new Half.</returns>
    public static Half Assemble(byte signBit, ushort expBits, ulong fracBits)
    {
        return XFloatingPoint.Assemble<Half>(signBit, expBits, fracBits);
    }

    /// <summary>
    /// Get a random Half.
    /// </summary>
    public static Half GetRandom()
    {
        Random rnd = new ();
        while (true)
        {
            var bits = XShort.GetRandom();
            var d = BitConverter.Int16BitsToHalf(bits);
            if (Half.IsFinite(d))
            {
                return d;
            }
        }
    }
}
