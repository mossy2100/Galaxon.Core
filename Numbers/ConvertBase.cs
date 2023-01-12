using System.Globalization;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Galaxon.Core.Exceptions;

namespace Galaxon.Core.Numbers;

/// <summary>
/// This class supports conversion between integers and strings of digits in a specified base,
/// which can be in the range 2..64.
///
/// NB: "radix" is used as a synonym for "base" in method arguments, because "base" is a keyword in
/// C#.
///
/// All built-in integer types are supported, including Int128, UInt128, and BigInteger.
///
/// Most languages support conversion to and from base 2 to 36, formed from the 10 digits plus the
/// 26 letters in the English alphabet.
///
/// Here I've appended the 28 of the 32 non-alphanumeric (symbol) ASCII characters to this character
/// set in order to extend the range of supported bases to 64. The symbols are appended to the
/// standard 36 digits in ASCII value order. These characters only require 1 byte in UTF-8 and I
/// assume all, or almost all, computer keyboards will support all of them.
///
/// This has been done to add support for base 64, mainly for my own amusement, and to provide
/// an alternative to bog-standard Base64 encoding (see link). The difference here, if it matters,
/// is that the base 64 digit values used here are compatible with hexadecimal digit values, and all
/// other bases.
///
/// The 4 omitted characters are '.' (period), ',' (comma), and '_' (underscore) because these are
/// used as group separators (in different contexts), and '"' (double quote) which is the string
/// delimiter in C#.
///
/// As in hexadecimal literals, upper-case letters have the same value as lower-case letters.
/// Use the parameter "letterCase" to specify for the result to use all lower- or all upper-case
/// letters. See the method documentation for ToBase() for how to use this parameter.
///
/// The default is to use lower-case for all letters except for L (see the Digits constant).
/// Lower-case letters are more easily distinguishable from numerals than upper-case letters.
/// For example:
///   O looks like 0
///   I looks like 1
///   Z looks like 2
///   S looks like 5
///   G looks like 6
///   T looks like 7
///   B looks like 8
/// The only similar problem with lower-case letters is that l looks like 1, so upper-case is used
/// for this letter only. (This is also why we use "L" as a suffix for long literals, as a rule.)
///
/// These days, most fonts, especially those used by IDEs, make it easy enough to distinguish
/// between letters and numbers, so it's not the issue it once was.
/// Multiple coding standards for CSS require lower-case hex digits in color literals.
/// "L" is not a hexadecimal digit, so this behaviour doesn't violate that standard.
/// Other than that, I can't find any standards that mandate one over the other.
///
/// The core methods are ToBase() and FromBase(). In addition, convenience methods are provided in
/// the form of "To" and "From" methods for all bases that are a power of 2:
/// ----------------------------------------------------
///    Numeral system          Base   Abbreviation
/// ----------------------------------------------------
///     binary                   2        Bin
///     quaternary               4        Quat
///     octal                    8        Oct
///     hexadecimal             16        Hex
///     triacontakaidecimal*    32        Tria
///     tetrasexagesimal        64        Tetra
/// ----------------------------------------------------
/// *Base 32 is more correctly called duotrigesimal. However, there are multiple methods in use for
/// encoding base 32 digits; the one used here is called triacontakaidecimal (see link below), also
/// known as base32hex. It's the same encoding used in Java in JavaScript.
/// </summary>
/// <see href="https://en.wikipedia.org/wiki/List_of_numeral_systems" />
/// <see href="https://en.wikipedia.org/wiki/Binary_number" />
/// <see href="https://en.wikipedia.org/wiki/Quaternary_numeral_system" />
/// <see href="https://en.wikipedia.org/wiki/Octal" />
/// <see href="https://en.wikipedia.org/wiki/Hexadecimal" />
/// <see href="https://en.wikipedia.org/wiki/Base32" />
/// <see href="https://en.wikipedia.org/wiki/Base64" />
public static class ConvertBase
{
    #region Constants

    /// <summary>The minimum base supported by the type.</summary>
    public const int MinBase = 2;

    /// <summary>The maximum base supported by the type.</summary>
    public const int MaxBase = 64;

    /// <summary>Valid digits as a string, supporting up to base 64.</summary>
    public const string Digits =
        "0123456789abcdefghijkLmnopqrstuvwxyz!#$%&'()*+-/:;<=>?@[\\]^`{|}~";

    #endregion Constants

    #region Extension methods

