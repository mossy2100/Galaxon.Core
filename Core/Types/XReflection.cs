using System.Reflection;

namespace Galaxon.Core.Types;

/// <summary>
/// Handy reflection-related methods.
/// </summary>
public static class XReflection
{
    /// <summary>
    /// Get the value of a static property.
    /// </summary>
    /// <param name="propertyName">The name of the property.</param>
    /// <typeparam name="T">The type.</typeparam>
    /// <returns>The value of the static property.</returns>
    /// <exception cref="MissingMemberException">If the static property doesn't exist.</exception>
    public static object? GetStaticPropertyValue<T>(string propertyName)
    {
        var t = typeof(T);
        var propInfo = t.GetProperty(propertyName);
        if (propInfo != null)
        {
            return propInfo.GetValue(null);
        }

        throw new MissingMemberException($"Static property '{propertyName}' not found in type '{t.Name}'");
    }

    /// <summary>
    /// See if a cast operator exists from one type to another.
    /// </summary>
    /// <param name="sourceType">The source type.</param>
    /// <param name="targetType">The target type.</param>
    /// <returns>If a cast operator exists.</returns>
    public static bool CanCast(Type sourceType, Type targetType)
    {
        // Search for explicit and implicit cast operators
        return targetType.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Any(m => (m.Name == "op_Implicit" || m.Name == "op_Explicit")
                && m.ReturnType == targetType
                && m.GetParameters().Length == 1
                && m.GetParameters()[0].ParameterType == sourceType);
    }

    /// <summary>
    /// See if a cast operator exists from one type to another.
    /// </summary>
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <typeparam name="TTarget">The target type.</typeparam>
    /// <returns>If a cast operator exists.</returns>
    public static bool CanCast<TSource, TTarget>()
    {
        return CanCast(typeof(TSource), typeof(TTarget));
    }
}
