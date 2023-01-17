using System.Text;
using System.Text.RegularExpressions;
using AnyAscii;
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
    /// Replace characters in a string with other characters by using a character map.
    /// Example use cases:
    ///   * making a string upper- or lower-case
    ///   * converting lowercase characters to small caps
    ///   * making a string superscript or subscript
    ///   * transliteration/removal of diacritics
    /// </summary>
    /// <param name="str">The original string.</param>
    /// <param name="charMap">The character map.</param>
    /// <param name="action">
    /// Code that tells the algorithm what to do if a character is encountered that is not in the
    /// character map.
    ///   Throw = Throw an exception.
    ///   Skip  = Skip it, excluding it from the output.
    ///   Keep  = Keep the original, untransformed character.
    /// </param>
    /// <returns>The transformed string.</returns>
    /// <exception cref="ArgumentInvalidException">
    /// If the original character isn't found in the character map.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the InvalidCharAction is out of range.
    /// </exception>
    public static string ReplaceChars(this string str, Dictionary<char, string> charMap,
        InvalidCharAction action = InvalidCharAction.Keep)
    {
        StringBuilder sb = new ();

        foreach (char original in str)
        {
            // Get the replacement character if it's there.
            bool mapHasChar = charMap.TryGetValue(original, out string? replacement);

            if (mapHasChar)
            {
                // Append the replacement character.
                sb.Append(replacement);
            }
            else
            {
                // The original character was not in the map.
                switch (action)
                {
                    case InvalidCharAction.Throw:
                        throw new ArgumentInvalidException(nameof(str),
                            $"Character '{original}' not found in the character map.");

                    case InvalidCharAction.Skip:
                        break;

                    case InvalidCharAction.Keep:
                        // Append the original character.
                        sb.Append(original);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(action), action,
                            "Invalid code provided for the invalid character found action.");
                }
            }
        }

        return sb.ToString();
    }

    public static string ToSmallCaps(this string str,
        InvalidCharAction action = InvalidCharAction.Keep) =>
        throw new NotImplementedException();

    /// <summary>
    /// Construct a new string by repeating a string multiple times.
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

    /// <summary>
    /// Remove whitespace from a string.
    /// </summary>
    /// <see href="https://www.compart.com/en/unicode/category/Zs" />
    /// <param name="str">The string to process.</param>
    /// <returns>The string with whitespace characters removed.</returns>
    public static string StripWhitespace(this string str) =>
        Regex.Replace(str, @"[\s\u00A0\u1680\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008"
            + @"\u2009\u200A\u202F\u205F\u3000]", "");

    /// <summary>
    /// Remove brackets (and whatever's between them) from a string.
    /// </summary>
    /// <param name="str">The string to process.</param>
    /// <param name="round">If round brackets should be removed.</param>
    /// <param name="square">If square brackets should be removed</param>
    /// <param name="curly">If curly brackets should be removed</param>
    /// <param name="angle">If angle brackets should be removed</param>
    /// <returns>The string with brackets removed.</returns>
    public static string StripBrackets(this string str, bool round = true, bool square = true,
        bool curly = true, bool angle = true)
    {
        if (round)
        {
            str = Regex.Replace(str, @"\([^\)]*\)", "");
        }

        if (square)
        {
            str = Regex.Replace(str, @"\[[^\]]*\]", "");
        }

        if (curly)
        {
            str = Regex.Replace(str, @"{[^}]*}", "");
        }

        if (angle)
        {
            str = Regex.Replace(str, @"<[^>]*>", "");
        }

        return str;
    }

    /// <summary>Strip HTML tags from a string.</summary>
    /// <param name="str">The string to process.</param>
    /// <returns>The string with HTML tags removed.</returns>
    public static string StripTags(this string str) =>
        str.StripBrackets(false, false, false, true);

    /// <summary>
    /// Check if a string contains only ASCII characters.
    /// </summary>
    /// <param name="str">The string to check.</param>
    /// <returns></returns>
    public static bool IsAscii(this string str) =>
        str.All(char.IsAscii);

    /// <summary>
    /// Convert a Unicode string (for example, a page title) to a user-, browser-, and search
    /// engine-friendly URL slug containing only lower-case alphanumeric ASCII characters and
    /// hyphens.
    /// This method does not remove short words, like some algorithms do. I don't perceive this as
    /// necessary - please let me know if you disagree.
    /// </summary>
    /// <param name="str">The string to process.</param>
    /// <returns>The ASCII slug.</returns>
    public static string MakeSlug(this string str)
    {
        // Convert to ASCII.
        string result = str.Transliterate();

        // Remove apostrophes.
        result = result.Replace("'", "");

        // Replace non-alphanumeric characters with hyphens.
        result = Regex.Replace(result, @"[^0-9a-z]+", "-", RegexOptions.IgnoreCase);

        // Trim hyphens from the start and end and lower-case the result.
        return result.Trim('-').ToLower();
    }
}
