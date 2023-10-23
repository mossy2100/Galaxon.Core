using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Galaxon.Core.Exceptions;

namespace Galaxon.Core.Numbers;

/// <summary>
/// This class supports conversion between integers and strings of digits in a specified base,
/// which can be in the range 2..64.
///
/// All built-in integer types are supported, including Int128, UInt128, and BigInteger.
///
/// Most languages support conversion to and from base 2 to 36, formed from the 10 digits plus the
/// 26 letters in the English alphabet. To this sequence I've appended 28 non-alphanumeric (symbol)
/// ASCII characters in order to extend the range of supported bases to 64. The symbols are appended
/// to the standard 36 digits in ASCII value order. These characters only require 1 byte in UTF-8
/// and I assume all, or almost all, computer keyboards will support all of them.
/// This has been done to add support for base 64, mainly for my own amusement, and to provide
/// an alternative to bog-standard Base64 encoding (see link).
///
/// The most important difference is that this encoding uses 8 bits per base-64 digit, whereas
/// standard base64 encoding uses 5 bits per digit, which is more compressed.
/// Another difference is that the base 64 digits used here are compatible with the digits used by
/// hexadecimal and all other bases.
///
/// ASCII provides 32 symbol (or punctuation) characters. The 4 unused characters are '.' (period),
/// ',' (comma), and '_' (underscore), which can be used as group separators (in
/// different contexts), and '"' (double quote) which is the string delimiter in C#.
///
/// As in hexadecimal literals, upper-case letters have the same value as lower-case letters.
/// Use the parameter "letterCase" to specify for the result to use all lower- or all upper-case
/// letters. See the method documentation for ToBase() for how to use this parameter.
/// The default is to use lower-case for all letters except for L (see the Digits constant).
/// Upper-case letters are more easily confused with numerals than lower-case. For example:
/// - 'O' can look like '0'
/// - 'I' can look like '1'
/// - 'Z' can look like '2'
/// - 'S' can look like '5'
/// - 'G' can look like '6'
/// - 'B' can look like '8'
/// The only similar problem with lower-case letters is that 'l' can look like '1'. To solve this,
/// upper-case is used for this letter only. (This is the same reason why we use "L" as a suffix for
/// long literals in C#, as a rule.)
/// These days, most fonts, especially those used by IDEs, make it easy enough to distinguish
/// between letters and numbers, so it's not the issue it once was.
/// Multiple coding standards for CSS require lower-case hex digits in color literals.
/// "L" is not a hexadecimal digit, so this behaviour doesn't violate that standard.
/// Other than that, I can't find any standards that mandate one over the other. It seems upper-case
/// is favoured in older languages, lower-case in newer.
///
/// The core methods are ToBase() and FromBase(). In addition, convenience methods are provided in
/// the form of "To" and "From" methods for all bases that are a power of 2, and base 10 (decimal),
/// which are the most commonly used:
/// |------------------|------------------------|--------|--------------------------|
/// |  Bits per digit  |  Numeral system        |  Base  |  Methods                 |
/// |------------------|------------------------|--------|--------------------------|
/// |        1         |  binary                |    2   |  ToBin()    FromBin()    |
/// |        2         |  quaternary            |    4   |  ToQuat()   FromQuat()   |
/// |        3         |  octal                 |    8   |  ToOct()    FromOct()    |
/// |                  |  decimal               |   10   |  ToDec()    FromDec()    |
/// |        4         |  hexadecimal           |   16   |  ToHex()    FromHex()    |
/// |        5         |  triacontakaidecimal * |   32   |  ToTria()   FromTria()   |
/// |        6         |  tetrasexagesimal **   |   64   |  ToTetra()  FromTetra()  |
/// |------------------|------------------------|--------|--------------------------|
///
/// * The term for base 32 is "duotrigesimal". However, there are multiple methods in use for
/// encoding base 32 digits; the one used here is called "triacontakaidecimal" (see link below),
/// also known as "base32hex". It's the same encoding used in Java in JavaScript. The term is
/// abbreviated here as "Tria" in the names of the conversion methods.
///
/// ** The term for Base 64 is "tetrasexagesimal". The term is abbreviated here as "Tetra" in the
/// names of the conversion methods.
///
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

    /// <summary>Unicode thin space character. Can be useful for formatting numbers.</summary>
    public const char ThinSpace = '\u2009';

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
    /// <param name="letterCase">
    /// If letters should be lower-case, upper-case, or default.
    /// null  = default (all lower-case except for L; see Digits)
    /// true  = upper-case
    /// false = lower-case
    /// </param>
    /// <returns>The string of digits.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If the base is out of range.</exception>
    /// <exception cref="ArgumentInvalidException"> If T is an unsupported type.</exception>
    public static string ToBase<T>(this T n, byte toBase, bool? letterCase = null)
        where T : IBinaryInteger<T>
    {
        // Check the base is valid.
        CheckBase(toBase);

        // Check for zero.
        if (n == T.Zero)
        {
            return "0";
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
            return "-" + (-bi).ToBase(toBase, letterCase);
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
            if (bi == 0)
            {
                break;
            }

            // Prepare for next iteration.
            bi /= toBase;
        }

        var result = sbDigits.ToString();

        // Transform the result case if necessary.
        return letterCase switch
        {
            true => result.ToUpper(),
            false => result.ToLower(),
            null => result,
        };
    }

    /// <summary>Convert an integer to binary digits.</summary>
    /// <typeparam name="T">The integer type.</typeparam>
    /// <param name="n">The integer to convert.</param>
    /// <returns>The value as a string of binary digits.</returns>
    public static string ToBin<T>(this T n) where T : IBinaryInteger<T>
    {
        return ToBase(n, 2);
    }

    /// <summary>Convert integer to quaternary digits.</summary>
    /// <typeparam name="T">The integer type.</typeparam>
    /// <param name="n">The integer to convert.</param>
    /// <returns>The value as a string of quaternary digits.</returns>
    public static string ToQuat<T>(this T n) where T : IBinaryInteger<T>
    {
        return ToBase(n, 4);
    }

    /// <summary>Convert integer to octal digits.</summary>
    /// <typeparam name="T">The integer type.</typeparam>
    /// <param name="n">The integer to convert.</param>
    /// <returns>The value as a string of octal digits.</returns>
    public static string ToOct<T>(this T n) where T : IBinaryInteger<T>
    {
        return ToBase(n, 8);
    }

    /// <summary>Convert an integer to decimal digits.</summary>
    /// <typeparam name="T">The integer type.</typeparam>
    /// <param name="n">The integer to convert.</param>
    /// <returns>The value as a string of decimal digits.</returns>
    public static string ToDec<T>(this T n) where T : IBinaryInteger<T>
    {
        return ToBase(n, 10);
    }

    /// <summary>Convert integer to hexadecimal digits.</summary>
    /// <typeparam name="T">The integer type.</typeparam>
    /// <param name="n">The integer to convert.</param>
    /// <param name="letterCase">If letters should be lower-case, upper-case, or default.</param>
    /// <returns>The value as a string of hexadecimal digits.</returns>
    public static string ToHex<T>(this T n, bool? letterCase = null) where T : IBinaryInteger<T>
    {
        return ToBase(n, 16, letterCase);
    }

    /// <summary>Convert integer to triacontakaidecimal (base 32) digits.</summary>
    /// <typeparam name="T">The integer type.</typeparam>
    /// <param name="n">The integer to convert.</param>
    /// <param name="letterCase">If letters should be lower-case, upper-case, or default.</param>
    /// <returns>The value as a string of triacontakaidecimal digits.</returns>
    public static string ToTria<T>(this T n, bool? letterCase = null)
        where T : IBinaryInteger<T>
    {
        return ToBase(n, 32, letterCase);
    }

    /// <summary>Convert integer to tetrasexagesimal (base 64) digits.</summary>
    /// <typeparam name="T">The integer type.</typeparam>
    /// <param name="n">The integer to convert.</param>
    /// <param name="letterCase">If letters should be lower-case, upper-case, or default.</param>
    /// <returns>The value as a string of tetrasexagesimal digits.</returns>
    public static string ToTetra<T>(this T n, bool? letterCase = null)
        where T : IBinaryInteger<T>
    {
        return ToBase(n, 64, letterCase);
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

        // Remove/ignore whitespace, group separator characters, and double quotes.
        digits = Regex.Replace(digits, $@"[\s.,_{ThinSpace}""]", "");

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

        // Try to convert the value to the target type.
        var t = typeof(T);
        try
        {
            if (t == typeof(sbyte))
            {
                return (T)(object)(sbyte)value;
            }
            if (t == typeof(byte))
            {
                return (T)(object)(byte)value;
            }
            if (t == typeof(short))
            {
                return (T)(object)(short)value;
            }
            if (t == typeof(ushort))
            {
                return (T)(object)(ushort)value;
            }
            if (t == typeof(int))
            {
                return (T)(object)(int)value;
            }
            if (t == typeof(uint))
            {
                return (T)(object)(uint)value;
            }
            if (t == typeof(long))
            {
                return (T)(object)(long)value;
            }
            if (t == typeof(ulong))
            {
                return (T)(object)(ulong)value;
            }
            if (t == typeof(Int128))
            {
                return (T)(object)(Int128)value;
            }
            if (t == typeof(UInt128))
            {
                return (T)(object)(UInt128)value;
            }
            if (t == typeof(BigInteger))
            {
                return (T)(object)value;
            }

            throw new InvalidCastException($"The type {t.Name} is unsupported.");
        }
        catch (OverflowException ex)
        {
            throw new OverflowException(
                $"The provided digit string does not represent a valid {t.Name}.", ex);
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
    /// Convert a string of decimal digits into an integer.
    /// </summary>
    /// <typeparam name="T">The integer type to create.</typeparam>
    /// <param name="digits">The string of digits to parse.</param>
    /// <returns>The integer equivalent of the digits.</returns>
    public static T FromDec<T>(string digits) where T : IBinaryInteger<T>
    {
        return FromBase<T>(digits, 10);
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

    /// <summary>
    /// Convert a string of tetrasexagesimal digits into an integer.
    /// </summary>
    /// <typeparam name="T">The integer type to create.</typeparam>
    /// <param name="digits">The string of digits to parse.</param>
    /// <returns>The integer equivalent of the digits.</returns>
    public static T FromTetra<T>(string digits) where T : IBinaryInteger<T>
    {
        return FromBase<T>(digits, 64);
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