    /// <summary>
    /// Convert an integer to a string of digits in a given base.
    ///
    /// Note, a negative value will be converted to a non-negative value with the same underlying
    /// bits. This reflects the behaviour of other base-conversion methods in .NET.
    /// </summary>
    /// <param name="n">The instance value.</param>
    /// <param name="radix">The base to convert to.</param>
    /// <param name="width">The minimum number of digits.</param>
    /// <param name="letterCase">
    /// If letters should be lower-case, upper-case, or default.
    ///     null  = default (all lower-case except for L; see Digits)
    ///     true  = upper-case
    ///     false = lower-case
    /// </param>
    /// <typeparam name="T">The integer type.</typeparam>
    /// <returns>The string of digits.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If the base is out of range.</exception>
    /// <exception cref="ArgumentInvalidException"> If T is an unsupported type.</exception>
    public static string ToBase<T>(this T n, byte radix, int width = 1, bool? letterCase = null)
        where T : IBinaryInteger<T>
    {
        // Check the base is valid.
        CheckBase(radix);

        // Check for zero.
        if (n == T.Zero)
        {
            return "0".PadLeft(width, '0');
        }

        // Convert value to BigInteger. It's much easier to work with BigInteger than T, and any
        // integer type can be converted to BigInteger.
        if (n is not BigInteger bi)
        {
            // We can't cast from T to BigInteger but we can do it with strings (decimal digits).
            if (n.ToString() is not { } strN)
            {
                throw new ArgumentInvalidException(typeof(T).Name, "Unsupported type.");
            }
            bi = BigInteger.Parse(strN);
        }

        // Check for negative value.
        if (bi < 0)
        {
            return "-" + (-bi).ToBase(radix, width, letterCase);
        }

        // Build the output string.
        StringBuilder sbDigits = new ();

        while (true)
        {
            // Get the next digit.
            BigInteger rem = bi % radix;
            sbDigits.Insert(0, Digits[(int)rem]);

            // Check if we're done.
            bi -= rem;
            if (bi == 0)
            {
                break;
            }

            // Prepare for next iteration.
            bi /= radix;
        }

        string result = sbDigits.ToString().PadLeft(width, '0');

        // Transform the result case if necessary.
        return letterCase switch
        {
            true => result.ToUpper(),
            false => result.ToLower(),
            null => result
        };
    }

    /// <summary>
    /// Variation of ToBase() that allows the user to omit the width parameter.
    /// </summary>
    /// <param name="n">The instance value.</param>
    /// <param name="radix">The base to convert to.</param>
    /// <param name="letterCase">If letters should be lower-case, upper-case, or default.</param>
    /// <typeparam name="T">The integer type.</typeparam>
    /// <returns>The string of digits.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If the base is out of range.</exception>
    /// <exception cref="ArgumentInvalidException"> If T is an unsupported type.</exception>
    public static string ToBase<T>(this T n, byte radix, bool? letterCase)
        where T : IBinaryInteger<T> =>
        n.ToBase(radix, 1, letterCase);

    /// <summary>
    /// Convert an integer to binary digits.
    /// </summary>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">
    /// The minimum number of digits in the result. Extra 0s will be prepended if necessary to make
    /// up the width.
    /// </param>
    /// <returns>The value as a string of binary digits.</returns>
    public static string ToBin<T>(this T n, int width = 1) where T : IBinaryInteger<T> =>
        ToBase(n, 2, width);

    /// <summary>
    /// Convert integer to quaternary digits.
    /// </summary>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">
    /// The minimum number of digits in the result. Extra 0s will be prepended if necessary to make
    /// up the width.
    /// </param>
    /// <returns>The value as a string of quaternary digits.</returns>
    public static string ToQuat<T>(this T n, int width = 1) where T : IBinaryInteger<T> =>
        ToBase(n, 4, width);

    /// <summary>
    /// Convert integer to octal digits.
    /// </summary>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">
    /// The minimum number of digits in the result. Extra 0s will be prepended if necessary to make
    /// up the width.
    /// </param>
    /// <returns>The value as a string of octal digits.</returns>
    public static string ToOct<T>(this T n, int width = 1) where T : IBinaryInteger<T> =>
        ToBase(n, 8, width);

    /// <summary>
    /// Convert integer to hexadecimal digits.
    /// </summary>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">
    /// The minimum number of digits in the result. Extra 0s will be prepended if necessary to make
    /// up the width.
    /// </param>
    /// <param name="letterCase">If letters should be lower-case, upper-case, or default.</param>
    /// <returns>The value as a string of hexadecimal digits.</returns>
    public static string ToHex<T>(this T n, int width = 1, bool? letterCase = null)
        where T : IBinaryInteger<T> =>
        ToBase(n, 16, width, letterCase);

    /// <summary>
    /// Convert integer to triacontakaidecimal digits.
    /// </summary>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">
    /// The minimum number of digits in the result. Extra 0s will be prepended if necessary to make
    /// up the width.
    /// </param>
    /// <param name="letterCase">If letters should be lower-case, upper-case, or default.</param>
    /// <returns>The value as a string of triacontakaidecimal digits.</returns>
    public static string ToTria<T>(this T n, int width = 1, bool? letterCase = null)
        where T : IBinaryInteger<T> =>
        ToBase(n, 32, width, letterCase);

    /// <summary>
    /// Convert integer to tetrasexagesimal digits.
    /// </summary>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">
    /// The minimum number of digits in the result. Extra 0s will be prepended if necessary to make
    /// up the width.
    /// </param>
    /// <param name="letterCase">If letters should be lower-case, upper-case, or default.</param>
    /// <returns>The value as a string of tetrasexagesimal digits.</returns>
    public static string ToTetra<T>(this T n, int width = 1, bool? letterCase = null)
        where T : IBinaryInteger<T> =>
        ToBase(n, 64, width, letterCase);

