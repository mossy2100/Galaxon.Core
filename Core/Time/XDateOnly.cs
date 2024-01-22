using System.Globalization;
using Galaxon.Core.Exceptions;

namespace Galaxon.Core.Time;

/// <summary>
/// Extension methods for the DateOnly class.
/// </summary>
public static class XDateOnly
{
    #region Formatting

    /// <summary>
    /// Same as DateTimeFormatInfo.SortableDateTimePattern, but without the time.
    /// </summary>
    public const string SORTABLE_DATE_PATTERN = "yyyy-MM-dd";

    /// <summary>
    /// Format the date using ISO 8601 format YYYY-MM-DD.
    /// <see href="https://en.wikipedia.org/wiki/ISO_8601#Calendar_dates"/>
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>A string representing the date in ISO format.</returns>
    public static string ToIsoString(this DateOnly date)
    {
        return date.ToString(SORTABLE_DATE_PATTERN);
    }

    #endregion Formatting

    #region Methods for converting to a DateTime

    /// <summary>
    /// Convert a DateOnly to a DateTime, with default time 00:00:00.
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>The new DateTime object</returns>
    public static DateTime ToDateTime(this DateOnly date)
    {
        return date.ToDateTime(new TimeOnly(0));
    }

    /// <summary>
    /// Convert a DateOnly to a DateTime, with default time 00:00:00 and specified DateTimeKind.
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <param name="kind">The DateTimeKind.</param>
    /// <returns>The new DateTime object</returns>
    public static DateTime ToDateTime(this DateOnly date, DateTimeKind kind)
    {
        return date.ToDateTime(new TimeOnly(0), kind);
    }

    #endregion Methods for converting to a DateTime

    #region Methods for getting the instant as a count of time units

    // These methods treat the DateOnly as an instant (the start of the given date in UTC), which of
    // course it isn't. But they are still useful.

    /// <summary>
    /// Get the number of ticks between the start of the epoch (0001-01-01 00:00:00) and the start
    /// of the date.
    /// If extension properties are added to the language I may change this to a property "Ticks"
    /// later, for consistency with DateTime.
    /// </summary>
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.datetime.ticks?view=net-7.0"/>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>The number of ticks.</returns>
    public static long GetTicks(this DateOnly date)
    {
        return date.ToDateTime().Ticks;
    }

    /// <summary>
    /// Get the number of seconds between the start of the epoch and the start of the date.
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>The number of seconds since the epoch start.</returns>
    public static long GetTotalSeconds(this DateOnly date)
    {
        return date.GetTicks() / TimeSpan.TicksPerSecond;
    }

    /// <summary>
    /// Get the number of days between the start of the epoch and the given date.
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>The number of days since the epoch start.</returns>
    public static long GetTotalDays(this DateOnly date)
    {
        return date.GetTicks() / TimeSpan.TicksPerDay;
    }

    /// <summary>
    /// Get the number of years between the start of the epoch and the start of the date.
    /// The result will be greater than or equal to `date.Year - 1` and less than `date.Year`.
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>The number of years since the epoch start.</returns>
    public static double GetTotalYears(this DateOnly date)
    {
        return (double)date.GetTicks() / XTimeSpan.TICKS_PER_YEAR;
    }

    #endregion Methods for getting the instant as a count of time units

    #region Methods for addition and subtraction

    /// <summary>
    /// Add a period of time to a date to find a new DateTime.
    /// </summary>
    /// <see cref="DateTime.Add(TimeSpan)"/>
    /// <param name="date">The date.</param>
    /// <param name="period">The time period to add.</param>
    /// <returns>The resulting DateTime.</returns>
    public static DateTime Add(this DateOnly date, TimeSpan period)
    {
        return date.ToDateTime().Add(period);
    }

    /// <summary>
    /// Add a time of day to a date to find a new DateTime.
    /// </summary>
    /// <param name="date">The date.</param>
    /// <param name="time">The time of day to add.</param>
    /// <returns>The resulting DateTime.</returns>
    public static DateTime Add(this DateOnly date, TimeOnly time)
    {
        return date.ToDateTime(time);
    }

    /// <summary>
    /// Add a number of weeks to a date.
    /// </summary>
    /// <param name="date">The date.</param>
    /// <param name="weeks">The number of weeks to add.</param>
    /// <returns>The resulting date.</returns>
    public static DateOnly AddWeeks(this DateOnly date, int weeks)
    {
        return date.AddDays(weeks * (int)XTimeSpan.DAYS_PER_WEEK);
    }

