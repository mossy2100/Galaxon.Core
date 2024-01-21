namespace Galaxon.Core.Time;

/// <summary>
/// Additional members to supplement the TimeSpan class.
/// Mostly constants.
/// For consistency with TimeSpan, I've used `long` as the type for any integer constants.
/// For non-integer constants I've used `double`.
/// The following constants are already provided by the TimeSpan class and not reproduced here:
/// - TicksPerMicrosecond
/// - TicksPerMillisecond
/// - TicksPerSecond
/// - TicksPerMinute
/// - TicksPerHour
/// - TicksPerDay
/// **NOTE: Any constants or methods for converting between time units will become obsolete
/// with the completion of Galaxon.Quantities.**
/// The word Month, Year, Decade, Century, or Millennium in a constant name usually refers to the
/// average length of that time unit in the Gregorian Calendar.
/// </summary>
public static class XTimeSpan
{
    #region Conversion Methods

    /// <summary>
    /// Convert a time value from one unit to another.
    /// TODO Replace with Quantity methods.
    /// </summary>
    /// <param name="amount">The amount.</param>
    /// <param name="fromUnit">The amount argument units.</param>
    /// <param name="toUnit">The result units.</param>
    /// <returns>The amount of the new unit.</returns>
    public static double Convert(double amount, ETimeUnit fromUnit,
        ETimeUnit toUnit = ETimeUnit.Tick)
    {
        var ticks = fromUnit switch
        {
            ETimeUnit.Nanosecond => amount * TICKS_PER_NANOSECOND,
            ETimeUnit.Tick => amount,
            ETimeUnit.Microsecond => amount * TimeSpan.TicksPerMicrosecond,
            ETimeUnit.Millisecond => amount * TimeSpan.TicksPerMillisecond,
            ETimeUnit.Second => amount * TimeSpan.TicksPerSecond,
            ETimeUnit.Minute => amount * TimeSpan.TicksPerMinute,
            ETimeUnit.Hour => amount * TimeSpan.TicksPerHour,
            ETimeUnit.Day => amount * TimeSpan.TicksPerDay,
            ETimeUnit.Week => amount * TICKS_PER_WEEK,
            ETimeUnit.Month => amount * TICKS_PER_MONTH,
            ETimeUnit.Year => amount * TICKS_PER_YEAR,
            ETimeUnit.Decade => amount * TICKS_PER_DECADE,
            ETimeUnit.Century => amount * TICKS_PER_CENTURY,
            ETimeUnit.Millennium => amount * TICKS_PER_MILLENNIUM,
            _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), "Invalid time unit.")
        };

        return toUnit switch
        {
            ETimeUnit.Nanosecond => ticks / TICKS_PER_NANOSECOND,
            ETimeUnit.Tick => ticks,
            ETimeUnit.Microsecond => ticks / TimeSpan.TicksPerMicrosecond,
            ETimeUnit.Millisecond => ticks / TimeSpan.TicksPerMillisecond,
            ETimeUnit.Second => ticks / TimeSpan.TicksPerSecond,
            ETimeUnit.Minute => ticks / TimeSpan.TicksPerMinute,
            ETimeUnit.Hour => ticks / TimeSpan.TicksPerHour,
            ETimeUnit.Day => ticks / TimeSpan.TicksPerDay,
            ETimeUnit.Week => ticks / TICKS_PER_WEEK,
            ETimeUnit.Month => ticks / TICKS_PER_MONTH,
            ETimeUnit.Year => ticks / TICKS_PER_YEAR,
            ETimeUnit.Decade => ticks / TICKS_PER_DECADE,
            ETimeUnit.Century => ticks / TICKS_PER_CENTURY,
            ETimeUnit.Millennium => ticks / TICKS_PER_MILLENNIUM,
            _ => throw new ArgumentOutOfRangeException(nameof(toUnit), "Invalid time unit.")
        };
    }

    #endregion Conversion Methods

    #region Miscelleanous conversion factors

    /// <summary>
    /// The number of minutes in an hour.
    /// </summary>
    public const long MINUTES_PER_HOUR = 60L;

    /// <summary>
    /// The number of minutes in a day.
    /// </summary>
    public const long MINUTES_PER_DAY = 1440L;

    /// <summary>
    /// The number of hours in an ephemeris day.
    /// </summary>
    public const long HOURS_PER_DAY = 24L;

    /// <summary>
    /// The number of hours in a week.
    /// </summary>
    public const long HOURS_PER_WEEK = 168L;

    /// <summary>
    /// The number of weeks in an average Gregorian month.
    /// </summary>
    public const double WEEKS_PER_MONTH = 4.348_125;

    /// <summary>
    /// The number of weeks in a Gregorian year.
    /// </summary>
    public const double WEEKS_PER_YEAR = 52.1775;

    /// <summary>
    /// The number of months in a Gregorian year.
    /// </summary>
    public const long MONTHS_PER_YEAR = 12L;

    #endregion Miscelleanous conversion factors

    #region Ticks per unit of time

    /// <summary>
    /// The number of ticks in a nanosecond.
    /// </summary>
    public const double TICKS_PER_NANOSECOND = 0.01;

    /// <summary>
    /// The number of ticks in a week.
    /// </summary>
    public const long TICKS_PER_WEEK = 6_048_000_000_000L;

    /// <summary>
    /// The number of ticks in a month.
    /// </summary>
    public const long TICKS_PER_MONTH = 26_297_460_000_000L;

    /// <summary>
    /// The number of ticks in a Gregorian year.
    /// </summary>
    public const long TICKS_PER_YEAR = 315_569_520_000_000L;

    /// <summary>
    /// The number of ticks in a Gregorian decade.
    /// </summary>
    public const long TICKS_PER_DECADE = 3_155_695_200_000_000L;

    /// <summary>
    /// The number of ticks in a Gregorian century.
    /// </summary>
    public const long TICKS_PER_CENTURY = 31_556_952_000_000_000L;

    /// <summary>
    /// The number of ticks in a Gregorian millennium.
    /// </summary>
    public const long TICKS_PER_MILLENNIUM = 315_569_520_000_000_000L;

    #endregion Seconds per unit of time

    #region Seconds per unit of time

    /// <summary>
    /// The number of seconds in a nanosecond.
    /// </summary>
    public const double SECONDS_PER_TICK = 1e-07;

    /// <summary>
    /// The number of seconds in a microsecond.
    /// </summary>
    public const double SECONDS_PER_MICROSECOND = 1e-6;

    /// <summary>
    /// The number of seconds in a millisecond.
    /// </summary>
    public const double SECONDS_PER_MILLISECOND = 1e-3;

    /// <summary>
    /// The number of seconds in a minute.
    /// </summary>
    public const long SECONDS_PER_MINUTE = 60L;

    /// <summary>
    /// The number of seconds in an hour.
    /// </summary>
    public const long SECONDS_PER_HOUR = 3600L;

    /// <summary>
    /// The number of seconds in an ephemeris day.
    /// </summary>
    public const long SECONDS_PER_DAY = 86_400L;

    /// <summary>
    /// The number of seconds in a week.
    /// </summary>
    public const long SECONDS_PER_WEEK = 604_800L;

    /// <summary>
    /// The average number of seconds in a month.
    /// </summary>
    public const long SECONDS_PER_MONTH = 2_629_746L;

    /// <summary>
    /// The average number of seconds in a Gregorian year.
    /// </summary>
    public const long SECONDS_PER_YEAR = 31_556_952L;

    /// <summary>
    /// The average number of seconds in a Gregorian decade.
    /// </summary>
    public const long SECONDS_PER_DECADE = 315_569_520L;

    /// <summary>
    /// The average number of seconds in a Gregorian century.
    /// </summary>
    public const long SECONDS_PER_CENTURY = 3_155_695_200L;

    /// <summary>
    /// The average number of seconds in a Gregorian millennium.
    /// </summary>
    public const long SECONDS_PER_MILLENNIUM = 31_556_952_000L;

    #endregion Seconds per unit of time

    #region Days per unit of time

    /// <summary>
    /// The number of days in a week.
    /// </summary>
    public const long DAYS_PER_WEEK = 7L;

    /// <summary>
    /// The average number of days in a Gregorian month.
    /// </summary>
    public const double DAYS_PER_MONTH = 30.436_875;

    /// <summary>
    /// The average number of days in a Gregorian year.
    /// </summary>
    public const double DAYS_PER_YEAR = 365.2425;

    /// <summary>
    /// The average number of days in a Gregorian decade.
    /// </summary>
    public const double DAYS_PER_DECADE = 3652.425;

    /// <summary>
    /// The average number of days in a Gregorian century.
    /// </summary>
    public const double DAYS_PER_CENTURY = 36_524.25;

    /// <summary>
    /// The average number of days in a Gregorian millennium.
    /// </summary>
    public const double DAYS_PER_MILLENNIUM = 365_242.5;

    #endregion Days per unit of time

    #region Years per unit of time

    /// <summary>
    /// Number of years in an olympiad.
    /// </summary>
    public const long YEARS_PER_OLYMPIAD = 4L;

    /// <summary>
    /// The number of years in a decade.
    /// The precise length of a decade depends on what year is being used.
    /// For a example, a Gregorian decade (3652.425 d on average) will have a different
    /// length to an Islamic Calendar decade (about 3543.67 d on average) or a tropical decade
    /// (3652.42198781 d on average).
    /// </summary>
    public const long YEARS_PER_DECADE = 10L;

    /// <summary>
    /// The number of years in a century.
    /// </summary>
    public const long YEARS_PER_CENTURY = 100L;

    /// <summary>
    /// The number of years in a millennium.
    /// </summary>
    public const long YEARS_PER_MILLENNIUM = 1000L;

    #endregion Years per unit of time

    #region Gregorian solar cycles

    /// <summary>
    /// The Gregorian Calendar repeats on a 400-year cycle called the "Gregorian solar cycle".
    /// (Not to be confused with the 11-year solar cycle.)
    /// There are 97 leap years in that period, giving an average calendar year length of
    /// 365 + (97/400) = 365.2425 days/year.
    /// 1 Gregorian solar cycle = 400 y = 4800 mon = 20,871 w = 146,097 d
    /// 5 Gregorian solar cycles = 2000 y = 2 ky
    /// Gregorian solar cycles are not ordinarily numbered, nor given a specific start date.
    /// However, within the proleptic Gregorian epoch (the one used by .NET), which began on Monday,
    /// 1 Jan, 1 CE, we are currently in the 6th solar cycle. It began on Monday, 1 Jan, 2001, which
    /// was also the first day of the 3rd millennium AD. It will end on Sunday, 31 Dec, 2400.
    /// See:
    /// - <see href="https://en.wikipedia.org/wiki/Solar_cycle_(calendar)"/>
    /// - <see href="https://en.wikipedia.org/wiki/Proleptic_Gregorian_calendar"/>
    /// </summary>
    public const long YEARS_PER_GREGORIAN_SOLAR_CYCLE = 400;

    /// <summary>
    /// Number of olympiads in a Gregorian solar cycle.
    /// </summary>
    public const long OLYMPIADS_PER_GREGORIAN_SOLAR_CYCLE = 100;

    /// <summary>
    /// Number of centuries in a Gregorian solar cycle.
    /// </summary>
    public const long CENTURIES_PER_GREGORIAN_SOLAR_CYCLE = 4;

    /// <summary>
    /// Number of decades in a Gregorian solar cycle.
    /// </summary>
    public const long DECADES_PER_GREGORIAN_SOLAR_CYCLE = 40;

    /// <summary>
    /// The number of leap years in a Gregorian solar cycle.
    /// </summary>
    public const long LEAP_YEARS_PER_GREGORIAN_SOLAR_CYCLE = 97;

    /// <summary>
    /// The number of common years in a Gregorian solar cycle.
    /// </summary>
    public const long COMMON_YEARS_PER_GREGORIAN_SOLAR_CYCLE = 303;

    /// <summary>
    /// The number of months in a Gregorian solar cycle.
    /// </summary>
    public const long MONTHS_PER_GREGORIAN_SOLAR_CYCLE = 4800;

    /// <summary>
    /// The number of weeks in a Gregorian solar cycle.
    /// </summary>
    public const long WEEKS_PER_GREGORIAN_SOLAR_CYCLE = 20_871;

    /// <summary>
    /// The number of days in a Gregorian solar cycle.
    /// </summary>
    public const long DAYS_PER_GREGORIAN_SOLAR_CYCLE = 146_097;

    /// <summary>
    /// The number of seconds in a Gregorian solar cycle.
    /// </summary>
    public const long SECONDS_PER_GREGORIAN_SOLAR_CYCLE = 12_622_780_800;

    /// <summary>
    /// The number of ticks in a Gregorian solar cycle.
    /// </summary>
    public const long TICKS_PER_GREGORIAN_SOLAR_CYCLE = 126_227_808_000_000_000;

    #endregion Solar cycles

    #region Julian Calendar

    /// <summary>
    /// The number of days in a Julian Calendar year.
    /// </summary>
    public const double DAYS_PER_JULIAN_YEAR = 365.25;

    /// <summary>
    /// The number of days in a Julian Calendar decade.
    /// </summary>
    public const double DAYS_PER_JULIAN_DECADE = 3652.5;

    /// <summary>
    /// The number of days in a Julian Calendar century.
    /// </summary>
    public const long DAYS_PER_JULIAN_CENTURY = 36_525L;

    /// <summary>
    /// The number of days in a Julian Calendar millennium.
    /// </summary>
    public const long DAYS_PER_JULIAN_MILLENNIUM = 365_250L;

    #endregion Julian Calendar

    #region Astronomical

    /// <summary>
    /// The number of seconds in a solar day (as at 2023).
    /// It is increasing by about 2 milliseconds per century.
    /// </summary>
    public const double SECONDS_PER_SOLAR_DAY = 86_400.002;

    /// <summary>
    /// Number of days in a synodic lunar month (a.k.a. "lunation").
    /// </summary>
    public const double DAYS_PER_LUNATION = 29.530_588_861;

    /// <summary>
    /// The number of days in the mean tropical year B1900 (days).
    /// This value is taken from the SOFA (Standards of Fundamental Astronomy) library, which is
    /// assumed to be authoritative.
    /// </summary>
    public const double DAYS_PER_TROPICAL_YEAR = 365.242_198_781;

    #endregion Astronomical
}