    #endregion Extension methods

    #region Regular static methods

    /// <summary>
    /// Convert a string of digits in a given base into a int.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// If the digits string is null, empty, or whitespace.
    /// </exception>
    /// <exception cref="ArgumentFormatException">
    /// If the digits string contains invalid characters.
    /// </exception>
    /// <exception cref="OverflowException">
    /// If the value is out of range for the given type.
    /// </exception>
    public static T FromBase<T>(string digits, byte radix) where T : IBinaryInteger<T>
    {
        // Check the input string != null, empty, or whitespace.
        if (string.IsNullOrWhiteSpace(digits))
        {
            throw new ArgumentNullException(nameof(digits), "Cannot be empty or whitespace.");
        }

        // Check the base is valid. This could throw an ArgumentOutOfRangeException.
        CheckBase(radix);

        // Remove/ignore whitespace and group separator characters.
        digits = Regex.Replace(digits, @"[\s.,_\u2009]", "");

        // Get a map of valid digits to their value.
        Dictionary<char, byte> digitValues = GetDigitValues(radix);

        // Do the conversion.
        BigInteger value = 0;
        foreach (char c in digits)
        {
            // Try to get the character value from the map.
            if (!digitValues.TryGetValue(c, out byte digitValue))
            {
                char[] digitChars = digitValues.Select(kvp => kvp.Key).ToArray();
                string digitList = string.Join(", ", digitChars[..^1]) + " and " + digitChars[^1];
                throw new ArgumentFormatException(nameof(digits),
                    $"A string representing a number in base {radix} may only include the digits {digitList}.");
            }

            // Add it to the result.
            value = value * radix + digitValue;
        }

        // Try to convert the value to the target type.
        try
        {
            T result = T.Zero;
            return result switch
            {
                sbyte => (T)(object)(sbyte)value,
                byte => (T)(object)(byte)value,
                short => (T)(object)(short)value,
                ushort => (T)(object)(ushort)value,
                int => (T)(object)(int)value,
                uint => (T)(object)(uint)value,
                long => (T)(object)(long)value,
                ulong => (T)(object)(ulong)value,
                Int128 => (T)(object)(Int128)value,
                UInt128 => (T)(object)(UInt128)value,
                BigInteger => (T)(object)value,
                _ => throw new InvalidCastException($"The type {typeof(T).Name} is not supported.")
            };
        }
        catch (OverflowException ex)
        {
            throw new OverflowException(
                $"The provided digit string does not represent a valid {typeof(T).Name}.", ex);
        }
    }

    /// <summary>
    /// Convert a string of binary digits into an integer.
    /// </summary>
    /// <param name="digits">The string of digits to parse.</param>
    /// <typeparam name="T">The integer type to create.</typeparam>
    /// <returns>The integer equivalent of the digits.</returns>
    public static T FromBin<T>(string digits) where T : IBinaryInteger<T> =>
        FromBase<T>(digits, 2);

    /// <summary>
    /// Convert a string of quaternary digits into an integer.
    /// </summary>
    /// <param name="digits">The string of digits to parse.</param>
    /// <typeparam name="T">The integer type to create.</typeparam>
    /// <returns>The integer equivalent of the digits.</returns>
    public static T FromQuat<T>(string digits) where T : IBinaryInteger<T> =>
        FromBase<T>(digits, 4);

    /// <summary>
    /// Convert a string of octal digits into an integer.
    /// </summary>
    /// <param name="digits">The string of digits to parse.</param>
    /// <typeparam name="T">The integer type to create.</typeparam>
    /// <returns>The integer equivalent of the digits.</returns>
    public static T FromOct<T>(string digits) where T : IBinaryInteger<T> =>
        FromBase<T>(digits, 8);

    /// <summary>
    /// Convert a string of hexadecimal digits into an integer.
    /// </summary>
    /// <param name="digits">The string of digits to parse.</param>
    /// <typeparam name="T">The integer type to create.</typeparam>
    /// <returns>The integer equivalent of the digits.</returns>
    public static T FromHex<T>(string digits) where T : IBinaryInteger<T> =>
        FromBase<T>(digits, 16);

    /// <summary>
    /// Convert a string of triacontakaidecimal digits into an integer.
    /// </summary>
    /// <param name="digits">The string of digits to parse.</param>
    /// <typeparam name="T">The integer type to create.</typeparam>
    /// <returns>The integer equivalent of the digits.</returns>
    public static T FromTria<T>(string digits) where T : IBinaryInteger<T> =>
        FromBase<T>(digits, 32);

    /// <summary>
    /// Convert a string of tetrasexagesimal digits into an integer.
    /// </summary>
    /// <param name="digits">The string of digits to parse.</param>
    /// <typeparam name="T">The integer type to create.</typeparam>
    /// <returns>The integer equivalent of the digits.</returns>
    public static T FromTetra<T>(string digits) where T : IBinaryInteger<T> =>
        FromBase<T>(digits, 64);

    #endregion Regular static methods

    #region Helper methods

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
            char c = Digits[i];
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

    #endregion Helper methods
}