    /// <summary>
    /// Returns the difference between two dates as a number of days.
    /// Emulates the <see cref="DateTime.Subtract(DateTime)"/> method.
    /// If the end date is later than the start date, the result will be positive.
    /// If they are equal, the result will be zero. Otherwise, the result will be negative.
    /// </summary>
    /// <param name="end">The end date.</param>
    /// <param name="start">The start date.</param>
    /// <returns>The number of days difference between the two dates.</returns>
    public static long Subtract(this DateOnly end, DateOnly start)
    {
        return end.GetTotalDays() - start.GetTotalDays();
    }

    #endregion Methods for addition and subtraction

    #region Create new object

    /// <summary>
    /// Find a date given the number of days from the start of the epoch.
    /// </summary>
    /// <see cref="XDateTime.FromTotalDays(double)"/>
    /// <param name="days">The number of days.</param>
    /// <returns>The resulting date.</returns>
    public static DateOnly FromTotalDays(long days)
    {
        return DateOnly.FromDateTime(XDateTime.FromTotalDays(days));
    }

    /// <summary>
    /// Find the date given the number of years since the start of the epoch.
    /// </summary>
    /// <param name="years">The number of years. May include a fractional part.</param>
    /// <returns>The resulting date.</returns>
    public static DateOnly FromTotalYears(double years)
    {
        return DateOnly.FromDateTime(XDateTime.FromTotalYears(years));
    }

    /// <summary>
    /// Find a date given a year and the day of the year.
    /// Formula from Meeus (Astronomical Algorithms 2 ed. p66).
    /// </summary>
    /// <param name="year">The year (1..9999).</param>
    /// <param name="dayOfYear">The day of the year (1..366).</param>
    /// <returns>The resulting date.</returns>
    public static DateOnly FromDayOfYear(int year, int dayOfYear)
    {
        // Check year is in the valid range.
        if (year is < 1 or > 9999)
        {
            throw new ArgumentOutOfRangeException(nameof(year), "Must be in the range 1..9999");
        }

        // Check day of year is in the valid range.
        GregorianCalendar gc = new ();
        var daysInYear = gc.GetDaysInYear(year);
        if (dayOfYear < 1 || dayOfYear > daysInYear)
        {
            throw new ArgumentOutOfRangeException(nameof(dayOfYear),
                $"Must be in the range 1..{daysInYear}");
        }

        // Calculate.
        var k = gc.IsLeapYear(year) ? 1 : 2;
        var month = dayOfYear < 32 ? 1 : (int)(9 * (k + dayOfYear) / 275.0 + 0.98);
        var day = dayOfYear - (int)(275 * month / 9.0) + k * (int)((month + 9) / 12.0) + 30;

        return new DateOnly(year, month, day);
    }

    #endregion Create new object

    #region Conversion to/from Julian Day

    /// <summary>
    /// Convert a DateOnly object to a Julian Day Number.
    /// The result gives the Julian Day Number of a given date.
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>The Julian Day Number.</returns>
    public static int ToJulianDay(this DateOnly date)
    {
        return (int)date.ToDateTime().ToJulianDate();
    }

    /// <summary>
    /// Convert a Julian Day Number to a Gregorian Date.
    /// </summary>
    /// <param name="jd">
    /// The Julian Day Number. If a fractional part indicating the time of day is included, this
    /// information will be discarded.
    /// </param>
    /// <returns>A new DateOnly object.</returns>
    public static DateOnly FromJulianDay(double jd)
    {
        return DateOnly.FromDateTime(XDateTime.FromJulianDate(jd));
    }

    #endregion Conversion to/from Julian Day

    #region Find special dates

    /// <summary>
    /// Get the date of Easter Sunday in the given year.
    /// Formula is from Wikipedia.
    /// This method uses the "Meeus/Jones/Butcher" algorithm from 1876, with the New Scientist
    /// modifications from 1961.
    /// Tested for years 1600..2299.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Date_of_Easter#Anonymous_Gregorian_algorithm"/>
    /// <see href="https://www.census.gov/data/software/x13as/genhol/easter-dates.html"/>
    /// <see href="https://www.assa.org.au/edm"/>
    /// <param name="year">The Gregorian year number.</param>
    /// <returns>The date of Easter Sunday for the given year.</returns>
    public static DateOnly GetEaster(int year)
    {
        var a = year % 19;
        var b = year / 100;
        var c = year % 100;
        var d = b / 4;
        var e = b % 4;
        var g = (8 * b + 13) / 25;
        var h = (19 * a + b - d - g + 15) % 30;
        var i = c / 4;
        var k = c % 4;
        var l = (32 + 2 * e + 2 * i - h - k) % 7;
        var m = (a + 11 * h + 19 * l) / 433;
        var q = h + l - 7 * m;
        var month = (q + 90) / 25;
        var day = (q + 33 * month + 19) % 32;
        return new DateOnly(year, month, day);
    }

