using System.Runtime.Remoting;
using System.Text;
using Galaxon.Core.Exceptions;

namespace Galaxon.Core.Strings;

/// <summary>
/// Extension methods for String.
/// </summary>
public static class XString
{
    /// <summary>
    /// See if 2 strings are equal, ignoring case.
    /// </summary>
    public static bool EqualsIgnoreCase(this string str1, string? str2) =>
        str1.Equals(str2, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Convert string to int without throwing.
    /// If the string cannot be parsed into an int, return null.
    /// </summary>
    public static int? ToInt(this string str) =>
        int.TryParse(str, out int i) ? i : null;

    /// <summary>
    /// Convert string to double without throwing.
    /// If the string cannot be parsed into a double, return null.
    /// </summary>
    public static double? ToDouble(this string str) =>
        double.TryParse(str, out double d) ? d : null;

    /// <summary>
    /// Convert string to decimal without throwing.
    /// If the string cannot be parsed into a decimal, return null.
    /// </summary>
    public static decimal? ToDecimal(this string str) =>
        decimal.TryParse(str, out decimal m) ? m : null;

    /// <summary>
    /// Checks to see if the string is a palindrome.
    /// </summary>
    public static bool IsPalindrome(this string str) =>
        str == new string(str.Reverse().ToArray());

    /// <summary>
    /// Transform characters in a string into other characters by using a character map.
    /// Example use cases:
    ///   * making a string upper- or lower-case
    ///   * converting lowercase characters to small caps
    ///   * making a string superscript or subscript
    ///   * transliteration/removal of diacritics
    ///   * converting non-alphanumeric characters into hyphens, e.g. for a URL
    /// </summary>
    /// <param name="str">The original string.</param>
    /// <param name="charMap">The character map.</param>
    /// <param name="action">
    /// Code that tells the algorithm what to do if a character is encountered that is not in the
    /// character map.
    ///   0 = Throw an exception (default).
    ///   1 = Skip it, excluding it from the output.
    ///   2 = Keep the original, untransformed character.
    /// </param>
    /// <returns></returns>
    /// <exception cref="ArgumentInvalidException"></exception>
    public static string Transform(this string str, Dictionary<char, char> charMap,
        InvalidCharAction action = 0)
    {
        StringBuilder sb = new ();

        foreach (char c in str)
        {
            // Check for invalid character.
            if (!charMap.ContainsKey(c))
            {
                switch (action)
                {
                    case InvalidCharAction.Throw:
                        throw new ArgumentInvalidException(nameof(str),
                            $"Character '{c}' not found in provided character map.");

                    case InvalidCharAction.Skip:
                        continue;

                    case InvalidCharAction.Keep:
                        sb.Append(c);
                        continue;
                }
            }

            // Append the transformed character.
            sb.Append(charMap[c]);
        }

        return sb.ToString();
    }

    public static string ToSuperscript(this string str,
        InvalidCharAction action = InvalidCharAction.Skip) =>
        str.Transform(SuperAndSubscriptFormatter.SuperscriptChars, action);

    public static string? ToSuperscript(this object obj,
        InvalidCharAction action = InvalidCharAction.Skip) =>
        obj.ToString()?.ToSuperscript();

    public static string ToSubscript(this string str,
        InvalidCharAction action = InvalidCharAction.Skip) =>
        str.Transform(SuperAndSubscriptFormatter.SubscriptChars, action);

    public static string? ToSubscript(this object obj,
        InvalidCharAction action = InvalidCharAction.Skip) =>
        obj.ToString()?.ToSubscript();

    public static string ToSmallCaps(this string str,
        InvalidCharAction action = InvalidCharAction.Keep) =>
        throw new NotImplementedException();

    public static string RemoveDiacritics(this string str,
        InvalidCharAction action = InvalidCharAction.Keep) =>
        throw new NotImplementedException();

    /// <summary>
    /// Construct a new string by repeating a string s n times.
    /// </summary>
    public static string Repeat(string s, int n)
    {
        // Guard.
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Cannot be negative.");
        }

        StringBuilder sb = new ();
        for (int i = 0; i < n; i++)
        {
            sb.Append(s);
        }
        return sb.ToString();
    }
}
