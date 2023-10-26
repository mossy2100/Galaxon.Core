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

    #region Check for cast operators

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

    #endregion Check for cast operators

    #region Check for interface implementation

    /// <summary>
    /// Check if a type implements an interface.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="interfaceType">The interface type.</param>
    /// <returns>True if the specified type implements the specified interface.</returns>
    public static bool ImplementsInterface(Type type, Type interfaceType)
    {
        return type.GetInterfaces().Any(i => i == interfaceType);
    }

    /// <summary>
    /// Check if a type implements an interface.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    /// <typeparam name="TInterface">The interface type.</typeparam>
    /// <returns>True if the specified type implements the specified interface.</returns>
    public static bool ImplementsInterface<T, TInterface>()
    {
        return ImplementsInterface(typeof(T), typeof(TInterface));
    }

    /// <summary>
    /// Check if a type implements a generic interface.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="interfaceType">The generic interface type.</param>
    /// <returns>True if the specified type implements the specified interface.</returns>
    public static bool ImplementsGenericInterface(Type type, Type interfaceType)
    {
        return type.GetInterfaces().Any(i => i.IsGenericType
            && i.GetGenericTypeDefinition() == interfaceType);
    }

    /// <summary>
    /// Check if a type implements an generic interface.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    /// <typeparam name="TInterface">The generic interface type.</typeparam>
    /// <returns>True if the specified type implements the specified interface.</returns>
    public static bool ImplementsGenericInterface<T, TInterface>()
    {
        return ImplementsGenericInterface(typeof(T), typeof(TInterface));
    }

    /// <summary>
    /// Check if a type implements a self-referencing generic interface
    /// (e.g. IBinaryInteger{TSelf}).
    /// Only works if the self-referenced type is the first type parameter.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="interfaceType">The self-referencing generic interface.</param>
    /// <returns>True if the specified type implements the specified interface.</returns>
    public static bool ImplementsSelfReferencingGenericInterface(Type type, Type interfaceType)
    {
        return type.GetInterfaces().Any(i => i.IsGenericType
            && i.GetGenericTypeDefinition() == interfaceType
            && i.GenericTypeArguments[0] == type);
    }

    /// <summary>
    /// Check if a type implements an self-referencing generic interface.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    /// <typeparam name="TInterface">The self-referencing generic interface type.</typeparam>
    /// <returns>True if the specified type implements the specified interface.</returns>
    public static bool ImplementsSelfReferencingGenericInterface<T, TInterface>()
    {
        return ImplementsSelfReferencingGenericInterface(typeof(T), typeof(TInterface));
    }

    #endregion Check for interface implementation
}
