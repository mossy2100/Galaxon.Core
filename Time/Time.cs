namespace AstroMultimedia.Core.Time;

public static class Time
{
    #region Conversion Methods

    /// <summary>
    /// Convert a  time value from one unit to another.
    /// </summary>
    /// <param name="amount">The amount.</param>
    /// <param name="fromUnit">The amount argument units.</param>
    /// <param name="toUnit">The result units.</param>
    /// <returns>The new TimeSpan object.</returns>
    public static double Convert(double amount, ETimeUnit fromUnit,
        ETimeUnit toUnit = ETimeUnit.Ticks)
    {
        double ticks = fromUnit switch
        {
            ETimeUnit.Nanoseconds => amount * TICKS_PER_NANOSECOND,
            ETimeUnit.Ticks => amount,
            ETimeUnit.Microseconds => amount * TICKS_PER_MICROSECOND,
            ETimeUnit.Milliseconds => amount * TICKS_PER_MILLISECOND,
            ETimeUnit.Seconds => amount * TICKS_PER_SECOND,
            ETimeUnit.Minutes => amount * TICKS_PER_MINUTE,
            ETimeUnit.Hours => amount * TICKS_PER_HOUR,
            ETimeUnit.Days => amount * TICKS_PER_DAY,
            ETimeUnit.Weeks => amount * TICKS_PER_WEEK,
            ETimeUnit.Months => amount * TICKS_PER_MONTH,
            ETimeUnit.Years => amount * TICKS_PER_YEAR,
            ETimeUnit.Decades => amount * TICKS_PER_DECADE,
            ETimeUnit.Centuries => amount * TICKS_PER_CENTURY,
            ETimeUnit.Millennia => amount * TICKS_PER_MILLENNIUM,
            _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), "Invalid time unit.")
        };

        return toUnit switch
        {
            ETimeUnit.Nanoseconds => ticks / TICKS_PER_NANOSECOND,
            ETimeUnit.Ticks => ticks,
            ETimeUnit.Microseconds => ticks / TICKS_PER_MICROSECOND,
            ETimeUnit.Milliseconds => ticks / TICKS_PER_MILLISECOND,
            ETimeUnit.Seconds => ticks / TICKS_PER_SECOND,
            ETimeUnit.Minutes => ticks / TICKS_PER_MINUTE,
            ETimeUnit.Hours => ticks / TICKS_PER_HOUR,
            ETimeUnit.Days => ticks / TICKS_PER_DAY,
            ETimeUnit.Weeks => ticks / TICKS_PER_WEEK,
            ETimeUnit.Months => ticks / TICKS_PER_MONTH,
            ETimeUnit.Years => ticks / TICKS_PER_YEAR,
            ETimeUnit.Decades => ticks / TICKS_PER_DECADE,
            ETimeUnit.Centuries => ticks / TICKS_PER_CENTURY,
            ETimeUnit.Millennia => ticks / TICKS_PER_MILLENNIUM,
            _ => throw new ArgumentOutOfRangeException(nameof(toUnit), "Invalid time unit.")
        };
    }

    #endregion Conversion Methods

    #region Constants

    /**
     * NOTE:
     * The word Month, Year, Decade, Century, or Millennium in a constant name
     * refers to the average length of that time unit in the Gregorian Calendar.
     */

    #region Basic multipliers

    // From these we can determine all others.
    public const double NANOSECONDS_PER_TICK = 100;
    public const double SECONDS_PER_MINUTE = 60;
    public const double MINUTES_PER_HOUR = 60;
    public const double HOURS_PER_DAY = 24;
    public const double DAYS_PER_WEEK = 7;
    // Here we're referring to days per Gregorian calendar year.
    public const double DAYS_PER_YEAR = 365.2425;
    // Here we're referring to months per Gregorian calendar year.
    public const double MONTHS_PER_YEAR = 12;
    public const double YEARS_PER_DECADE = 10;
    public const double YEARS_PER_CENTURY = 100;
    public const double YEARS_PER_MILLENNIUM = 1000;

    #endregion Basic multipliers

    #region Ticks per unit of time

    public const double TICKS_PER_NANOSECOND = 1 / NANOSECONDS_PER_TICK;
    public const double TICKS_PER_MICROSECOND = TICKS_PER_NANOSECOND * 1000;
    public const double TICKS_PER_MILLISECOND = TICKS_PER_NANOSECOND * 1e6;
    public const double TICKS_PER_SECOND = TICKS_PER_NANOSECOND * 1e9;
    public const double TICKS_PER_MINUTE = TICKS_PER_SECOND * SECONDS_PER_MINUTE;
    public const double TICKS_PER_HOUR = TICKS_PER_SECOND * SECONDS_PER_HOUR;
    public const double TICKS_PER_DAY = TICKS_PER_SECOND * SECONDS_PER_DAY;
    public const double TICKS_PER_WEEK = TICKS_PER_DAY * DAYS_PER_WEEK;
    public const double TICKS_PER_MONTH = TICKS_PER_DAY * DAYS_PER_MONTH;
    public const double TICKS_PER_YEAR = TICKS_PER_DAY * DAYS_PER_YEAR;
    public const double TICKS_PER_DECADE = TICKS_PER_YEAR * YEARS_PER_DECADE;
    public const double TICKS_PER_CENTURY = TICKS_PER_YEAR * YEARS_PER_CENTURY;
    public const double TICKS_PER_MILLENNIUM = TICKS_PER_YEAR * YEARS_PER_MILLENNIUM;

    #endregion Seconds per unit of time

    #region Seconds per unit of time

    public const double SECONDS_PER_NANOSECOND = 1e-9;
    public const double SECONDS_PER_TICK = 1 / TICKS_PER_SECOND;
    public const double SECONDS_PER_MICROSECOND = 1e-6;
    public const double SECONDS_PER_MILLISECOND = 1e-3;
    public const double SECONDS_PER_HOUR = SECONDS_PER_MINUTE * MINUTES_PER_HOUR;
    public const double SECONDS_PER_DAY = SECONDS_PER_HOUR * HOURS_PER_DAY;
    public const double SECONDS_PER_WEEK = SECONDS_PER_DAY * DAYS_PER_WEEK;
    public const double SECONDS_PER_MONTH = SECONDS_PER_DAY * DAYS_PER_MONTH;
    public const double SECONDS_PER_YEAR = SECONDS_PER_DAY * DAYS_PER_YEAR;
    public const double SECONDS_PER_DECADE = SECONDS_PER_YEAR * YEARS_PER_DECADE;
    public const double SECONDS_PER_CENTURY = SECONDS_PER_YEAR * YEARS_PER_CENTURY;
    public const double SECONDS_PER_MILLENNIUM = SECONDS_PER_YEAR * YEARS_PER_MILLENNIUM;

    #endregion Seconds per unit of time

    #region Days per unit of time

    public const double DAYS_PER_MONTH = DAYS_PER_YEAR / MONTHS_PER_YEAR;
    public const double DAYS_PER_DECADE = DAYS_PER_YEAR * YEARS_PER_DECADE;
    public const double DAYS_PER_CENTURY = DAYS_PER_YEAR * YEARS_PER_CENTURY;
    public const double DAYS_PER_MILLENNIUM = DAYS_PER_YEAR * YEARS_PER_MILLENNIUM;

    public const double DAYS_PER_JULIAN_YEAR = 365.25;
    public const double DAYS_PER_JULIAN_DECADE = DAYS_PER_JULIAN_YEAR * YEARS_PER_DECADE;

    public const double DAYS_PER_JULIAN_CENTURY =
        DAYS_PER_JULIAN_YEAR * YEARS_PER_CENTURY;

    public const double DAYS_PER_JULIAN_MILLENNIUM =
        DAYS_PER_JULIAN_YEAR * YEARS_PER_MILLENNIUM;

    /// <summary>
    /// Length of tropical year B1900 (days) (value copied from SOFA).
    /// </summary>
    public const double DAYS_PER_TROPICAL_YEAR = 365.242198781;

    // Possible leap year fractions:
    // 97 / 400 = 0.2425 (Gregorian)
    // 969 / 4000 = 0.24225 (modified Gregorian - if a year is divisible by 4000 then it's not a leap year)
    // 8 / 33 = .242424... (Iranian)
    // 872 / 3600 = .2422222... (sexagenary)
    // 29 / 120 = 0.24166666666667 (sexagenary)
    // 31 / 128 = 0.2421875 (mine)
    // 1211 / 5000 = 0.2422 (millennial)
    // 807 / 3332 = 0.2421968787515 (modified Gregorian - if a year is divisible by 3332 then it's not a leap year)

    public const double DAYS_PER_LUNATION = 29.530588861;

    #endregion Days per unit of time

    #region Gregorian calendar cycles

    /// <summary>
    /// The Gregorian Calendars repeats on a 400-year cycle.
    /// There are 97 leap years in that period, giving an average calendar year length of
    /// 365 + (97/400) = 365.2425 days/year
    /// There are a whole number of months, weeks, and days in that period.
    /// </summary>
    public const int YEARS_PER_CYCLE = 400;
    public const int LEAP_YEARS_PER_CYCLE = 97;
    public const int MONTHS_PER_CYCLE = 4800;
    public const int DAYS_PER_CYCLE = 146_097; // 3 * 3 * 3 * 7 * 773
    public const int WEEKS_PER_CYCLE = 20_871; // 3 * 3 * 3 * 773

    #endregion Gregorian calendar cycles

    #endregion Constants
}
