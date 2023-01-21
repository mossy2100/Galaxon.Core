using System.Globalization;

namespace Galaxon.Core.Time;

/// <summary>
/// Extension methods for the DateOnly class.
/// </summary>
public static class XDateOnly
{
    #region Formatting

    /// <summary>
    /// Format the date using ISO 8601 format YYYY-MM-DD.
    /// <see href="https://en.wikipedia.org/wiki/ISO_8601#Calendar_dates" />
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>A string representing the date in ISO format.</returns>
    public static string ToIsoString(this DateOnly date) =>
        date.ToString("yyyy-MM-dd");

    #endregion Formatting

    #region Additional methods for converting to a DateTime

    /// <summary>
    /// Convert a DateOnly to a DateTime, with default time 00:00:00.
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>The new DateTime object</returns>
    public static DateTime ToDateTime(this DateOnly date) =>
        date.ToDateTime(new TimeOnly(0));

    /// <summary>
    /// Convert a DateOnly to a DateTime, with default time 00:00:00 and specified DateTimeKind.
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <param name="kind">The DateTimeKind.</param>
    /// <returns>The new DateTime object</returns>
    public static DateTime ToDateTime(this DateOnly date, DateTimeKind kind) =>
        date.ToDateTime(new TimeOnly(0), kind);

    #endregion Additional methods for converting to a DateTime

    #region Methods for getting the instant as a count of time units

    // These methods treat the DateOnly as an instant (the start of the given date in UTC), which of
    // course it isn't. But they are still useful.
    // Note: these return longs, unlike the methods in DateTime, because they will always be
    // integers.

    /// <summary>
    /// Get the number of ticks between the start of the epoch (0001-01-01 00:00:00) and the start
    /// of the date.
    /// If extension properties are added to the language I may change this to a property "Ticks"
    /// later, for consistency with DateTime.
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.datetime.ticks?view=net-7.0" />
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>The number of ticks.</returns>
    public static long GetTicks(this DateOnly date) =>
        date.ToDateTime().Ticks;

    /// <summary>
    /// Get the number of seconds between the start of the epoch and the start of the date.
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>The number of seconds since the epoch start.</returns>
    public static long GetTotalSeconds(this DateOnly date) =>
        date.ToDateTime().Ticks / TimeSpan.TicksPerSecond;

    /// <summary>
    /// Get the number of days between the start of the epoch and the start of the date.
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>The number of days since the epoch start.</returns>
    public static long GetTotalDays(this DateOnly date) =>
        date.ToDateTime().Ticks / TimeSpan.TicksPerDay;

    #endregion Methods for getting the instant as a count of time units

    #region Subtraction methods

    // These reflect the DateTime.Subtract() methods.

    /// <summary>
    /// Returns the difference between two dates (as DateOnly objects) as a TimeSpan.
    /// Emulates the DateTime.Subtract(DateTime) method.
    /// Because we're subtracting dates without time of day information, the result will contain
    /// only a whole number of days; the hours, minutes, and seconds parts of the TimeSpan will
    /// be 0.
    /// If the DateOnly instance date is later than the parameter date, the result will be
    /// positive.
    /// If they are the same dates, the result will be zero.
    /// Otherwise, the result will be negative.
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.datetime.subtract?view=net-7.0#system-datetime-subtract(system-datetime)" />
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <param name="date2">The DateOnly to be subtracted.</param>
    /// <returns>The difference between the two dates as a TimeSpan.</returns>
    public static TimeSpan Subtract(this DateOnly date, DateOnly date2) =>
        date.ToDateTime().Subtract(date2.ToDateTime());

    /// <summary>
    /// Returns a DateTime with a TimeSpan subtracted.
    /// Emulates the DateTime.Subtract(TimeSpan) method.
    /// Because time of day information is discarded, the TimeSpan parameter is effectively
    /// rounded up to the nearest whole day.
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.datetime.subtract?view=net-7.0#system-datetime-subtract(system-timespan)" />
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <param name="span">The TimeSpan to be subtracted.</param>
    /// <returns>The resulting date.</returns>
    public static DateOnly Subtract(this DateOnly date, TimeSpan span) =>
        DateOnly.FromDateTime(date.ToDateTime().Subtract(span));

    #endregion Subtraction methods

    #region Create new object from day count

