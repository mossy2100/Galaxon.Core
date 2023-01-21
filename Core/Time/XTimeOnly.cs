namespace Galaxon.Core.Time;

/// <summary>
/// Extension methods for TimeOnly.
/// </summary>
public static class XTimeOnly
{
    /// <summary>
    /// If the time is between midnight and noon.
    /// </summary>
    public static bool IsAm(this TimeOnly t) =>
        t.Hour < 12;

    /// <summary>
    /// If the time is between noon and midnight.
    /// </summary>
    public static bool IsPm(this TimeOnly t) =>
        t.Hour >= 12;

    /// <summary>
    /// If the time is between midnight and 06:00.
    /// </summary>
    public static bool IsSmallHours(this TimeOnly t) =>
        t.Hour < 6;

    /// <summary>
    /// If the time is between 06:00 and noon.
    /// </summary>
    public static bool IsMorning(this TimeOnly t) =>
        t.Hour is >= 6 and < 12;

    /// <summary>
    /// If the time is between noon and 18:00.
    /// </summary>
    public static bool IsAfternoon(this TimeOnly t) =>
        t.Hour is >= 12 and < 18;

    /// <summary>
    /// If the time is between 18:00 and midnight.
    /// </summary>
    public static bool IsEvening(this TimeOnly t) =>
        t.Hour >= 18;
}
