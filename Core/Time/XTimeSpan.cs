namespace Galaxon.Core.Time;

/// <summary>
/// Additional members to supplement the TimeSpan class.
///
/// Mostly constants.
/// As in TimeSpan, I've used long as the type for any integer constants.
/// I've used double for non-integer constants.
///
/// The following are already provided by the TimeSpan class and not reproduced here:
///   - TicksPerMicrosecond
///   - TicksPerMillisecond
///   - TicksPerSecond
///   - TicksPerMinute
///   - TicksPerHour
///   - TicksPerDay
///
/// <strong>NOTE: Any constants or methods for converting between time units will become obsolete
/// with the completion of Galaxon.Quantities. So, don't use these for anything permanent.</strong>
///
/// The word Month, Year, Decade, Century, or Millennium in a constant name usually refers to the
/// average length of that time unit in the Gregorian.
/// </summary>
public static class XTimeSpan
{
    #region Conversion Methods

    /// <summary>
    /// Convert a  time value from one unit to another.
    /// TODO Replace with Quantity methods.
    /// </summary>
    /// <param name="amount">The amount.</param>
    /// <param name="fromUnit">The amount argument units.</param>
    /// <param name="toUnit">The result units.</param>
    /// <returns>The amount of the new unit.</returns>
    public static double Convert(double amount, ETimeUnit fromUnit,
        ETimeUnit toUnit = ETimeUnit.Tick)
    {
        double ticks = fromUnit switch
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
            _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), "Invalid time unit.")
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
            _ => throw new ArgumentOutOfRangeException(nameof(toUnit), "Invalid time unit.")
        };
    }

    #endregion Conversion Methods

    #region Miscelleanous other conversion factors

    /// <summary>
    /// The number of minutes in an hour.
    /// </summary>
    public const long MinutesPerHour = 60;

    /// <summary>
    /// The number of hours in an ephemeris (or SI) day.
    /// </summary>
    public const long HoursPerDay = 24;

    /// <summary>
    /// The number of calendar months in a Gregorian year.
    /// </summary>
    public const long MonthsPerYear = 12;

    #endregion Miscelleanous other conversion factors

    #region Ticks per unit of time

    /// <summary>
    /// The number of ticks in a nanosecond.
    /// </summary>
    public const double TicksPerNanosecond = 1.0 / TimeSpan.NanosecondsPerTick;

    /// <summary>
    /// The number of ticks in a week.
    /// </summary>
    public const long TicksPerWeek = TimeSpan.TicksPerDay * DaysPerWeek;

    /// <summary>
    /// The number of ticks in a month.
    /// </summary>
    public const long TicksPerMonth = (long)(TimeSpan.TicksPerDay * DaysPerMonth);

    /// <summary>
    /// The number of ticks in a Gregorian year.
    /// </summary>
    public const long TicksPerYear = (long)(TimeSpan.TicksPerDay * DaysPerYear);

    /// <summary>
    /// The number of ticks in a Gregorian decade.
    /// </summary>
    public const long TicksPerDecade = TicksPerYear * YearsPerDecade;

    /// <summary>
    /// The number of ticks in a Gregorian century.
    /// </summary>
    public const long TicksPerCentury = TicksPerYear * YearsPerCentury;

    /// <summary>
    /// The number of ticks in a Gregorian millennium.
    /// </summary>
    public const long TicksPerMillennium = TicksPerYear * YearsPerMillennium;

    #endregion Seconds per unit of time

    #region Seconds per unit of time

    /// <summary>
    /// The number of seconds in a nanosecond.
    /// </summary>
    public const double SecondsPerTick = 1.0 / TimeSpan.TicksPerSecond;

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
    public const long SecondsPerMinute = 60;

    /// <summary>
    /// The number of seconds in an hour.
    /// </summary>
    public const long SecondsPerHour = SecondsPerMinute * MinutesPerHour;

    /// <summary>
    /// The number of seconds in an ephemeris day.
    /// </summary>
    public const long SecondsPerDay = SecondsPerHour * HoursPerDay;

    /// <summary>
    /// The number of seconds in a solar day (as at 2023).
    /// It is increasing by about 2 milliseconds per century.
    /// </summary>
    public const double SecondsPerSolarDay = 86_400.002;

    /// <summary>
    /// The number of seconds in a week.
    /// </summary>
    public const long SecondsPerWeek = SecondsPerDay * DaysPerWeek;

    /// <summary>
    /// The average number of seconds in a month.
    /// </summary>
    public const long SecondsPerMonth = (long)(SecondsPerDay * DaysPerMonth);

    /// <summary>
    /// The average number of seconds in a Gregorian year.
    /// </summary>
    public const long SecondsPerYear = (long)(SecondsPerDay * DaysPerYear);

    /// <summary>
    /// The average number of seconds in a Gregorian decade.
    /// </summary>
    public const long SecondsPerDecade = SecondsPerYear * YearsPerDecade;

    /// <summary>
    /// The average number of seconds in a Gregorian century.
    /// </summary>
    public const long SecondsPerCentury = SecondsPerYear * YearsPerCentury;

    /// <summary>
    /// The average number of seconds in a Gregorian millennium.
    /// </summary>
    public const long SecondsPerMillennium = SecondsPerYear * YearsPerMillennium;

    #endregion Seconds per unit of time

    #region Days per unit of time

    /// <summary>
    /// The number of solar days in a week.
    /// </summary>
    public const long DaysPerWeek = 7;

    /// <summary>
    /// The average number of days in a Gregorian month.
    /// </summary>
    public const double DaysPerMonth = DaysPerYear / MonthsPerYear;

    /// <summary>
    /// The average number of solar days in a Gregorian year.
    /// </summary>
    public const double DaysPerYear = 365.2425;

    /// <summary>
    /// The average number of solar days in a Gregorian decade.
    /// </summary>
    public const double DaysPerDecade = DaysPerYear * YearsPerDecade;

    /// <summary>
    /// The average number of solar days in a Gregorian century.
    /// </summary>
    public const double DaysPerCentury = DaysPerYear * YearsPerCentury;

    /// <summary>
    /// The average number of solar days in a Gregorian millennium.
    /// </summary>
    public const double DaysPerMillennium = DaysPerYear * YearsPerMillennium;

    /// <summary>
    /// The number of solar days in a Julian Calendar year.
    /// </summary>
    public const double DaysPerJulianYear = 365.25;

    /// <summary>
    /// The number of solar days in a Julian Calendar decade.
    /// </summary>
    public const double DaysPerJulianDecade = DaysPerJulianYear * YearsPerDecade;

    /// <summary>
    /// The number of solar days in a Julian Calendar century.
    /// </summary>
    public const long DaysPerJulianCentury = (long)(DaysPerJulianYear * YearsPerCentury);

    /// <summary>
    /// The number of solar days in a Julian Calendar millennium.
    /// </summary>
    public const long DaysPerJulianMillennium = (long)(DaysPerJulianYear * YearsPerMillennium);

    /// <summary>
    /// The number of days in the mean tropical year B1900 (days).
    /// This value is taken from the SOFA (Standards of Fundamental Astronomy) library, which is
    /// assumed to be authoritative.
    /// </summary>
    public const double DaysPerTropicalYear = 365.242198781;

    /// <summary>
    /// Length of synodic lunar month (a.k.a. "lunation") in days.
    /// </summary>
    public const double DaysPerLunation = 29.530588861;

    #endregion Days per unit of time

    #region Years per unit of time

    /// <summary>
    /// Number of years in an olympiad.
    /// </summary>
    public const long YearsPerOlympiad = 4;

    /// <summary>
    /// The number of years in a decade.
    /// The precise length of a decade depends on what year is being used.
    /// For a example, a Gregorian decade (3652.425 d on average) will have a different
    /// length to an Islamic Calendar decade (about 3543.67 d on average) or a tropical decade
    /// (3652.42198781 d on average).
    /// </summary>
    public const long YearsPerDecade = 10;

    /// <summary>
    /// The number of years in a century.
    /// </summary>
    public const long YearsPerCentury = 100;

    /// <summary>
    /// The number of years in a millennium.
    /// </summary>
    public const long YearsPerMillennium = 1000;

    #endregion Years per unit of time

    #region Solar cycles

    /// <summary>
    /// The Gregorian Calendars repeats on a 400-year cycle called the solar cycle.
    /// There are 97 leap years in that period, giving an average calendar year length of
    /// 365 + (97/400) = 365.2425 days/year
    /// 1 Gregorian solar cycle = 100 olympiads = 4800 mon = 20,871 w = 146,097 d
    /// </summary>
    public const long YearsPerSolarCycle = 400;

    /// <summary>
    /// Number of olympiads in an Gregorian solar cycle.
    /// </summary>
    public const long OlympiadsPerSolarCycle = 100;

    /// <summary>
    /// The number of leap years in a Gregorian solar cycle.
    /// </summary>
    public const long LeapYearsPerSolarCycle = 97;

    /// <summary>
    /// The number of months in a Gregorian solar cycle.
    /// </summary>
    public const long MonthsPerSolarCycle = 4800;

    /// <summary>
    /// The number of weeks in a Gregorian solar cycle.
    /// </summary>
    public const long WeeksPerSolarCycle = 20_871;

    /// <summary>
    /// The number of days in a Gregorian solar cycle.
    /// </summary>
    public const long DaysPerSolarCycle = 146_097;

    #endregion Solar cycles
}
