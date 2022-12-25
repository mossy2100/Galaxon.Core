namespace Galaxon.Core.Time;

/// <summary>
/// Additional members to supplement the TimeSpan class.
///
/// Mostly constants.
/// As in TimeSpan, I've used long as the type for any integer constants.
/// I've used double for non-integer constants.
///
/// The word Month, Year, Decade, Century, or Millennium in a constant name refers to the average
/// length of that time unit in the Gregorian Calendar.
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
        ETimeUnit toUnit = ETimeUnit.Ticks)
    {
        double ticks = fromUnit switch
        {
            ETimeUnit.Nanoseconds => amount * TicksPerNanosecond,
            ETimeUnit.Ticks => amount,
            ETimeUnit.Microseconds => amount * TimeSpan.TicksPerMicrosecond,
            ETimeUnit.Milliseconds => amount * TimeSpan.TicksPerMillisecond,
            ETimeUnit.Seconds => amount * TimeSpan.TicksPerSecond,
            ETimeUnit.Minutes => amount * TimeSpan.TicksPerMinute,
            ETimeUnit.Hours => amount * TimeSpan.TicksPerHour,
            ETimeUnit.Days => amount * TimeSpan.TicksPerDay,
            ETimeUnit.Weeks => amount * TicksPerWeek,
            ETimeUnit.Months => amount * TicksPerMonth,
            ETimeUnit.Years => amount * TicksPerYear,
            ETimeUnit.Decades => amount * TicksPerDecade,
            ETimeUnit.Centuries => amount * TicksPerCentury,
            ETimeUnit.Millennia => amount * TicksPerMillennium,
            _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), "Invalid time unit.")
        };

        return toUnit switch
        {
            ETimeUnit.Nanoseconds => ticks / TicksPerNanosecond,
            ETimeUnit.Ticks => ticks,
            ETimeUnit.Microseconds => ticks / TimeSpan.TicksPerMicrosecond,
            ETimeUnit.Milliseconds => ticks / TimeSpan.TicksPerMillisecond,
            ETimeUnit.Seconds => ticks / TimeSpan.TicksPerSecond,
            ETimeUnit.Minutes => ticks / TimeSpan.TicksPerMinute,
            ETimeUnit.Hours => ticks / TimeSpan.TicksPerHour,
            ETimeUnit.Days => ticks / TimeSpan.TicksPerDay,
            ETimeUnit.Weeks => ticks / TicksPerWeek,
            ETimeUnit.Months => ticks / TicksPerMonth,
            ETimeUnit.Years => ticks / TicksPerYear,
            ETimeUnit.Decades => ticks / TicksPerDecade,
            ETimeUnit.Centuries => ticks / TicksPerCentury,
            ETimeUnit.Millennia => ticks / TicksPerMillennium,
            _ => throw new ArgumentOutOfRangeException(nameof(toUnit), "Invalid time unit.")
        };
    }

    #endregion Conversion Methods

    #region Basic multipliers

    // From these we can determine all others.
    public const long SecondsPerMinute = 60;
    public const long MinutesPerHour = 60;
    public const long HoursPerDay = 24;

    public const long DaysPerWeek = 7;

    // Here we're referring to days per Gregorian calendar year.
    public const double DaysPerYear = 365.2425;

    // Here we're referring to months per Gregorian calendar year.
    public const long MonthsPerYear = 12;
    public const long YearsPerDecade = 10;
    public const long YearsPerCentury = 100;
    public const long YearsPerMillennium = 1000;

    #endregion Basic multipliers

    #region Ticks per unit of time

    public const double TicksPerNanosecond = 1.0 / TimeSpan.NanosecondsPerTick;
    public const long TicksPerWeek = TimeSpan.TicksPerDay * DaysPerWeek;
    public const long TicksPerMonth = (long)(TimeSpan.TicksPerDay * DaysPerMonth);
    public const long TicksPerYear = (long)(TimeSpan.TicksPerDay * DaysPerYear);
    public const long TicksPerDecade = TicksPerYear * YearsPerDecade;
    public const long TicksPerCentury = TicksPerYear * YearsPerCentury;
    public const long TicksPerMillennium = TicksPerYear * YearsPerMillennium;

    // The following are provided by the TimeSpan class:
    //   - TicksPerMicrosecond
    //   - TicksPerMillisecond
    //   - TicksPerSecond
    //   - TicksPerMinute
    //   - TicksPerHour
    //   - TicksPerDay

    #endregion Seconds per unit of time

    #region Seconds per unit of time

    public const double SecondsPerNanosecond = 1e-9;
    public const double SecondsPerTick = 1.0 / TimeSpan.TicksPerSecond;
    public const double SecondsPerMicrosecond = 1e-6;
    public const double SecondsPerMillisecond = 1e-3;
    public const long SecondsPerHour = SecondsPerMinute * MinutesPerHour;
    public const long SecondsPerDay = SecondsPerHour * HoursPerDay;
    public const long SecondsPerWeek = SecondsPerDay * DaysPerWeek;
    public const long SecondsPerMonth = (long)(SecondsPerDay * DaysPerMonth);
    public const long SecondsPerYear = (long)(SecondsPerDay * DaysPerYear);
    public const long SecondsPerDecade = SecondsPerYear * YearsPerDecade;
    public const long SecondsPerCentury = SecondsPerYear * YearsPerCentury;
    public const long SecondsPerMillennium = SecondsPerYear * YearsPerMillennium;

    #endregion Seconds per unit of time

    #region Days per unit of time

    public const double DaysPerMonth = DaysPerYear / MonthsPerYear;
    public const double DaysPerDecade = DaysPerYear * YearsPerDecade;
    public const double DaysPerCentury = DaysPerYear * YearsPerCentury;
    public const double DaysPerMillennium = DaysPerYear * YearsPerMillennium;

    public const double DaysPerJulianYear = 365.25;
    public const double DaysPerJulianDecade = DaysPerJulianYear * YearsPerDecade;
    public const long DaysPerJulianCentury = (long)(DaysPerJulianYear * YearsPerCentury);
    public const long DaysPerJulianMillennium = (long)(DaysPerJulianYear * YearsPerMillennium);

    /// <summary>
    /// Length of tropical year B1900 (days) (value copied from SOFA).
    /// </summary>
    public const double DaysPerTropicalYear = 365.242198781;

    // Possible leap year fractions:
    // 97 / 400 = 0.2425 (Gregorian)
    // 969 / 4000 = 0.24225 (modified Gregorian - if a year is divisible by 4000 then it's not a leap year)
    // 8 / 33 = .242424... (Iranian)
    // 872 / 3600 = .2422222... (sexagenary)
    // 29 / 120 = 0.24166666666667 (sexagenary)
    // 31 / 128 = 0.2421875 (mine)
    // 1211 / 5000 = 0.2422 (millennial)
    // 807 / 3332 = 0.2421968787515 (modified Gregorian - if a year is divisible by 3332 then it's not a leap year)

    /// <summary>
    /// Length of synodic lunar month (a.k.a. "lunation") in days.
    /// </summary>
    public const double DaysPerLunation = 29.530588861;

    #endregion Days per unit of time

    #region Gregorian calendar cycles

    /// <summary>
    /// The Gregorian Calendars repeats on a 400-year cycle.
    /// There are 97 leap years in that period, giving an average calendar year length of
    /// 365 + (97/400) = 365.2425 days/year
    /// There are a whole number of months, weeks, and days in that period.
    /// </summary>
    public const long YearsPerCycle = 400;

    public const long LeapYearsPerCycle = 97;
    public const long MonthsPerCycle = 4800;
    public const long DaysPerCycle = 146_097; // 3 * 3 * 3 * 7 * 773
    public const long WeeksPerCycle = 20_871; // 3 * 3 * 3 * 773

    #endregion Gregorian calendar cycles
}
