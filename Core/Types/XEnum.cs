using System.ComponentModel;

namespace Galaxon.Core.Types;

/// <summary>
/// Extension methods for enum types.
/// </summary>
public static class XEnum
{
    /// <summary>
    /// Get the value of the Description attribute for the enum value, or, if not provided,
    /// the name of the value (same as ToString()).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string GetDescription(this Enum value)
    {
        // Get the value name.
        var name = value.ToString();

        // Get the field info of the value.
        var field = value.GetType().GetField(name);

        // If we couldn't find it, the value is invalid.
        if (field == null)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Invalid enum value.");
        }

        // Get the attributes attached to the value.
        var attributes =
            (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

        // Return the Description if set, otherwise the name.
        return attributes.Length > 0 ? attributes[0].Description : name;
    }

    /// <summary>
    /// Find an enum value given a description.
    /// If no values are found with a matching description, looks for a match on name.
    /// Must match exactly (case-sensitive) the Description attribute attached to the value, or the
    /// value name.
    /// </summary>
    /// <param name="description">The description.</param>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>The value if found.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the type param is not an enum, or the description doesn't match any values.
    /// </exception>
    public static T FindValueByDescription<T>(string description) where T : struct
    {
        // Make sure this is being used with an enum type.
        var enumType = typeof(T);
        if (!enumType.IsEnum)
        {
            throw new ArgumentOutOfRangeException(nameof(T), "The provided type must be an enum type.");
        }

        // Inspect each value in the enum for a matching description.
        foreach (var field in enumType.GetFields())
        {
            // Check the description attribute.
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                if (attribute.Description == description)
                {
                    return (T)field.GetValue(null)!;
                }
            }
        }

        // As a fallback, look for a matching name.
        if (Enum.TryParse(description, out T result))
        {
            return result;
        }

        throw new ArgumentOutOfRangeException(nameof(description),
            $"No value found in enum {enumType.Name} with description or name matching '{description}'.");
    }
}
