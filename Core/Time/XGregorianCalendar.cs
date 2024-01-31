using Galaxon.Core.Exceptions;
using Galaxon.Core.Numbers;

namespace Galaxon.Core.Time;

public static class XGregorianCalendar
{
    #region Conversion factors

    /// <summary>
    /// The average number of weeks in a Gregorian calendar month.
    /// </summary>
    public const decimal WEEKS_PER_MONTH = 4.348_125M;

    /// <summary>
    /// The average number of weeks in a Gregorian calendar year.
    /// </summary>
    public const decimal WEEKS_PER_YEAR = 52.1775M;

    /// <summary>
    /// The number of months in a Gregorian calendar year.
    /// </summary>
    public const long MONTHS_PER_YEAR = 12L;

    /// <summary>
    /// The average number of ticks in a Gregorian calendar month.
    /// </summary>
    public const long TICKS_PER_MONTH = 26_297_460_000_000L;

    /// <summary>
    /// The average number of ticks in a Gregorian calendar year.
    /// </summary>
    public const long TICKS_PER_YEAR = 315_569_520_000_000L;

    /// <summary>
    /// The average number of ticks in a Gregorian calendar decade.
    /// </summary>
    public const long TICKS_PER_DECADE = 3_155_695_200_000_000L;

    /// <summary>
    /// The average number of ticks in a Gregorian calendar century.
    /// </summary>
    public const long TICKS_PER_CENTURY = 31_556_952_000_000_000L;

    /// <summary>
    /// The average number of ticks in a Gregorian calendar millennium.
    /// </summary>
    public const long TICKS_PER_MILLENNIUM = 315_569_520_000_000_000L;

    /// <summary>
    /// The average number of milliseconds in a Gregorian calendar month.
    /// </summary>
    public const long MILLISECONDS_PER_MONTH = 2_629_746_000L;

    /// <summary>
    /// The average number of milliseconds in a Gregorian calendar year.
    /// </summary>
    public const long MILLISECONDS_PER_YEAR = 31_556_952_000L;

    /// <summary>
    /// The average number of milliseconds in a Gregorian calendar decade.
    /// </summary>
    public const long MILLISECONDS_PER_DECADE = 315_569_520_000L;

    /// <summary>
    /// The average number of milliseconds in a Gregorian calendar century.
    /// </summary>
    public const long MILLISECONDS_PER_CENTURY = 3_155_695_200_000L;

    /// <summary>
    /// The average number of milliseconds in a Gregorian calendar millennium.
    /// </summary>
    public const long MILLISECONDS_PER_MILLENNIUM = 31_556_952_000_000L;

    /// <summary>
    /// The average number of seconds in a Gregorian calendar month.
    /// </summary>
    public const long SECONDS_PER_MONTH = 2_629_746L;

    /// <summary>
    /// The average number of seconds in a Gregorian calendar year.
    /// </summary>
    public const long SECONDS_PER_YEAR = 31_556_952L;

    /// <summary>
    /// The average number of seconds in a Gregorian calendar decade.
    /// </summary>
    public const long SECONDS_PER_DECADE = 315_569_520L;

    /// <summary>
    /// The average number of seconds in a Gregorian calendar century.
    /// </summary>
    public const long SECONDS_PER_CENTURY = 3_155_695_200L;

    /// <summary>
    /// The average number of seconds in a Gregorian calendar millennium.
    /// </summary>
    public const long SECONDS_PER_MILLENNIUM = 31_556_952_000L;

    /// <summary>
    /// The average number of days in a Gregorian calendar month.
    /// </summary>
    public const decimal DAYS_PER_MONTH = 30.436_875M;

    /// <summary>
    /// The average number of days in a Gregorian calendar year.
    /// </summary>
    public const decimal DAYS_PER_YEAR = 365.2425M;

    /// <summary>
    /// The average number of days in a Gregorian calendar decade.
    /// </summary>
    public const decimal DAYS_PER_DECADE = 3652.425M;

    /// <summary>
    /// The average number of days in a Gregorian calendar century.
    /// </summary>
    public const decimal DAYS_PER_CENTURY = 36_524.25M;

