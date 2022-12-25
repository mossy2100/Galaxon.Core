using Galaxon.Core.Time;
using static System.DateTimeKind;

namespace Galaxon.Core.Tests;

[TestClass]
public class TestXDateTime
{
    private const double _Delta = 1e-9;

    [TestMethod]
    public void TestDateTimeGetTotalDays()
    {
        DateTime dt;

        // Test start of range.
        dt = new DateTime(1, 1, 1, 0, 0, 0, 0, Utc);
        Assert.AreEqual(dt.GetTotalDays(), 0);

        // Test current date.
        dt = new DateTime(2022, 6, 8, 5, 50, 24, 0, Utc);
        Assert.AreEqual(dt.GetTotalDays(), 738313.243333333, _Delta);

        // Test middle of range.
        dt = new DateTime(5000, 7, 2, 12, 30, 0, 0, Utc);
        Assert.AreEqual(dt.GetTotalDays(), 1826029.520833333, _Delta);

        // Test end of range.
        dt = new DateTime(9999, 12, 31, 23, 59, 59, 999, Utc);
        Assert.AreEqual(dt.GetTotalDays(), 3652058.999999988, _Delta);
    }

    [TestMethod]
    public void TestDateTimeFromTotalDays()
    {
        DateTime dt1, dt2;
        const long epsilon = 1000;

        dt1 = new DateTime(1, 1, 1, 0, 0, 0, 0, Utc);
        dt2 = XDateTime.FromTotalDays(0);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks);

        dt1 = new DateTime(2022, 6, 8, 5, 50, 24, 0, Utc);
        dt2 = XDateTime.FromTotalDays(738313.24333333333333);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks, epsilon);

        dt1 = new DateTime(5000, 7, 2, 12, 30, 0, 0, Utc);
        dt2 = XDateTime.FromTotalDays(1826029.52083333333333);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks, epsilon);

        dt1 = new DateTime(9999, 12, 31, 23, 59, 59, 999, Utc);
        dt2 = XDateTime.FromTotalDays(3652058.99999998842593);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks, epsilon);
    }

    // TODO Include a reference to the source of the test data.
    [TestMethod]
    public void TestDateTimeToJulianDay()
    {
        DateTime dt;

        // Test start of range.
        dt = new DateTime(1, 1, 1, 0, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDay(), 1721425.5);

        // Test current date.
        dt = new DateTime(2022, 6, 7, 0, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDay(), 2459737.5);

        // Test middle of range.
        dt = new DateTime(5000, 7, 2, 0, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDay(), 3547454.5);

        // Test end of range.
        dt = new DateTime(9999, 12, 31, 0, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDay(), 5373483.5);

        // Test values from Meeus p62.
        dt = new DateTime(2000, 1, 1, 12, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDay(), 2451545.0);
        dt = new DateTime(1999, 1, 1, 0, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDay(), 2451179.5);
        dt = new DateTime(1987, 1, 27, 0, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDay(), 2446822.5);
        dt = new DateTime(1987, 6, 19, 12, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDay(), 2446966.0);
        dt = new DateTime(1988, 1, 27, 0, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDay(), 2447187.5);
        dt = new DateTime(1988, 6, 19, 12, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDay(), 2447332.0);
        dt = new DateTime(1900, 1, 1, 0, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDay(), 2415020.5);
        dt = new DateTime(1600, 1, 1, 0, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDay(), 2305447.5);
        dt = new DateTime(1600, 12, 31, 0, 0, 0, 0, Utc);
        Assert.AreEqual(dt.ToJulianDay(), 2305812.5);
    }

    // TODO Include a reference to the source of the test data.
    [TestMethod]
    public void TestDateTimeFromJulianDay()
    {
        DateTime dt1, dt2;

        dt1 = new DateTime(1, 1, 1, 0, 0, 0, 0, Utc);
        dt2 = XDateTime.FromJulianDay(1721425.5);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks);

        dt1 = new DateTime(2022, 6, 7, 0, 0, 0, 0, Utc);
        dt2 = XDateTime.FromJulianDay(2459737.5);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks);

        dt1 = new DateTime(5000, 7, 2, 0, 0, 0, 0, Utc);
        dt2 = XDateTime.FromJulianDay(3547454.5);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks);

        dt1 = new DateTime(9999, 12, 31, 0, 0, 0, 0, Utc);
        dt2 = XDateTime.FromJulianDay(5373483.5);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks);
    }
}
