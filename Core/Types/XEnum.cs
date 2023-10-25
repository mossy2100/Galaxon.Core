using System.ComponentModel;

namespace Galaxon.Core.Types;

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
        var fieldInfo = value.GetType().GetField(value.ToString());
        if (fieldInfo == null)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Invalid units value.");
        }
        var attributes =
            (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute),
                false);
        return attributes.Length > 0 ? attributes[0].Description : value.ToString();
    }
}