    /// <summary>
    /// The average number of days in a Gregorian calendar millennium.
    /// </summary>
    public const decimal DAYS_PER_MILLENNIUM = 365_242.5M;

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
    public const long YEARS_PER_GREGORIAN_SOLAR_CYCLE = 400L;

    /// <summary>
    /// Number of olympiads in a Gregorian solar cycle.
    /// </summary>
    public const long OLYMPIADS_PER_GREGORIAN_SOLAR_CYCLE = 100L;

    /// <summary>
    /// Number of centuries in a Gregorian solar cycle.
    /// </summary>
    public const long CENTURIES_PER_GREGORIAN_SOLAR_CYCLE = 4L;

    /// <summary>
    /// Number of decades in a Gregorian solar cycle.
    /// </summary>
    public const long DECADES_PER_GREGORIAN_SOLAR_CYCLE = 40L;

    /// <summary>
    /// The number of leap years in a Gregorian solar cycle.
    /// </summary>
    public const long LEAP_YEARS_PER_GREGORIAN_SOLAR_CYCLE = 97L;

    /// <summary>
    /// The number of common years in a Gregorian solar cycle.
    /// </summary>
    public const long COMMON_YEARS_PER_GREGORIAN_SOLAR_CYCLE = 303L;

    /// <summary>
    /// The number of months in a Gregorian solar cycle.
    /// </summary>
    public const long MONTHS_PER_GREGORIAN_SOLAR_CYCLE = 4800L;

    /// <summary>
    /// The number of weeks in a Gregorian solar cycle.
    /// </summary>
    public const long WEEKS_PER_GREGORIAN_SOLAR_CYCLE = 20_871L;

    /// <summary>
    /// The number of days in a Gregorian solar cycle.
    /// </summary>
    public const long DAYS_PER_GREGORIAN_SOLAR_CYCLE = 146_097L;

    /// <summary>
    /// The number of seconds in a Gregorian solar cycle.
    /// </summary>
    public const long SECONDS_PER_GREGORIAN_SOLAR_CYCLE = 12_622_780_800L;

    /// <summary>
    /// The number of ticks in a Gregorian solar cycle.
    /// </summary>
    public const long TICKS_PER_GREGORIAN_SOLAR_CYCLE = 126_227_808_000_000_000L;

    #endregion Conversion factors

    #region Replacment methods that support negative years

    /// <summary>
    /// Check if a year is a leap year.
    /// As we're using the floored division version of the modulo operator, years can be negative.
    /// The .NET GregorianCalendar and DateTime classes only supports positive years.
    /// To convert from BCE (BC) to a negative (proleptic) Gregorian Calendar year number, subtract
    /// the year from 1.
    ///   e.g. 45 BCE = 1 - 45 = -44
    /// </summary>
    /// <param name="y">The year number in the proleptic Gregorian Calendar.</param>
    /// <returns>If the year is a leap year.</returns>
    public static bool IsLeapYear(int y)
    {
        return XNumber.Mod(y, 400) == 0
            || (XNumber.Mod(y, 4) == 0 && XNumber.Mod(y, 100) != 0);
    }

    /// <summary>
    /// Get the number of days in a year.
    /// Supports negative years, unlike the .NET GregorianCalendar or DateTime classes.
    /// </summary>
    /// <param name="y">The year number in the proleptic Gregorian Calendar.</param>
    /// <returns>The number of days in the year.</returns>
    public static int DaysInYear(int y)
    {
        return IsLeapYear(y) ? 366 : 365;
    }

    /// <summary>
    /// Get the number of days in a month.
    /// Supports negative years, unlike the .NET GregorianCalendar or DateTime classes.
    /// </summary>
    /// <param name="y">The year number in the proleptic Gregorian Calendar.</param>
    /// <param name="m">The month number.</param>
    /// <returns>The number of days in the month.</returns>
    public static int DaysInMonth(int y, int m)
    {
        return m switch
        {
            1 or 3 or 5 or 7 or 8 or 10 or 12 => 31,
            4 or 6 or 9 or 11 => 30,
            2 => IsLeapYear(y) ? 29 : 28,
            _ => throw new ArgumentOutOfRangeException(nameof(m), "Must be in range 1-12.")
        };
    }

