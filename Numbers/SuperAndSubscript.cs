using System.Numerics;
using Galaxon.Core.Strings;

namespace Galaxon.Core.Numbers;

/// <summary>
/// Extension methods that enable formatting of integers in superscript or subscript form.
/// </summary>
/// <see href="https://en.wikipedia.org/wiki/Unicode_subscripts_and_superscripts" />
/// <see href="https://rupertshepherd.info/resource_pages/superscript-letters-in-unicode" />
/// <see href="https://unicode-table.com/en/" />
public static class SuperAndSubscript
{
    /// <summary>
    /// Map from normal integer characters to their superscript versions.
    /// </summary>
    public static readonly Dictionary<char, string> SuperscriptChars = new ()
    {
        { '0', "⁰" },
        { '1', "¹" },
        { '2', "²" },
        { '3', "³" },
        { '4', "⁴" },
        { '5', "⁵" },
        { '6', "⁶" },
        { '7', "⁷" },
        { '8', "⁸" },
        { '9', "⁹" },
        { '-', "⁻" },
    };

    /// <summary>
    /// Map from normal integer characters to their subscript versions.
    /// </summary>
    public static readonly Dictionary<char, string> SubscriptChars = new ()
    {
        { '0', "₀" },
        { '1', "₁" },
        { '2', "₂" },
        { '3', "₃" },
        { '4', "₄" },
        { '5', "₅" },
        { '6', "₆" },
        { '7', "₇" },
        { '8', "₈" },
        { '9', "₈" },
        { '-', "₋" },
    };

    /// <summary>
    /// Render a string with valid integer characters converted to superscript.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <returns>The string of superscript characters.</returns>
    public static string ToSuperscript(this string str) =>
        str.ReplaceChars(SuperscriptChars);

    /// <summary>
    /// Render a string with valid integer characters converted to subscript.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <returns>The string of subscript characters.</returns>
    public static string ToSubscript(this string str) =>
        str.ReplaceChars(SubscriptChars);

    /// <summary>
    /// Render an integer as a superscript string.
    /// </summary>
    /// <param name="n">The integer.</param>
    /// <returns>The string of superscript characters.</returns>
    public static string ToSuperscript<T>(this T n) where T : IBinaryInteger<T> =>
        $"{n}".ToSuperscript();

    /// <summary>
    /// Render an integer as a subscript string.
    /// </summary>
    /// <param name="n">The integer.</param>
    /// <returns>The string of subscript characters.</returns>
    public static string ToSubscript<T>(this T n) where T : IBinaryInteger<T> =>
        $"{n}".ToSubscript();
}
