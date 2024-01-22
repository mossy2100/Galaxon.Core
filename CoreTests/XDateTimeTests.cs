using Galaxon.Core.Time;
using static System.DateTimeKind;

namespace Galaxon.Core.Tests;

[TestClass]
public class XDateTimeTests
{
    private const double _DELTA = 1e-9;

    [TestMethod]
    public void TestDateTimeGetTotalDays()
    {
        DateTime dt;

        // Test start of range.
        dt = new DateTime(1, 1, 1, 0, 0, 0, 0, Utc);
        Assert.AreEqual(dt.GetTotalDays(), 0);

        // Test current date.
        dt = new DateTime(2022, 6, 8, 5, 50, 24, 0, Utc);
        Assert.AreEqual(dt.GetTotalDays(), 738313.243333333, _DELTA);

        // Test middle of range.
        dt = new DateTime(5000, 7, 2, 12, 30, 0, 0, Utc);
        Assert.AreEqual(dt.GetTotalDays(), 1826029.520833333, _DELTA);

        // Test end of range.
        dt = new DateTime(9999, 12, 31, 23, 59, 59, 999, Utc);
        Assert.AreEqual(dt.GetTotalDays(), 3652058.999999988, _DELTA);
    }

    [TestMethod]
    public void TestDateTimeFromTotalDays()
    {
        DateTime dt1, dt2;
        const long EPSILON = 1000;

        dt1 = new DateTime(1, 1, 1, 0, 0, 0, 0, Utc);
        dt2 = XDateTime.FromTotalDays(0);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks);

        dt1 = new DateTime(2022, 6, 8, 5, 50, 24, 0, Utc);
        dt2 = XDateTime.FromTotalDays(738313.24333333333333);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks, EPSILON);

        dt1 = new DateTime(5000, 7, 2, 12, 30, 0, 0, Utc);
        dt2 = XDateTime.FromTotalDays(1826029.52083333333333);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks, EPSILON);

        dt1 = new DateTime(9999, 12, 31, 23, 59, 59, 999, Utc);
        dt2 = XDateTime.FromTotalDays(3652058.99999998842593);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks, EPSILON);
    }

    // TODO Include a reference to the source of the test data.
    [TestMethod]
    public void TestDateTimeToJulianDay()
    {
        DateTime dt;

        // Test start of range.
        dt = new DateTime(1, 1, 1, 0, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDate(), 1721425.5);

        // Test current date.
        dt = new DateTime(2022, 6, 7, 0, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDate(), 2459737.5);

        // Test middle of range.
        dt = new DateTime(5000, 7, 2, 0, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDate(), 3547454.5);

        // Test end of range.
        dt = new DateTime(9999, 12, 31, 0, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDate(), 5373483.5);

        // Test values from Meeus p62.
        dt = new DateTime(2000, 1, 1, 12, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDate(), 2451545.0);
        dt = new DateTime(1999, 1, 1, 0, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDate(), 2451179.5);
        dt = new DateTime(1987, 1, 27, 0, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDate(), 2446822.5);
        dt = new DateTime(1987, 6, 19, 12, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDate(), 2446966.0);
        dt = new DateTime(1988, 1, 27, 0, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDate(), 2447187.5);
        dt = new DateTime(1988, 6, 19, 12, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDate(), 2447332.0);
        dt = new DateTime(1900, 1, 1, 0, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDate(), 2415020.5);
        dt = new DateTime(1600, 1, 1, 0, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDate(), 2305447.5);
        dt = new DateTime(1600, 12, 31, 0, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDate(), 2305812.5);
    }

    // TODO Include a reference to the source of the test data.
    [TestMethod]
    public void TestDateTimeFromJulianDay()
    {
        DateTime dt1, dt2;

        dt1 = new DateTime(1, 1, 1, 0, 0, 0, 0, Utc);
        dt2 = XDateTime.FromJulianDate(1721425.5);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks);

        dt1 = new DateTime(2022, 6, 7, 0, 0, 0, 0, Utc);
        dt2 = XDateTime.FromJulianDate(2459737.5);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks);

        dt1 = new DateTime(5000, 7, 2, 0, 0, 0, 0, Utc);
        dt2 = XDateTime.FromJulianDate(3547454.5);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks);

        dt1 = new DateTime(9999, 12, 31, 0, 0, 0, 0, Utc);
        dt2 = XDateTime.FromJulianDate(5373483.5);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks);
    }
}
