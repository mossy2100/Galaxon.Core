﻿using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Galaxon.Core.Exceptions;
using Galaxon.Core.Types;

namespace Galaxon.Core.Numbers;

/// <summary>
/// This class supports conversion between integers and strings of digits in a specified base,
/// which can be in the range 2..36.
///
/// All built-in integer types are supported, including Int128, UInt128, and BigInteger.
///
/// As in hexadecimal literals, upper-case letters have the same value as lower-case letters.
/// Lower-case is the default for output strings.
/// In the ToBase(), ToHex(), and ToTria() methods, the "upper" parameter can be used to specify if
/// the result should use upper-case letters instead.
/// Lower-case letters are the default because upper-case letters are more easily confused with
/// numerals than lower-case. For example:
/// - 'O' can look like '0'
/// - 'I' can look like '1'
/// - 'Z' can look like '2'
/// - 'S' can look like '5'
/// - 'G' can look like '6'
/// - 'B' can look like '8'
/// The only similar problem with lower-case letters is that 'l' can look like '1'. However, these
/// days, most fonts, especially those used by IDEs, are chosen so that it's easy to distinguish
/// between letters and numbers, so it's not the issue it once was.
/// Multiple coding standards for CSS require lower-case hex digits in color literals.
/// Other than that, I can't find any standards that mandate one over the other. It seems upper-case
/// is favoured in older languages, lower-case in newer.
///
/// The core methods are ToBase() and FromBase(). In addition, convenience methods are provided for
/// all bases that are powers of 2, which are the most commonly used:
/// |------------------|------------------------|--------|--------------------------|
/// |  Bits per digit  |  Numeral system        |  Base  |  Methods                 |
/// |------------------|------------------------|--------|--------------------------|
/// |        1         |  binary                |    2   |  ToBin()    FromBin()    |
/// |        2         |  quaternary            |    4   |  ToQuat()   FromQuat()   |
/// |        3         |  octal                 |    8   |  ToOct()    FromOct()    |
/// |        4         |  hexadecimal           |   16   |  ToHex()    FromHex()    |
/// |        5         |  triacontakaidecimal * |   32   |  ToTria()   FromTria()   |
/// |------------------|------------------------|--------|--------------------------|
/// * The general term for base 32 is "duotrigesimal". However, there are multiple methods in use
/// for encoding base 32 digits; the one used here is called "triacontakaidecimal" (see link below),
/// also known as "base32hex". It's the same encoding used in Java in JavaScript. The term is
/// abbreviated here as "Tria" for the sake of the ToTria() and FromTria() methods.
/// </summary>
/// <see href="https://en.wikipedia.org/wiki/List_of_numeral_systems"/>
/// <see href="https://en.wikipedia.org/wiki/Binary_number"/>
/// <see href="https://en.wikipedia.org/wiki/Quaternary_numeral_system"/>
/// <see href="https://en.wikipedia.org/wiki/Octal"/>
/// <see href="https://en.wikipedia.org/wiki/Hexadecimal"/>
/// <see href="https://en.wikipedia.org/wiki/Base32"/>
public static class ConvertBase
{
    #region Constants

    /// <summary>The minimum base supported by the type.</summary>
    public const int MinBase = 2;

    /// <summary>The maximum base supported by the type.</summary>
    public const int MaxBase = 36;

    /// <summary>Valid digit characters.</summary>
    public const string Digits = "0123456789abcdefghijklmnopqrstuvwxyz";

    #endregion Constants

    #region Extension methods

    /// <summary>
    /// Convert an integer to a string of digits in a given base.
    /// Note, a negative value will be converted to a non-negative value with the same underlying
    /// bits. This reflects the behaviour of other base-conversion methods in .NET.
    /// </summary>
    /// <typeparam name="T">The integer type.</typeparam>
    /// <param name="n">The instance value.</param>
    /// <param name="toBase">The base to convert to.</param>
    /// <param name="width">
    /// The minimum number of digits in the result. Any value less than 2 will have no effect.
    /// The result will be padded with '0' characters on the left as needed.
    /// This width does not include the '-' character if needed for a negative value. It only refers
    /// to the minimum number of digits.
    /// </param>
    /// <param name="upper">
    /// If letters should be lower or upper-case.
    ///   false = lower-case (default)
    ///   true  = upper-case
    /// </param>
    /// <returns>The string of digits.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If the base is out of range.</exception>
    /// <exception cref="ArgumentInvalidException"> If T is an unsupported type.</exception>
    public static string ToBase<T>(this T n, byte toBase, int width = 1, bool upper = false)
        where T : IBinaryInteger<T>
    {
        // Check the base is valid.
        CheckBase(toBase);

        // Make sure width is valid.
        if (width < 1)
        {
            width = 1;
        }

        // Initialize the result.
        var result = "";

        // Check for zero.
        if (n != T.Zero)
        {
            // Convert value to BigInteger. It's much easier to work with BigInteger than T, and any
            // integer type can be converted to BigInteger.
            if (n is not BigInteger bi)
            {
                bi = XReflection.Cast<T, BigInteger>(n);
            }

            // Check for negative value.
            if (bi < 0)
            {
                return "-" + (-bi).ToBase(toBase, width, upper);
            }

            // Build the output string.
            StringBuilder sbDigits = new ();
            while (true)
            {
                // Get the next digit.
                var rem = bi % toBase;
                sbDigits.Insert(0, Digits[(int)rem]);

                // Check if we're done.
                bi -= rem;
                if (bi == 0) break;

                // Prepare for next iteration.
                bi /= toBase;
            }
            result = sbDigits.ToString();

            // Transform the result case if necessary.
            if (upper)
            {
                result = result.ToUpper();
            }
        }

        return result.PadLeft(width, '0');
    }