    /// <summary>
    /// Get the number of seconds in a year.
    /// Supports negative years, unlike the .NET GregorianCalendar or DateTime classes.
    /// Currently does not support leap seconds. May add this capability later.
    /// </summary>
    /// <param name="y">The year number in the proleptic Gregorian Calendar.</param>
    /// <returns>The number of seconds in the year.</returns>
    public static long SecondsInYear(int y)
    {
        return DaysInYear(y) * XTimeSpan.SECONDS_PER_DAY;
    }

    /// <summary>
    /// Which day of the year is a given date.
    /// Supports negative years, unlike the .NET GregorianCalendar or DateTime classes.
    /// </summary>
    /// <param name="y">The year number in the proleptic Gregorian Calendar.</param>
    /// <param name="m">The month number.</param>
    /// <param name="d">The day of the month.</param>
    /// <returns>The day of the year.</returns>
    public static int DayOfYear(int y, int m, int d)
    {
        // Check month.
        if (m is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(m), "Must be in range 1-12.");
        }

        // Check day.
        int dim = DaysInMonth(y, m);
        if (d < 1 || d > dim)
        {
            throw new ArgumentOutOfRangeException(nameof(d), $"Must be in range 1-{dim}.");
        }

        // Calculate day of year.
        int doy = d;
        for (var n = 1; n < m; n++)
        {
            doy += DaysInMonth(y, n);
        }
        return doy;
    }

    #endregion Replacment methods that support negative years

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
    public static DateOnly Easter(int year)
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
    /// Get the date of Christmas Day in the given year.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <returns>The date of Christmas in the given year.</returns>
    public static DateOnly Christmas(int year)
    {
        return new DateOnly(year, 12, 31);
    }

    /// <summary>
    /// Find the nth weekday in a given month.
    ///
    /// Example: Get the 4th Thursday in January, 2023.
    /// <code>
    /// DateOnly meetup = XDateOnly.GetNthWeekdayInMonth(2023, 1, 4, DayOfWeek.Thursday);
    /// </code>
    ///
    /// A negative value for n means count from the end of the month.
    /// n = -1 means the last one in the month. n = -2 means the second-last, etc.
    /// Example: Get the last Monday in November, 2025.
    /// <code>
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
    public static DateOnly NthWeekdayInMonth(int year, int month, int n, DayOfWeek dayOfWeek)
    {
        if (Math.Abs(n) is < 1 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(n),
                "The absolute value must be in the range 1..5.");
        }

        // Get the number of days in the month.
        int daysInMonth = DaysInMonth(year, month);
        var daysPerWeek = (int)XTimeSpan.DAYS_PER_WEEK;
        int day;

        if (n > 0)
        {
            // Get the number of days difference between the start of the month and the result.
            DateOnly firstOfMonth = new (year, month, 1);
            int diffDays = dayOfWeek - firstOfMonth.DayOfWeek;
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
            int diffDays = lastOfMonth.DayOfWeek - dayOfWeek;
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
    public static DateOnly Thanksgiving(int year, string countryCode = "US")
    {
        return countryCode switch
        {
            // Norfolk Island: Last Wednesday in November.
            "NF" => NthWeekdayInMonth(year, 11, -1, DayOfWeek.Wednesday),

            // Canada: 2nd Monday in October.
            "CA" => NthWeekdayInMonth(year, 10, 2, DayOfWeek.Monday),

            // Grenada: October 25.
            "GD" => new DateOnly(year, 10, 25),

            // Liberia: 1st Thursday in November.
            "LR" => NthWeekdayInMonth(year, 11, 1, DayOfWeek.Thursday),

            // Rwanda: 1st Friday in August.
            "RW" => NthWeekdayInMonth(year, 8, 1, DayOfWeek.Friday),

            // Saint Lucia: 1st Monday in October.
            "LC" => NthWeekdayInMonth(year, 10, 1, DayOfWeek.Monday),

            // US, etc.: 4th Thursday in November.
            "US" or "NL" or "PH" or "BR" => NthWeekdayInMonth(year, 11, 4, DayOfWeek.Thursday),

            _ => throw new ArgumentOutOfRangeException(nameof(countryCode),
                "A date for Thanksgiving could not be determined for the specified country.")
        };
    }

    #endregion Find special dates
}