    /// <summary>
    /// Get the date of Christmas Day in the given year.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <returns>The date of Christmas in the given year.</returns>
    public static DateOnly GetChristmas(int year)
    {
        return new DateOnly(year, 12, 31);
    }

    /// <summary>
    /// Find the nth weekday in a given month.
    /// Example:
    /// <code>
    /// // Get the 4th Thursday in January, 2023.
    /// DateOnly meetup = XDateOnly.GetNthWeekdayInMonth(2023, 1, 4, DayOfWeek.Thursday);
    /// </code>
    /// A negative value for n means count from the end of the month.
    /// n = -1 means the last one in the month. n = -2 means the second-last, etc.
    /// Example:
    /// <code>
    /// // Get the last Monday in November, 2025.
    /// DateOnly meetup = XDateOnly.GetNthWeekdayInMonth(2025, 11, -1, DayOfWeek.Monday);
    /// </code>
    /// </summary>
    /// <param name="year">The year.</param>
    /// <param name="month">The month.</param>
    /// <param name="n">Which occurence of the day of the week within the month.</param>
    /// <param name="dayOfWeek">The day of the week.</param>
    /// <returns>The requested date.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If Abs(n) not in the range 1..5</exception>
    /// <exception cref="InvalidOperationException">If a valid date could not be found.</exception>
    public static DateOnly GetNthWeekdayInMonth(int year, int month, int n, DayOfWeek dayOfWeek)
    {
        if (Math.Abs(n) is < 1 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(n),
                "The absolute value must be in the range 1..5.");
        }

        // Get the number of days in the month.
        GregorianCalendar gc = new ();
        var daysInMonth = gc.GetDaysInMonth(year, month);
        var daysPerWeek = (int)XTimeSpan.DAYS_PER_WEEK;
        int day;

        if (n > 0)
        {
            // Get the number of days difference between the start of the month and the result.
            DateOnly firstOfMonth = new (year, month, 1);
            var diffDays = dayOfWeek - firstOfMonth.DayOfWeek;
            if (diffDays < 0)
            {
                diffDays += daysPerWeek;
            }
            day = 1 + (n - 1) * daysPerWeek + diffDays;
        }
        else
        {
            // Get the number of days difference between the end of the month and the result.
            DateOnly lastOfMonth = new (year, month, daysInMonth);
            var diffDays = lastOfMonth.DayOfWeek - dayOfWeek;
            if (diffDays < 0)
            {
                diffDays += daysPerWeek;
            }
            day = daysInMonth + (n + 1) * daysPerWeek - diffDays;
        }

        // Check day of month is valid.
        if (day < 0 || day > daysInMonth)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Could not find a valid date.");
        }

        // Return the requested date.
        return new DateOnly(year, month, day);
    }

    /// <summary>
    /// Find the date of Thanksgiving for a specified country in a given year.
    /// Only years with a holiday called "Thanksgiving" are supported. The default is "US".
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Thanksgiving#Observance"/>
    /// <param name="year">The year.</param>
    /// <param name="countryCode">The ISO 2-letter country code.</param>
    /// <returns>The date of Thanksgiving.</returns>
    /// <exception cref="ArgumentInvalidException">
    /// Either this country doesn't celebrate Thanksgiving, or the method doesn't support it.
    /// </exception>
    public static DateOnly GetThanksgiving(int year, string countryCode = "US")
    {
        return countryCode switch
        {
            // Norfolk Island: Last Wednesday in November.
            "NF" => GetNthWeekdayInMonth(year, 11, -1, DayOfWeek.Wednesday),

            // Canada: 2nd Monday in October.
            "CA" => GetNthWeekdayInMonth(year, 10, 2, DayOfWeek.Monday),

            // Grenada: October 25.
            "GD" => new DateOnly(year, 10, 25),

            // Liberia: 1st Thursday in November.
            "LR" => GetNthWeekdayInMonth(year, 11, 1, DayOfWeek.Thursday),

            // Rwanda: 1st Friday in August.
            "RW" => GetNthWeekdayInMonth(year, 8, 1, DayOfWeek.Friday),

            // Saint Lucia: 1st Monday in October.
            "LC" => GetNthWeekdayInMonth(year, 10, 1, DayOfWeek.Monday),

            // US, etc.: 4th Thursday in November.
            "US" or "NL" or "PH" or "BR" => GetNthWeekdayInMonth(year, 11, 4, DayOfWeek.Thursday),

            _ => throw new ArgumentOutOfRangeException(nameof(countryCode),
                "A date for Thanksgiving could not be determined for the specified country.")
        };
    }

    #endregion Find special dates
}
