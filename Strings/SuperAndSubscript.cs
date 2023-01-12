using System.Numerics;

namespace Galaxon.Core.Strings;

/// <summary>
/// Extension methods that enable formatting of numbers and numeric strings in superscript or
/// subscript form.
/// </summary>
/// <see href="https://en.wikipedia.org/wiki/Unicode_subscripts_and_superscripts" />
/// <see href="https://rupertshepherd.info/resource_pages/superscript-letters-in-unicode" />
/// <see href="https://unicode-table.com/en/" />
public static class SuperAndSubscript
{
    /// <summary>
    /// Map from normal characters to their superscript versions.
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
    /// Map from normal characters to their subscript versions.
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

    /// <summary>
    /// Convert a string to superscript characters.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="action">The action to take if an unsupported character is encountered.</param>
    /// <returns>The string of superscript characters.</returns>
    public static string ToSuperscript(this string str,
        InvalidCharAction action = InvalidCharAction.Skip) =>
        str.Transform(SuperscriptChars, action);

    /// <summary>
    /// Convert a string to subscript characters.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="action">The action to take if an unsupported character is encountered.</param>
    /// <returns>The string of subscript characters.</returns>
    public static string ToSubscript(this string str,
        InvalidCharAction action = InvalidCharAction.Skip) =>
        str.Transform(SubscriptChars, action);

    /// <summary>
    /// Convert a number to a superscript string.
    /// Supports both integer and floating point types.
    /// </summary>
    /// <param name="n">A number.</param>
    /// <typeparam name="T">A number type.</typeparam>
    /// <returns>The string of superscript characters.</returns>
    public static string? ToSuperscript<T>(this T n) where T : INumberBase<T> =>
        n.ToString()?.ToSuperscript();

    /// <summary>
    /// Convert a number to a superscript string.
    /// Only supports integer types.
    /// </summary>
    /// <param name="n">An integer.</param>
    /// <typeparam name="T">An integer type.</typeparam>
    /// <returns>The string of subscript characters.</returns>
    public static string? ToSubscript<T>(this T n) where T : IBinaryInteger<T> =>
        n.ToString()?.ToSubscript();
}
