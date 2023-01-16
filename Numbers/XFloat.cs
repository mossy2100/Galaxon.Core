namespace Galaxon.Core.Numbers;

/// <summary>Extension methods and other bonus stuff for float.</summary>
public static class XFloat
{
    /// <summary>The number of bits in the exponent.</summary>
    public const byte NumExpBits = 8;

    /// <summary>The number of bits in the fraction.</summary>
    public const byte NumFracBits = 23;

    /// <summary>The minimum binary exponent supported by the type.</summary>
    public const short MinExp = -126;

    /// <summary>The maximum binary exponent supported by the type.</summary>
    public const short MaxExp = 127;

    /// <summary>
    /// The maximum positive subnormal value.
    /// </summary>
    public static float MaxPosSubnormalValue => Assemble(0, 0, 0x7f_ffff);

    /// <summary>
    /// The minimum positive normal value.
    /// </summary>
    public static float MinPosNormalValue => Assemble(0, 1, 0);

    /// <summary>
    /// Disassemble the float into its bitwise components.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Single-precision_floating-point_format" />
    public static (byte signBit, ushort expBits, ulong fracBits) Disassemble(this float x) =>
        x.Disassemble<float>();

    /// <summary>
    /// Assemble a new float from parts.
    /// </summary>
    /// <param name="signBit">The sign bit (1 or 0).</param>
    /// <param name="expBits">The exponent bits.</param>
    /// <param name="fracBits">The fraction bits.</param>
    /// <returns>The new float.</returns>
    public static float Assemble(byte signBit, ushort expBits, ulong fracBits) =>
        XFloatingPoint.Assemble<float>(signBit, expBits, fracBits);

    /// <summary>
    /// Get a random float.
    /// </summary>
    public static float GetRandom()
    {
        Random rnd = new ();
        while (true)
        {
            int bits = XInt.GetRandom();
            float f = BitConverter.Int32BitsToSingle(bits);
            if (float.IsFinite(f))
            {
                return f;
            }
        }
    }
}
