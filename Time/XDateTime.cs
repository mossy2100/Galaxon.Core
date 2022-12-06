namespace AstroMultimedia.Core.Time;

/// <summary>
/// Extension methods for the DateTime class.
/// </summary>
public static class XDateTime
{
    #region Constants

    /// <summary>
    /// The number of days from the start of the Julian period to the start of the epoch used by
    /// .NET (0001-01-01 00:00:00 UTC).
    /// <see cref="GetTotalDays" />
    /// </summary>
    public const double JULIAN_PERIOD_OFFSET = 1721425.5;

    #endregion Constants

    #region Extract date and time parts

    /// <summary>
    /// Get the date part of a DateTime as a DateOnly object.
    /// An alternative to the Date property, which returns a DateTime.
    /// </summary>
    /// <param name="dt">The DateTime.</param>
    /// <returns>The date part of the DateTime.</returns>
    public static DateOnly GetDateOnly(this DateTime dt) => DateOnly.FromDateTime(dt);

    /// <summary>
    /// Get the time part of a DateTime as a TimeOnly object.
    /// An alternative to the TimeOfDay property, which returns a TimeSpan.
    /// </summary>
    /// <param name="dt">The DateTime.</param>
    /// <returns>The time part of the DateTime.</returns>
    public static TimeOnly GetTimeOnly(this DateTime dt) => TimeOnly.FromTimeSpan(dt.TimeOfDay);

    #endregion Extract date and time parts

    #region Methods for getting the instant as a count of time units

    // The naming convention for these methods is intended to match the TimeSpan
    // properties (e.g. TotalDays, TotalHours, TotalSeconds, etc.)

    /// <summary>
    /// Get the total number of nanoseconds from the start of the epoch to the datetime.
    /// </summary>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>The number of nanoseconds since the epoch start.</returns>
    public static double GetTotalNanoseconds(this DateTime dt) =>
        dt.Ticks / Time.TICKS_PER_NANOSECOND;

    /// <summary>
    /// Get the total number of microseconds from the start of the epoch to the datetime.
    /// </summary>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>The number of microseconds since the epoch start.</returns>
    public static double GetTotalMicroseconds(this DateTime dt) =>
        (double)dt.Ticks / Time.TICKS_PER_MICROSECOND;

    /// <summary>
    /// Get the total number of milliseconds from the start of the epoch to the datetime.
    /// </summary>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>The number of milliseconds since the epoch start.</returns>
    public static double GetTotalMilliseconds(this DateTime dt) =>
        (double)dt.Ticks / Time.TICKS_PER_MILLISECOND;

    /// <summary>
    /// Get the total number of seconds from the start of the epoch to the datetime.
    /// </summary>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>The number of seconds since the epoch start.</returns>
    public static double GetTotalSeconds(this DateTime dt) =>
        (double)dt.Ticks / Time.TICKS_PER_SECOND;

    /// <summary>
    /// Get the total number of minutes from the start of the epoch to the datetime.
    /// </summary>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>The number of minutes since the epoch start.</returns>
    public static double GetTotalMinutes(this DateTime dt) =>
        (double)dt.Ticks / Time.TICKS_PER_MINUTE;

    /// <summary>
    /// Get the total number of hours from the start of the epoch to the datetime.
    /// </summary>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>The number of hours since the epoch start.</returns>
    public static double GetTotalHours(this DateTime dt) => (double)dt.Ticks / Time.TICKS_PER_HOUR;

    /// <summary>
    /// Get the total number of days from the start of the epoch to the datetime.
    /// </summary>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>The number of days since the epoch start.</returns>
    public static double GetTotalDays(this DateTime dt) => (double)dt.Ticks / Time.TICKS_PER_DAY;

    #endregion Methods for getting the instant as a count of time units

    #region Create new object from time unit count

    /// <summary>
    /// Create a new DateTime given the number of seconds since the start of the epoch.
    /// </summary>
    /// <param name="seconds">The number of seconds.</param>
    /// <returns>A new DateTime object.</returns>
    public static DateTime FromTotalSeconds(double seconds)
    {
        long ticks = (long)Math.Round(seconds * Time.TICKS_PER_SECOND);
        return new DateTime(ticks, DateTimeKind.Utc);
    }

    /// <summary>
    /// Create a new DateTime given the number of days since the start of the epoch.
    /// </summary>
    /// <param name="days">
    /// The day count. May include a fractional part indicating the time of day.
    /// </param>
    /// <returns>A new DateTime object.</returns>
    public static DateTime FromTotalDays(double days)
    {
        long ticks = (long)Math.Round(days * Time.TICKS_PER_DAY);
        return new DateTime(ticks, DateTimeKind.Utc);
    }

    #endregion Create new object from time unit count

    #region Conversion to/from Julian Day

    /// <summary>
    /// Express the DateTime as a Julian Day.
    /// The time of day information in the DateTime will be expressed as the fractional part of
    /// the return value. Note, however, a Julian Day begins at 12:00 noon.
    /// </summary>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>The Julian Day value</returns>
    public static double ToJulianDay(this DateTime dt) => JULIAN_PERIOD_OFFSET + GetTotalDays(dt);

    /// <summary>
    /// Convert a Julian Day value to a DateTime object.
    /// </summary>
    /// <param name="jd">
    /// The Julian Day value. May include a fractional part indicating the time of day.
    /// </param>
    /// <returns>A new DateTime object.</returns>
    public static DateTime FromJulianDay(double jd) => FromTotalDays(jd - JULIAN_PERIOD_OFFSET);

    #endregion Conversion to/from Julian Day
}
