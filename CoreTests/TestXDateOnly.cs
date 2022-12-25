using Galaxon.Core.Time;

namespace Galaxon.Core.Tests;

[TestClass]
public class TestXDateOnly
{
    [TestMethod]
    public void TestDateOnlyGetTotalDays()
    {
        DateOnly date;

        // Test start of range.
        date = new DateOnly(1, 1, 1);
        Assert.AreEqual(date.GetTotalDays(), 0);

        // Test current date.
        date = new DateOnly(2022, 6, 8);
        Assert.AreEqual(date.GetTotalDays(), 738313);

        // Test middle of range.
        date = new DateOnly(5000, 7, 2);
        Assert.AreEqual(date.GetTotalDays(), 1826029);

        // Test end of range.
        date = new DateOnly(9999, 12, 31);
        Assert.AreEqual(date.GetTotalDays(), 3652058);
    }

    [TestMethod]
    public void TestDateOnlyFromTotalDays()
    {
        DateOnly date1, date2;

        date1 = new DateOnly(1, 1, 1);
        date2 = XDateOnly.FromTotalDays(0);
        Assert.AreEqual(date1.GetTicks(), date2.GetTicks());

        date1 = new DateOnly(2022, 6, 8);
        date2 = XDateOnly.FromTotalDays(738313);
        Assert.AreEqual(date1.GetTicks(), date2.GetTicks());

        date1 = new DateOnly(5000, 7, 2);
        date2 = XDateOnly.FromTotalDays(1826029);
        Assert.AreEqual(date1.GetTicks(), date2.GetTicks());

        date1 = new DateOnly(9999, 12, 31);
        date2 = XDateOnly.FromTotalDays(3652058);
        Assert.AreEqual(date1.GetTicks(), date2.GetTicks());
    }

    // TODO Include a reference to the source of the test data.
    [TestMethod]
    public void TestDateOnlyToJulianDay()
    {
        DateOnly date;

        // Test start of range.
        date = new DateOnly(1, 1, 1);
        Assert.AreEqual(date.ToJulianDay(), 1721425.5);

        // Test current date.
        date = new DateOnly(2022, 6, 8);
        Assert.AreEqual(date.ToJulianDay(), 2459738.5);

        // Test middle of range.
        date = new DateOnly(5000, 7, 2);
        Assert.AreEqual(date.ToJulianDay(), 3547454.5);

        // Test end of range.
        date = new DateOnly(9999, 12, 31);
        Assert.AreEqual(date.ToJulianDay(), 5373483.5);
    }

    // TODO Include a reference to the source of the test data.
    [TestMethod]
    public void TestDateOnlyFromJulianDay()
    {
        DateOnly date1, date2;

        // Test start of range.
        date1 = new DateOnly(1, 1, 1);
        date2 = XDateOnly.FromJulianDay(1721425.5);
        Assert.AreEqual(date1.GetTicks(), date2.GetTicks());

        // Test current date.
        date1 = new DateOnly(2022, 6, 8);
        date2 = XDateOnly.FromJulianDay(2459738.5);
        Assert.AreEqual(date1.GetTicks(), date2.GetTicks());

        // Test middle of range.
        date1 = new DateOnly(5000, 7, 2);
        date2 = XDateOnly.FromJulianDay(3547454.5);
        Assert.AreEqual(date1.GetTicks(), date2.GetTicks());

        // Test end of range.
        date1 = new DateOnly(9999, 12, 31);
        date2 = XDateOnly.FromJulianDay(5373483.5);
        Assert.AreEqual(date1.GetTicks(), date2.GetTicks());
    }

    [TestMethod]
    public void TestSubtract()
    {
        // This uses the dates of Halley's Comet from example 7.d in AA2 p64.
        DateOnly date1 = new (1910, 4, 20);
        DateOnly date2 = new (1986, 2, 9);
        Assert.AreEqual(date2.Subtract(date1).Days, 27689);
    }

