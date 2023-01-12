using System.Numerics;
using Galaxon.Core.Numbers;

namespace Galaxon.Core.Tests;

[TestClass]
public class TestXBigInteger
{
    [TestMethod]
    public void TestToUnsignedZero()
    {
        BigInteger bi = 0;
        BigInteger expected = 0;
        BigInteger actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestToUnsignedPositive()
    {
        BigInteger bi;
        BigInteger expected;
        BigInteger actual;

        bi = 1;
        expected = 1;
        actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);

        bi = 123;
        expected = 123;
        actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);

        bi = sbyte.MaxValue;
        expected = sbyte.MaxValue;
        actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);

        bi = byte.MaxValue;
        expected = byte.MaxValue;
        actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);

        bi = int.MaxValue;
        expected = int.MaxValue;
        actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);

        bi = uint.MaxValue;
        expected = uint.MaxValue;
        actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestToUnsignedNegative()
    {
        BigInteger bi;
        BigInteger expected;
        BigInteger actual;

        bi = -1;
        expected = 255;
        actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);

        bi = -128;
        expected = 128;
        actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);

        bi = -100;
        expected = bi + byte.MaxValue + 1;
        actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);

        bi = -10000;
        expected = bi + ushort.MaxValue + 1;
        actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);

        bi = -12345678;
        expected = bi + uint.MaxValue + 1;
        actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestNumDigitsZero()
    {
        BigInteger bi;
        int nDigits;

        bi = 0;
        nDigits = bi.NumDigits();
        Assert.AreEqual(1, nDigits);
    }

    [TestMethod]
    public void TestNumDigitsPositive()
    {
        BigInteger bi;
        int nDigits;

        bi = 1;
        nDigits = bi.NumDigits();
        Assert.AreEqual(1, nDigits);

        bi = 9;
        nDigits = bi.NumDigits();
        Assert.AreEqual(1, nDigits);

        bi = 10;
        nDigits = bi.NumDigits();
        Assert.AreEqual(2, nDigits);

        bi = 999;
        nDigits = bi.NumDigits();
        Assert.AreEqual(3, nDigits);

        bi = 1000;
        nDigits = bi.NumDigits();
        Assert.AreEqual(4, nDigits);

        bi = 999_999_999;
        nDigits = bi.NumDigits();
        Assert.AreEqual(9, nDigits);

        bi = 1_000_000_000;
        nDigits = bi.NumDigits();
        Assert.AreEqual(10, nDigits);

        bi = BigInteger.Parse("99999999999999999999");
        nDigits = bi.NumDigits();
        Assert.AreEqual(20, nDigits);
    }

    [TestMethod]
    public void TestNumDigitsNegative()
    {
        BigInteger bi;
        int nDigits;

        bi = -1;
        nDigits = bi.NumDigits();
        Assert.AreEqual(1, nDigits);

        bi = -9;
        nDigits = bi.NumDigits();
        Assert.AreEqual(1, nDigits);

        bi = -10;
        nDigits = bi.NumDigits();
        Assert.AreEqual(2, nDigits);

        bi = -999;
        nDigits = bi.NumDigits();
        Assert.AreEqual(3, nDigits);

        bi = -1000;
        nDigits = bi.NumDigits();
        Assert.AreEqual(4, nDigits);

        bi = -999_999_999;
        nDigits = bi.NumDigits();
        Assert.AreEqual(9, nDigits);

        bi = -1_000_000_000;
        nDigits = bi.NumDigits();
        Assert.AreEqual(10, nDigits);

        bi = BigInteger.Parse("-99999999999999999999");
        nDigits = bi.NumDigits();
        Assert.AreEqual(20, nDigits);
    }
}