    /// <summary>
    /// Determine the date given the number of days from the start of the epoch (0001-01-01).
    ///
    /// See <see cref="XDateTime.FromTotalDays" />
    /// </summary>
    /// <param name="days">The number of days.</param>
    /// <returns>A new DateOnly object.</returns>
    public static DateOnly FromTotalDays(long days) =>
        DateOnly.FromDateTime(XDateTime.FromTotalDays(days));

    /// <summary>
    /// Construct a new DateOnly instance given a year and the day of the year.
    /// Formula from Meeus (Astronomical Algorithms 2 ed., p66).
    /// </summary>
    /// <param name="year">The year (1..9999).</param>
    /// <param name="dayOfYear">The day of the year (1..366).</param>
    /// <returns></returns>
    public static DateOnly FromDayOfYear(int year, int dayOfYear)
    {
        // Check year is in the valid range.
        if (year is < 1 or > 9999)
        {
            throw new ArgumentOutOfRangeException(nameof(year), "Must be in the range 1..9999");
        }

        // Check day of year is in the valid range.
        GregorianCalendar gc = new ();
        int daysInYear = gc.GetDaysInYear(year);
        if (dayOfYear < 1 || dayOfYear > daysInYear)
        {
            throw new ArgumentOutOfRangeException(nameof(dayOfYear), $"Must be in the range 1..{daysInYear}");
        }

        // Calculate.
        int k = daysInYear - 364;
        int month = (dayOfYear < 32) ? 1 : (int)(9 * (k + dayOfYear) / 275.0 + 0.98);
        int day = dayOfYear - (int)(275 * month / 9.0) + k * (int)((month + 9) / 12.0) + 30;

        return new DateOnly(year, month, day);
    }

    #endregion Create new object from day count

    #region Conversion to/from Julian Day

    /// <summary>
    /// Convert a  DateOnly object to a Julian Day value.
    /// The result gives the Julian Day at the start of the given date
    /// (00:00:00, i.e. midnight), which will always have a fraction of 0.5, since a Julian Day
    /// starts at 12:00:00 (noon).
    /// <see cref="XDateTime.ToJulianDay" />
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>The Julian Day value.</returns>
    public static double ToJulianDay(this DateOnly date) =>
        date.ToDateTime().ToJulianDay();

    /// <summary>
    /// Convert a Julian Day value to a DateOnly object.
    /// <see cref="XDateTime.FromJulianDay" />
    /// </summary>
    /// <param name="jd">
    /// The Julian Day value. If a fractional part indicating the time of day is included, this
    /// information will be discarded.
    /// </param>
    /// <returns>A new DateOnly object.</returns>
    public static DateOnly FromJulianDay(double jd) =>
        DateOnly.FromDateTime(XDateTime.FromJulianDay(jd));

    #endregion Conversion to/from Julian Day

    #region Find special dates

    /// <summary>
    /// Get the date of Easter Sunday in the given year, in the Gregorian Calendar.
    /// Formula is from Wikipedia.
    /// This method uses the "Meeus/Jones/Butcher" algorithm from 1876, with the New Scientist
    /// modifications from 1961.
    /// Tested for years 1600..2299.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Date_of_Easter#Anonymous_Gregorian_algorithm" />
    /// <see href="https://www.census.gov/data/software/x13as/genhol/easter-dates.html" />
    /// <see href="https://www.assa.org.au/edm" />
    /// <param name="year">The Gregorian year number.</param>
    /// <returns>The date of Easter Sunday for the given year.</returns>
    public static DateOnly GetEaster(int year)
    {
        int a = year % 19;
        int b = year / 100;
        int c = year % 100;
        int d = b / 4;
        int e = b % 4;
        int g = (8 * b + 13) / 25;
        int h = (19 * a + b - d - g + 15) % 30;
        int i = c / 4;
        int k = c % 4;
        int l = (32 + 2 * e + 2 * i - h - k) % 7;
        int m = (a + 11 * h + 19 * l) / 433;
        int q = h + l - 7 * m;
        int month = (q + 90) / 25;
        int day = (q + 33 * month + 19) % 32;
        return new DateOnly(year, month, day);
    }

    /// <summary>
    /// Get the date of Christmas Day in the given year, in the Gregorian Calendar.
    /// </summary>
    public static DateOnly GetChristmas(int y) =>
        new (y, 12, 31);

    #endregion Find special dates
}