    [TestMethod]
    public void TestAdd()
    {
        // From the exercise in AA2 p64.
        DateOnly date1 = new (1991, 7, 11);
        DateOnly date2 = date1.AddDays(10000);
        Assert.AreEqual(date2.ToIsoString(), "2018-11-26");
    }

    [TestMethod]
    // Also tests ToIsoString().
    public void TestFromDayOfYear()
    {
        DateOnly date1, date2;
        int n1, n2;

        // Test start of range.
        date1 = new DateOnly(1, 1, 1);
        n1 = date1.DayOfYear;
        date2 = XDateOnly.FromDayOfYear(1, n1);
        Assert.AreEqual(date2.ToIsoString(), "0001-01-01");
        n2 = date2.DayOfYear;
        Assert.AreEqual(n1, n2);

        // Test leap year.
        date1 = new DateOnly(2020, 12, 31);
        n1 = date1.DayOfYear;
        date2 = XDateOnly.FromDayOfYear(2020, n1);
        Assert.AreEqual(date2.ToIsoString(), "2020-12-31");
        n2 = date2.DayOfYear;
        Assert.AreEqual(n1, n2);

        // Test current date.
        date1 = new DateOnly(2022, 6, 8);
        n1 = date1.DayOfYear;
        date2 = XDateOnly.FromDayOfYear(2022, n1);
        Assert.AreEqual(date2.ToIsoString(), "2022-06-08");
        n2 = date2.DayOfYear;
        Assert.AreEqual(n1, n2);

        // Test middle of range.
        date1 = new DateOnly(5000, 7, 2);
        n1 = date1.DayOfYear;
        date2 = XDateOnly.FromDayOfYear(5000, n1);
        Assert.AreEqual(date2.ToIsoString(), "5000-07-02");
        n2 = date2.DayOfYear;
        Assert.AreEqual(n1, n2);

        // Test end of range.
        date1 = new DateOnly(9999, 12, 31);
        n1 = date1.DayOfYear;
        date2 = XDateOnly.FromDayOfYear(9999, n1);
        Assert.AreEqual(date2.ToIsoString(), "9999-12-31");
        n2 = date2.DayOfYear;
        Assert.AreEqual(n1, n2);
    }

    /// <summary>
    /// These examples are from Meeus AA2 p67.
    /// I've also verified the dates at:
    /// <see href="https://www.assa.org.au/edm" />
    /// </summary>
    [TestMethod]
    public static void TestEaster()
    {
        DateOnly date;

        date = XDateOnly.Easter(1991);
        Assert.AreEqual(date.ToIsoString(), "1991-03-31");

        date = XDateOnly.Easter(1992);
        Assert.AreEqual(date.ToIsoString(), "1992-04-19");

        date = XDateOnly.Easter(1993);
        Assert.AreEqual(date.ToIsoString(), "1993-04-11");

        date = XDateOnly.Easter(1954);
        Assert.AreEqual(date.ToIsoString(), "1954-04-18");

        date = XDateOnly.Easter(2000);
        Assert.AreEqual(date.ToIsoString(), "2000-04-23");

        date = XDateOnly.Easter(1818);
        Assert.AreEqual(date.ToIsoString(), "1818-03-22");

        date = XDateOnly.Easter(2285);
        Assert.AreEqual(date.ToIsoString(), "2285-03-22");

        date = XDateOnly.Easter(1886);
        Assert.AreEqual(date.ToIsoString(), "1886-04-25");

        date = XDateOnly.Easter(1943);
        Assert.AreEqual(date.ToIsoString(), "1943-04-25");

        date = XDateOnly.Easter(2038);
        Assert.AreEqual(date.ToIsoString(), "2038-04-25");
    }

    /// <summary>
    /// Tests the Easter dates from the database (1600-2299).
    /// TODO Update to use a data file.
    /// </summary>
    // [TestMethod]
    public static void TestEaster2()
    {
    }
}
