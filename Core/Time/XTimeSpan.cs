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
            ETimeUnit.Nanosecond => amount * TicksPerNanosecond,
            ETimeUnit.Tick => amount,
            ETimeUnit.Microsecond => amount * TimeSpan.TicksPerMicrosecond,
            ETimeUnit.Millisecond => amount * TimeSpan.TicksPerMillisecond,
            ETimeUnit.Second => amount * TimeSpan.TicksPerSecond,
            ETimeUnit.Minute => amount * TimeSpan.TicksPerMinute,
            ETimeUnit.Hour => amount * TimeSpan.TicksPerHour,
            ETimeUnit.Day => amount * TimeSpan.TicksPerDay,
            ETimeUnit.Week => amount * TicksPerWeek,
            ETimeUnit.Month => amount * TicksPerMonth,
            ETimeUnit.Year => amount * TicksPerYear,
            ETimeUnit.Decade => amount * TicksPerDecade,
            ETimeUnit.Century => amount * TicksPerCentury,
            ETimeUnit.Millennium => amount * TicksPerMillennium,
            _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), "Invalid time unit."),
        };

        return toUnit switch
        {
            ETimeUnit.Nanosecond => ticks / TicksPerNanosecond,
            ETimeUnit.Tick => ticks,
            ETimeUnit.Microsecond => ticks / TimeSpan.TicksPerMicrosecond,
            ETimeUnit.Millisecond => ticks / TimeSpan.TicksPerMillisecond,
            ETimeUnit.Second => ticks / TimeSpan.TicksPerSecond,
            ETimeUnit.Minute => ticks / TimeSpan.TicksPerMinute,
            ETimeUnit.Hour => ticks / TimeSpan.TicksPerHour,
            ETimeUnit.Day => ticks / TimeSpan.TicksPerDay,
            ETimeUnit.Week => ticks / TicksPerWeek,
            ETimeUnit.Month => ticks / TicksPerMonth,
            ETimeUnit.Year => ticks / TicksPerYear,
            ETimeUnit.Decade => ticks / TicksPerDecade,
            ETimeUnit.Century => ticks / TicksPerCentury,
            ETimeUnit.Millennium => ticks / TicksPerMillennium,
            _ => throw new ArgumentOutOfRangeException(nameof(toUnit), "Invalid time unit."),
        };
    }

    #endregion Conversion Methods

    #region Miscelleanous conversion factors

    /// <summary>
    /// The number of minutes in an hour.
    /// </summary>
    public const long MinutesPerHour = 60L;

    /// <summary>
    /// The number of minutes in a day.
    /// </summary>
    public const long MinutesPerDay = 1440L;

    /// <summary>
    /// The number of hours in an ephemeris day.
    /// </summary>
    public const long HoursPerDay = 24L;

    /// <summary>
    /// The number of hours in a week.
    /// </summary>
    public const long HoursPerWeek = 168L;

    /// <summary>
    /// The number of weeks in an average Gregorian month.
    /// </summary>
    public const double WeeksPerMonth = 4.348_125;

    /// <summary>
    /// The number of weeks in a Gregorian year.
    /// </summary>
    public const double WeeksPerYear = 52.1775;

    /// <summary>
    /// The number of months in a Gregorian year.
    /// </summary>
    public const long MonthsPerYear = 12L;

    #endregion Miscelleanous conversion factors

    #region Ticks per unit of time

    /// <summary>
    /// The number of ticks in a nanosecond.
    /// </summary>
    public const double TicksPerNanosecond = 0.01;

    /// <summary>
    /// The number of ticks in a week.
    /// </summary>
    public const long TicksPerWeek = 6_048_000_000_000L;

    /// <summary>
    /// The number of ticks in a month.
    /// </summary>
    public const long TicksPerMonth = 26_297_460_000_000L;

    /// <summary>
    /// The number of ticks in a Gregorian year.
    /// </summary>
    public const long TicksPerYear = 315_569_520_000_000L;

    /// <summary>
    /// The number of ticks in a Gregorian decade.
    /// </summary>
    public const long TicksPerDecade = 3_155_695_200_000_000L;

    /// <summary>
    /// The number of ticks in a Gregorian century.
    /// </summary>
    public const long TicksPerCentury = 31_556_952_000_000_000L;

    /// <summary>
    /// The number of ticks in a Gregorian millennium.
    /// </summary>
    public const long TicksPerMillennium = 315_569_520_000_000_000L;

    #endregion Seconds per unit of time

    #region Seconds per unit of time

    /// <summary>
    /// The number of seconds in a nanosecond.
    /// </summary>
    public const double SecondsPerTick = 1e-07;

    /// <summary>
    /// The number of seconds in a microsecond.
    /// </summary>
    public const double SecondsPerMicrosecond = 1e-6;

    /// <summary>
    /// The number of seconds in a millisecond.
    /// </summary>
    public const double SecondsPerMillisecond = 1e-3;

    /// <summary>
    /// The number of seconds in a minute.
    /// </summary>
    public const long SecondsPerMinute = 60L;

    /// <summary>
    /// The number of seconds in an hour.
    /// </summary>
    public const long SecondsPerHour = 3600L;

    /// <summary>
    /// The number of seconds in an ephemeris day.
    /// </summary>
    public const long SecondsPerDay = 86_400L;

    /// <summary>
    /// The number of seconds in a week.
    /// </summary>
    public const long SecondsPerWeek = 604_800L;

    /// <summary>
    /// The average number of seconds in a month.
    /// </summary>
    public const long SecondsPerMonth = 2_629_746L;

    /// <summary>
    /// The average number of seconds in a Gregorian year.
    /// </summary>
    public const long SecondsPerYear = 31_556_952L;

    /// <summary>
    /// The average number of seconds in a Gregorian decade.
    /// </summary>
    public const long SecondsPerDecade = 315_569_520L;

    /// <summary>
    /// The average number of seconds in a Gregorian century.
    /// </summary>
    public const long SecondsPerCentury = 3_155_695_200L;

    /// <summary>
    /// The average number of seconds in a Gregorian millennium.
    /// </summary>
    public const long SecondsPerMillennium = 31_556_952_000L;

    #endregion Seconds per unit of time

    #region Days per unit of time

    /// <summary>
    /// The number of days in a week.
    /// </summary>
    public const long DaysPerWeek = 7L;

    /// <summary>
    /// The average number of days in a Gregorian month.
    /// </summary>
    public const double DaysPerMonth = 30.436_875;

    /// <summary>
    /// The average number of days in a Gregorian year.
    /// </summary>
    public const double DaysPerYear = 365.2425;

    /// <summary>
    /// The average number of days in a Gregorian decade.
    /// </summary>
    public const double DaysPerDecade = 3652.425;

    /// <summary>
    /// The average number of days in a Gregorian century.
    /// </summary>
    public const double DaysPerCentury = 36_524.25;

    /// <summary>
    /// The average number of days in a Gregorian millennium.
    /// </summary>
    public const double DaysPerMillennium = 365_242.5;

    #endregion Days per unit of time

    #region Years per unit of time

    /// <summary>
    /// Number of years in an olympiad.
    /// </summary>
    public const long YearsPerOlympiad = 4L;

    /// <summary>
    /// The number of years in a decade.
    /// The precise length of a decade depends on what year is being used.
    /// For a example, a Gregorian decade (3652.425 d on average) will have a different
    /// length to an Islamic Calendar decade (about 3543.67 d on average) or a tropical decade
    /// (3652.42198781 d on average).
    /// </summary>
    public const long YearsPerDecade = 10L;

    /// <summary>
    /// The number of years in a century.
    /// </summary>
    public const long YearsPerCentury = 100L;

    /// <summary>
    /// The number of years in a millennium.
    /// </summary>
    public const long YearsPerMillennium = 1000L;

    #endregion Years per unit of time

    #region Solar cycles

    /// <summary>
    /// The Gregorian Calendars repeats on a 400-year cycle called the *solar cycle*.
    /// There are 97 leap years in that period, giving an average calendar year length of
    /// 365 + (97/400) = 365.2425 days/year.
    /// 1 Gregorian solar cycle = 400 y = 4800 mon = 20,871 w = 146,097 d
    /// 5 Gregorian solar cycles = 2000 y = 2 ky
    /// Solar cycles are not ordinarily numbered, nor given a specific start date.
    /// However, within the proleptic Gregorian epoch (the one used by .NET), which began on Monday,
    /// 1 Jan, 1 AD, we are currently in the 6th solar cycle. It began on Monday, 1 Jan, 2001, which
    /// was also the first day of the 3rd millennium AD. It will end on Sunday, 31 Dec, 2400.
    /// See:
    /// - <see href="https://en.wikipedia.org/wiki/Solar_cycle_(calendar)" />
    /// - <see href="https://en.wikipedia.org/wiki/Proleptic_Gregorian_calendar" />
    /// </summary>
    public const long YearsPerSolarCycle = 400L;

    /// <summary>
    /// Number of olympiads in an Gregorian solar cycle.
    /// </summary>
    public const long OlympiadsPerSolarCycle = 100L;

    /// <summary>
    /// The number of leap years in a Gregorian solar cycle.
    /// </summary>
    public const long LeapYearsPerSolarCycle = 97L;

    /// <summary>
    /// The number of common years in a Gregorian solar cycle.
    /// </summary>
    public const long CommonYearsPerSolarCycle = 303L;

    /// <summary>
    /// The number of months in a Gregorian solar cycle.
    /// </summary>
    public const long MonthsPerSolarCycle = 4800L;

    /// <summary>
    /// The number of weeks in a Gregorian solar cycle.
    /// </summary>
    public const long WeeksPerSolarCycle = 20_871L;

    /// <summary>
    /// The number of days in a Gregorian solar cycle.
    /// </summary>
    public const long DaysPerSolarCycle = 146_097L;

    /// <summary>
    /// The number of seconds in a Gregorian solar cycle.
    /// </summary>
    public const long SecondsPerSolarCycle = 12_622_780_800L;

    /// <summary>
    /// The number of ticks in a Gregorian solar cycle.
    /// </summary>
    public const long TicksPerSolarCycle = 126_227_808_000_000_000L;

    #endregion Solar cycles

    #region Julian Calendar

    /// <summary>
    /// The number of days in a Julian Calendar year.
    /// </summary>
    public const double DaysPerJulianYear = 365.25;

    /// <summary>
    /// The number of days in a Julian Calendar decade.
    /// </summary>
    public const double DaysPerJulianDecade = 3652.5;

    /// <summary>
    /// The number of days in a Julian Calendar century.
    /// </summary>
    public const long DaysPerJulianCentury = 36_525L;

    /// <summary>
    /// The number of days in a Julian Calendar millennium.
    /// </summary>
    public const long DaysPerJulianMillennium = 365_250L;

    #endregion Julian Calendar

    #region Astronomical

    /// <summary>
    /// The number of seconds in a solar day (as at 2023).
    /// It is increasing by about 2 milliseconds per century.
    /// </summary>
    public const double SecondsPerSolarDay = 86_400.002;

    /// <summary>
    /// Number of days in a synodic lunar month (a.k.a. "lunation").
    /// </summary>
    public const double DaysPerLunation = 29.530_588_861;

    /// <summary>
    /// The number of days in the mean tropical year B1900 (days).
    /// This value is taken from the SOFA (Standards of Fundamental Astronomy) library, which is
    /// assumed to be authoritative.
    /// </summary>
    public const double DaysPerTropicalYear = 365.242_198_781;

    #endregion Astronomical
}
