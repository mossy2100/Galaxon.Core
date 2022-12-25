using Galaxon.Core.Numbers;

namespace Galaxon.Core.Testing;

/// <summary>
/// Container for my own custom Assert methods.
/// </summary>
public static class XAssert
{
    /// <summary>
    /// Helper function to compare DMS (degrees, minutes, seconds) tuples for
    /// equality.
    /// I may need to rethink the delta parameter (e.g. make it also a tuple so
    /// I can set different deltas for degrees, minutes, and seconds) but for
    /// now it's fine.
    /// </summary>
    /// <param name="A">Angle 1</param>
    /// <param name="B">Angle 2</param>
    /// <param name="delta">Maximum acceptable difference between value pairs.</param>
    public static void AreEqual((double d, double m, double s) A,
        (double d, double m, double s) B, double delta = 0)
    {
        Assert.AreEqual(A.d, B.d, delta);
        Assert.AreEqual(A.m, B.m, delta);
        Assert.AreEqual(A.s, B.s, delta);
    }

    /// <summary>
    /// Helper function to compare DateTimes for equality.
    /// </summary>
    /// <param name="dt1">The first DateTime</param>
    /// <param name="dt2">The second DateTime</param>
    /// <param name="delta">Maximum acceptable difference.</param>
    public static void AreEqual(DateTime dt1, DateTime dt2, TimeSpan? delta = null)
    {
        double deltaTicks = delta?.Ticks ?? 0;
        Assert.AreEqual(dt1.Ticks, dt2.Ticks, deltaTicks);
    }

    /// <summary>
    /// Check if a value is in a given range.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="lower">The lower value.</param>
    /// <param name="upper">The upper value.</param>
    /// <param name="includeLower">Include lower value in the range.</param>
    /// <param name="includeUpper">Include upper value in the range.</param>
    public static void IsInRange(double value, double lower, double upper,
        bool includeLower = true, bool includeUpper = false)
    {
        Assert.IsTrue(includeLower ? value >= lower : value > lower);
        Assert.IsTrue(includeUpper ? value <= upper : value < upper);
    }

    /// <summary>
    /// Compares 2 doubles to see if they are close enough to be considered equal.
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="delta">The max allowable difference.</param>
    public static void AreEqual(double expected, double actual, double delta = XDouble.Delta) =>
        Assert.AreEqual(expected, actual, delta);
        // Assert.IsTrue(expected.FuzzyEquals(actual, delta));

    /// <summary>
    /// Helper function to test if a double equals a decimal.
    /// </summary>
    /// <param name="expected">Expected double value</param>
    /// <param name="actual">Actual decimal value</param>
    public static void AreEqual(double expected, decimal actual)
    {
        // Doubles and decimals are only equal to a limited number of significant figures, so scale
        // larger values to the range [0..10) before comparing.
        Console.WriteLine($"Comparing {expected} with {actual}");
        decimal a = actual;
        decimal e = (decimal)expected;
        if (actual != 0)
        {
            int m = (int)Math.Floor(Math.Log10(Math.Abs(expected)));
            if (m > 0)
            {
                decimal scaleFactor = XDecimal.Exp10(m);
                a /= scaleFactor;
                e /= scaleFactor;
            }
        }

        // Compare decimals.
        Assert.AreEqual(e, a, 1e-13m);
    }

    public static void AreEqualPercent(double expected, double actual, double percent)
    {
        double delta1 = Math.Abs(expected * percent / 100);
        double delta2 = Math.Abs(actual * percent / 100);
        double delta = Math.Max(delta1, delta2);

        // If delta is 0 at this point then both values are very close to 0.
        // Let's call them equal.
        if (delta == 0)
        {
            return;
        }

        Assert.AreEqual(expected, actual, delta);
    }
}