    /// <summary>Convert an integer to binary digits.</summary>
    /// <typeparam name="T">The integer type.</typeparam>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">The minimum number of digits in the result.</param>
    /// <returns>The value as a string of binary digits.</returns>
    public static string ToBin<T>(this T n, int width = 1) where T : IBinaryInteger<T>
    {
        return ToBase(n, 2, width);
    }

    /// <summary>Convert integer to quaternary digits.</summary>
    /// <typeparam name="T">The integer type.</typeparam>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">The minimum number of digits in the result.</param>
    /// <returns>The value as a string of quaternary digits.</returns>
    public static string ToQuat<T>(this T n, int width = 1) where T : IBinaryInteger<T>
    {
        return ToBase(n, 4, width);
    }

    /// <summary>Convert integer to octal digits.</summary>
    /// <typeparam name="T">The integer type.</typeparam>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">The minimum number of digits in the result.</param>
    /// <returns>The value as a string of octal digits.</returns>
    public static string ToOct<T>(this T n, int width = 1) where T : IBinaryInteger<T>
    {
        return ToBase(n, 8, width);
    }

    /// <summary>Convert integer to hexadecimal digits.</summary>
    /// <typeparam name="T">The integer type.</typeparam>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">The minimum number of digits in the result.</param>
    /// <param name="upper">If letters should be upper-case (false = lower, true = upper).</param>
    /// <returns>The value as a string of hexadecimal digits.</returns>
    public static string ToHex<T>(this T n, int width = 1, bool upper = false) where T : IBinaryInteger<T>
    {
        return ToBase(n, 16, width, upper);
    }

    /// <summary>Convert integer to triacontakaidecimal (base 32) digits.</summary>
    /// <typeparam name="T">The integer type.</typeparam>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">The minimum number of digits in the result.</param>
    /// <param name="upper">If letters should be upper-case (false = lower, true = upper).</param>
    /// <returns>The value as a string of triacontakaidecimal digits.</returns>
    public static string ToTria<T>(this T n, int width = 1, bool upper = false)
        where T : IBinaryInteger<T>
    {
        return ToBase(n, 32, width, upper);
    }

    #endregion Extension methods

    #region Static conversion methods

    /// <summary>
    /// Convert a string of digits in a given base into a int.
    /// Group separator characters, including spaces, newlines, thin spaces, periods, commas, and
    /// underscores, will be ignored.
    /// </summary>
    /// <typeparam name="T">The integer type to create.</typeparam>
    /// <param name="digits">A string of digits in the specified base.</param>
    /// <param name="fromBase">The base that the digits in the string are in.</param>
    /// <returns>The integer equivalent of the digits.</returns>
    /// <exception cref="ArgumentNullException">If string is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentFormatException">
    /// If string contains invalid characters for the specified base.
    /// </exception>
    /// <exception cref="InvalidCastException">If a cast to the target type failed.</exception>
    /// <exception cref="OverflowException">
    /// If the resulting value is out of range for the target type.
    /// </exception>
    public static T FromBase<T>(string digits, byte fromBase) where T : IBinaryInteger<T>
    {
        // Check the input string != null, empty, or whitespace.
        if (string.IsNullOrWhiteSpace(digits))
        {
            throw new ArgumentNullException(nameof(digits), "Cannot be empty or whitespace.");
        }

        // Check the base is valid. This could throw an ArgumentOutOfRangeException.
        CheckBase(fromBase);

        // Remove/ignore whitespace, decimal points, and digit group separator characters.
        digits = Regex.Replace(digits, $@"[\s.,_]", "");

        // See if the value is negative.
        var sign = 1;
        if (digits[0] == '-')
        {
            sign = -1;
            // Remove the minus sign.
            digits = digits[1..];
        }

        // Get a map of valid digits to their value.
        var digitValues = GetDigitValues(fromBase);

        // Do the conversion.
        BigInteger value = 0;
        foreach (var c in digits)
        {
            // Try to get the character value from the map.
            if (!digitValues.TryGetValue(c, out var digitValue))
            {
                var digitChars = digitValues.Select(kvp => kvp.Key).ToArray();
                var digitList = string.Join(", ", digitChars[..^1]) + " and " + digitChars[^1];
                throw new ArgumentFormatException(nameof(digits),
                    $"A string representing a number in base {fromBase} may only include the digits {digitList}.");
            }

            // Add it to the result.
            value = value * fromBase + digitValue;
        }
        // Make the result negative or positive as needed.
        value *= sign;

        // Try to convert the value to the target type.
        var t = typeof(T);
        try
        {
            return XReflection.Cast<BigInteger, T>(value);
            // if (t == typeof(sbyte))
            // {
            //     return (T)(object)(sbyte)value;
            // }
            // if (t == typeof(byte))
            // {
            //     return (T)(object)(byte)value;
            // }
            // if (t == typeof(short))
            // {
            //     return (T)(object)(short)value;
            // }
            // if (t == typeof(ushort))
            // {
            //     return (T)(object)(ushort)value;
            // }
            // if (t == typeof(int))
            // {
            //     return (T)(object)(int)value;
            // }
            // if (t == typeof(uint))
            // {
            //     return (T)(object)(uint)value;
            // }
            // if (t == typeof(long))
            // {
            //     return (T)(object)(long)value;
            // }
            // if (t == typeof(ulong))
            // {
            //     return (T)(object)(ulong)value;
            // }
            // if (t == typeof(Int128))
            // {
            //     return (T)(object)(Int128)value;
            // }
            // if (t == typeof(UInt128))
            // {
            //     return (T)(object)(UInt128)value;
            // }
            // if (t == typeof(BigInteger))
            // {
            //     return (T)(object)value;
            // }
            // throw new InvalidCastException($"The type {t.Name} is unsupported.");
        }
        catch (OverflowException ex)
        {
            throw new OverflowException(
                $"The integer represented by the string is outside the valid range for {t.Name}.", ex);
        }
    }

