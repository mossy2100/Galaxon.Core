using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using Galaxon.Core.Exceptions;
using Galaxon.Core.Numbers;

namespace Galaxon.Core.Strings;

/// <summary>
/// Enables formatting of a number or numeric string as its superscript or subscript form.
/// </summary>
/// <see href="https://en.wikipedia.org/wiki/Unicode_subscripts_and_superscripts" />
/// <see href="https://rupertshepherd.info/resource_pages/superscript-letters-in-unicode" />
/// <see href="https://unicode-table.com/en/" />
public class SuperAndSubscriptFormatter : IFormatProvider, ICustomFormatter
{
    /// <summary>
    /// Superscript versions of characters.
    /// The intention here is to support most characters likely to appear in a superscript.
    /// The most important ones are digits and the hyphen.
    /// </summary>
    public static readonly Dictionary<char, char> SuperscriptChars = new ()
    {
        { '0', '⁰' },
        { '1', '¹' },
        { '2', '²' },
        { '3', '³' },
        { '4', '⁴' },
        { '5', '⁵' },
        { '6', '⁶' },
        { '7', '⁷' },
        { '8', '⁸' },
        { '9', '⁹' },
        { '-', '⁻' },
        { '+', '⁺' },
        { '.', '˙' },
        { ',', '’' },
        { 'e', 'ᵉ' },
        { 'E', 'ᴱ' }
    };

    /// <summary>
    /// Subscript versions of characters.
    /// </summary>
    public static readonly Dictionary<char, char> SubscriptChars = new ()
    {
        { '0', '₀' },
        { '1', '₁' },
        { '2', '₂' },
        { '3', '₃' },
        { '4', '₄' },
        { '5', '₅' },
        { '6', '₆' },
        { '7', '₇' },
        { '8', '₈' },
        { '9', '₈' },
        { '-', '₋' },
    };

    public object? GetFormat(Type? formatType) =>
        formatType == typeof(ICustomFormatter) ? this : null;

    /// <summary>
    /// Format an object using superscripts or subscripts.
    ///
    /// Floating and fixed-point values are formatted using the format code "G". If an upper-case
    /// 'E' would normally appear in the string representation of a value, in subscript this will be
    /// changed to a lower-case 'e' as the subscript upper-case version is unavailable at this time.
    ///
    /// If you want a numeric value formatted differently to the default used in this method ("D" or
    /// "G") , you can chain calls to ToString(), e.g.
    /// <code>
    /// double x = 3.1416;
    /// string formattedD = x.ToString("F2").ToString("sup");
    /// </code>
    /// </summary>
    /// <param name="specifier">
    /// The format specifier, comprising "sup" or "sub", optionally followed by a digit indicating
    /// the action to take on encountering an invalid character.
    ///   0 = Throw an exception.
    ///   1 = Skip it, excluding it from the output (default).
    ///   2 = Keep the original, untransformed character.
    /// These map to values in the TransformInvalidCharAction enum.
    /// Thus, the valid formats are: sup, sup0, sup1, sup2, sub, sub0, sub1, and sub2
    /// </param>
    /// <param name="arg">The number, string, or other object to format.</param>
    /// <param name="provider">Reference to this instance.</param>
    /// <returns>
    /// The number, string, or other object formatted as a superscript or subscript string.
    /// </returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentInvalidException"></exception>
    public string Format(string? specifier, object? arg, IFormatProvider? provider)
    {
        // Check for legitimate callback.
        if (!Equals(provider))
        {
            throw new ArgumentInvalidException(nameof(provider),
                "Should match the instance.");
        }

        // Format null as empty string.
        if (arg is null)
        {
            return "";
        }

        // Check for valid format string.
        if (specifier is null
            || !Regex.IsMatch(specifier, "^su[pb][0-2]?$", RegexOptions.IgnoreCase))
        {
            throw new ArgumentInvalidException(nameof(specifier), "Invalid format specifier. Must "
                + "be \"sup\" for superscript or \"sub\" for subscript, optionally followed by a "
                + "single-digit action code (0-2). See documentation for more details.");
        }

        // Extract the format parameters.
        string format = specifier[..3].ToLower(CultureInfo.InvariantCulture);
        InvalidCharAction action = (specifier.Length == 4)
            ? (InvalidCharAction)(specifier[^1] - '0')
            : InvalidCharAction.Skip;

        // Convert the argument into a string, which can then be transformed into superscript or
        // subscript.
        string? strArg;

        if (arg is string s)
        {
            strArg = s;
        }
        else if (XNumber.IsNumber(arg))
        {
            // Get the number format. Integers are formatted using "D". Fixed and floating point
            // values are formatted as "G" for superscript, and "F0" for subscript.
            string numFormat = XNumber.IsInteger(arg) ? "D" : (format == "sup" ? "G" : "F0");

            // Convert argument to string.
            strArg = arg switch
            {
                sbyte n => n.ToString(numFormat),
                byte n => n.ToString(numFormat),
                short n => n.ToString(numFormat),
                ushort n => n.ToString(numFormat),
                int n => n.ToString(numFormat),
                uint n => n.ToString(numFormat),
                long n => n.ToString(numFormat),
                ulong n => n.ToString(numFormat),
                Int128 n => n.ToString(numFormat),
                UInt128 n => n.ToString(numFormat),
                BigInteger n => n.ToString(numFormat),
                float n => n.ToString(numFormat),
                Half n => n.ToString(numFormat),
                double n => n.ToString(numFormat),
                decimal n => n.ToString(numFormat),
                Complex n => n.ToString(numFormat),
                _ => null,
            };
        }
        else
        {
            strArg = arg.ToString();
        }

        // If ToString() returns a null, return an empty string.
        if (strArg is null)
        {
            return "";
        }

        // Transform the string.
        return format switch
        {
            // Superscript.
            "sup" => strArg.Transform(SuperscriptChars, action),

            // Subscript.
            "sub" => strArg.Transform(SubscriptChars, action),

            _ => throw new ArgumentInvalidException(nameof(specifier),
                "Invalid format.")
        };
    }
}
