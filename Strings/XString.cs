using System.Text;
using AstroMultimedia.Core.Exceptions;

namespace AstroMultimedia.Core.Strings;

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
    /// Transform the characters in a string into another character by using a character map.
    /// </summary>
    /// <param name="str">The original string.</param>
    /// <param name="charMap">The character map.</param>
    /// <param name="invalidCharAction">
    /// Code that tells the algorithm what to do if a character is encountered that is not in the
    /// character map.
    ///   E = Throw an exception.
    ///   I = Ignore it.
    ///   U = Use the original, untransformed character.
    /// </param>
    /// <returns></returns>
    /// <exception cref="ArgumentInvalidException"></exception>
    public static string Transform(this string str, Dictionary<char, char> charMap,
        char invalidCharAction = 'E')
    {
        StringBuilder sb = new ();

        foreach (char c in str)
        {
            if (!charMap.ContainsKey(c))
            {
                switch (invalidCharAction)
                {
                    case 'E':
                        throw new ArgumentInvalidException(nameof(str),
                            $"Invalid character 'c', not found in character map.");

                    case 'I':
                        continue;

                    case 'U':
                        sb.Append(c);
                        continue;
                }
            }
            sb.Append(charMap[c]);
        }

        return sb.ToString();
    }
}