    /// <summary>
    /// Convert a string of binary digits into an integer.
    /// </summary>
    /// <typeparam name="T">The integer type to create.</typeparam>
    /// <param name="digits">The string of digits to parse.</param>
    /// <returns>The integer equivalent of the digits.</returns>
    public static T FromBin<T>(string digits) where T : IBinaryInteger<T>
    {
        return FromBase<T>(digits, 2);
    }

    /// <summary>
    /// Convert a string of quaternary digits into an integer.
    /// </summary>
    /// <typeparam name="T">The integer type to create.</typeparam>
    /// <param name="digits">The string of digits to parse.</param>
    /// <returns>The integer equivalent of the digits.</returns>
    public static T FromQuat<T>(string digits) where T : IBinaryInteger<T>
    {
        return FromBase<T>(digits, 4);
    }

    /// <summary>
    /// Convert a string of octal digits into an integer.
    /// </summary>
    /// <typeparam name="T">The integer type to create.</typeparam>
    /// <param name="digits">The string of digits to parse.</param>
    /// <returns>The integer equivalent of the digits.</returns>
    public static T FromOct<T>(string digits) where T : IBinaryInteger<T>
    {
        return FromBase<T>(digits, 8);
    }

    /// <summary>
    /// Convert a string of hexadecimal digits into an integer.
    /// </summary>
    /// <typeparam name="T">The integer type to create.</typeparam>
    /// <param name="digits">The string of digits to parse.</param>
    /// <returns>The integer equivalent of the digits.</returns>
    public static T FromHex<T>(string digits) where T : IBinaryInteger<T>
    {
        return FromBase<T>(digits, 16);
    }

    /// <summary>
    /// Convert a string of triacontakaidecimal digits into an integer.
    /// </summary>
    /// <typeparam name="T">The integer type to create.</typeparam>
    /// <param name="digits">The string of digits to parse.</param>
    /// <returns>The integer equivalent of the digits.</returns>
    public static T FromTria<T>(string digits) where T : IBinaryInteger<T>
    {
        return FromBase<T>(digits, 32);
    }

    #endregion Static conversion methods

    #region Private helper methods

    /// <summary>
    /// Checks if the provided base is valid for use by this class.
    /// </summary>
    /// <param name="radix">The base to check.</param>
    /// <exception cref="ArgumentOutOfRangeException">If the base is out of range.</exception>
    private static void CheckBase(byte radix)
    {
        if (radix is < MinBase or > MaxBase)
        {
            throw new ArgumentOutOfRangeException(nameof(radix),
                $"Value must be in the range {MinBase}..{MaxBase}.");
        }
    }

    /// <summary>
    /// Get the values of valid digits for the specified base.
    /// </summary>
    /// <param name="radix">The base.</param>
    /// <returns>The map of digit characters to their values.</returns>
    private static Dictionary<char, byte> GetDigitValues(byte radix)
    {
        Dictionary<char, byte> digitValues = new ();
        for (byte i = 0; i < radix; i++)
        {
            var c = Digits[i];
            if (char.IsLetter(c))
            {
                // Add both the upper- and lower-case variants.
                digitValues[char.ToUpper(c)] = i;
                digitValues[char.ToLower(c)] = i;
            }
            else
            {
                digitValues[c] = i;
            }
        }
        return digitValues;
    }

    #endregion Private helper methods
}
