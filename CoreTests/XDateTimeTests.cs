﻿using Galaxon.Core.Time;

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
        dt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        Assert.AreEqual(dt.GetTotalDays(), 0);

        // Test current date.
        dt = new DateTime(2022, 6, 8, 5, 50, 24, 0, DateTimeKind.Utc);
        Assert.AreEqual(dt.GetTotalDays(), 738313.243333333, _DELTA);

        // Test middle of range.
        dt = new DateTime(5000, 7, 2, 12, 30, 0, 0, DateTimeKind.Utc);
        Assert.AreEqual(dt.GetTotalDays(), 1826029.520833333, _DELTA);

        // Test end of range.
        dt = new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc);
        Assert.AreEqual(dt.GetTotalDays(), 3652058.999999988, _DELTA);
    }

    [TestMethod]
    public void TestDateTimeFromTotalDays()
    {
        DateTime dt1, dt2;
        const long EPSILON = 1000;

        dt1 = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dt2 = XDateTime.FromTotalDays(0);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks);

        dt1 = new DateTime(2022, 6, 8, 5, 50, 24, 0, DateTimeKind.Utc);
        dt2 = XDateTime.FromTotalDays(738313.24333333333333);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks, EPSILON);

        dt1 = new DateTime(5000, 7, 2, 12, 30, 0, 0, DateTimeKind.Utc);
        dt2 = XDateTime.FromTotalDays(1826029.52083333333333);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks, EPSILON);

        dt1 = new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc);
        dt2 = XDateTime.FromTotalDays(3652058.99999998842593);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks, EPSILON);
    }
}
