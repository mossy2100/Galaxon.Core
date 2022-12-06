using System.Text;

namespace AstroMultimedia.Core.Strings;

/// <summary>
/// Extension methods for StringBuilder.
/// </summary>
public static class XStringBuilder
{
    /// <summary>
    /// Prepends a StringBuilder with a string.
    /// Works same as Append().
    /// </summary>
    public static StringBuilder Prepend(this StringBuilder sb, object? value) =>
        (value == null) ? sb : sb.Insert(0, value.ToString());
}
