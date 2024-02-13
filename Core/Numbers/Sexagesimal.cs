using System.Numerics;
using Galaxon.Core.Strings;

namespace Galaxon.Core.Numbers;

/// <summary>
/// Utility class for converting decimal numbers to sexagesimal values. This can be integer,
/// minutes, and seconds parts, or formatted strings.
/// </summary>
public static class Sexagesimal
{
    /// <summary>
    /// The radix or base of sexagesimal numbers.
    /// </summary>
    public const int BASE = 60;

    /// <summary>
    /// Map of digit position in sexagesimal number to the correct character for degrees, minutes,
    /// seconds notation.
    /// </summary>
    private static readonly Dictionary<int, char> _POSITION_TO_PRIMES = new ()
    {
        { -3, '‴' },
        { -2, '″' },
        { -1, '′' },
        { 0, '°' },
        { 1, '‵' },
        { 2, '‶' },
        { 3, '‷' },
    };

    /// <summary>
    /// Get the correct string of degree or prime characters to indicate the position of a
    /// sexagesimal digit within a number.
    /// </summary>
    /// <param name="digit">The digit</param>
    /// <param name="position"></param>
    /// <returns></returns>
    private static string PositionToPrimes(byte digit, int position)
    {
        if (digit >= BASE)
        {
            throw new ArgumentOutOfRangeException(nameof(digit), $"Must be in the range 0-{BASE - 1}.");
        }

        BigInteger q, r;

        switch (position)
        {
            case >= -3 and <= 3:
                return _POSITION_TO_PRIMES[position].ToString();

            case < -3:
                (q, r) = XBigInteger.DivMod(-position, 3);
                return XString.Repeat(_POSITION_TO_PRIMES[-3].ToString(), (int)q)
                    + _POSITION_TO_PRIMES[-(int)r];

            case > 3:
                (q, r) = XBigInteger.DivMod(position, 3);
                return XString.Repeat(_POSITION_TO_PRIMES[3].ToString(), (int)q)
                    + _POSITION_TO_PRIMES[(int)r];
        }
    }

    /// <summary>
    /// Convert a decimal value into a hours, minutes, and seconds.
    /// This can also be used to convert an angle from decimal degrees to integer degrees,
    /// arcminutes, and arcseconds.
    /// The hours part will be an integer.
    /// The minutes part will be a sexagesimal digit, i.e. an integer in the range 0-59.
    /// The seconds part will be a decimal in the range 0-59.999...
    /// The minutes and seconds values will have the same sign as the whole number, or be
    /// zero.
    /// </summary>
    /// <param name="decimalHours">
    /// The decimal value to convert. This could be a time in hours or an angle in degrees.
    /// </param>
    /// <returns>
    /// A tuple containing values representing the hours (or degrees), minutes (or arcminutes),
    /// and seconds (or arcseconds).
    /// </returns>
    public static (long, sbyte, decimal) ToHoursMinutesSeconds(decimal decimalHours)
    {
        // Calculate the hours part.
        // This will throw an exception if the truncated value of n is outside the valid range for
        // long. We could make hours a BigInteger but that seems unnecessary at this stage.
        long hours = (long)decimal.Truncate(decimalHours);

        // Calculate the minutes part.
        decimal decimalMinutes = (decimalHours - hours) * BASE;
        sbyte minutes = (sbyte)decimal.Truncate(decimalMinutes);

        // Calculate the seconds part.
        decimal seconds = (decimalMinutes - minutes) * BASE;

        return (hours, minutes, seconds);
    }

    /// <summary>Convert a decimal value to integer, minutes, and seconds notation.</summary>
    /// <param name="decimalHours">
    /// The decimal value to convert. This could be a time in hours or an angle in degrees.
    /// </param>
    /// <param name="notation">The notation to use for the output string.</param>
    /// <returns>The argument as a string formatted using the specified notation.</returns>
    public static string ToString(decimal decimalHours,
        ESexagesimalNotation notation = ESexagesimalNotation.Angle)
    {
        // Handle negative values.
        if (decimalHours < 0)
        {
            return '-' + ToString(-decimalHours);
        }

        // Convert decimal value to hours, minutes, and seconds.
        (long hours, sbyte minutes, decimal seconds) = ToHoursMinutesSeconds(decimalHours);

        // Format the output.
        return notation switch
        {
            ESexagesimalNotation.Angle => $"{hours}°{minutes}′{seconds}″",
            ESexagesimalNotation.Colons => $"{hours}:{minutes}:{seconds}",
            ESexagesimalNotation.TimeUnits => $"{hours}h {minutes}m {seconds}s",
            ESexagesimalNotation.Neugebauer => $"{hours};{minutes},{seconds}",
            _ => throw new ArgumentOutOfRangeException(nameof(notation), "Invalid notation.")
        };
    }
}
