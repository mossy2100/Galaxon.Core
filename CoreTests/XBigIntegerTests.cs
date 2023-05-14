using System.Numerics;
using Galaxon.Core.Numbers;

namespace Galaxon.Core.Tests;

[TestClass]
public class XBigIntegerTests
{
    [TestMethod]
    public void Reverse_ReturnsCorrectValue()
    {
        // Arrange
        BigInteger n = 123456789;

        // Act
        var result = n.Reverse();

        // Assert
        Assert.AreEqual(987654321, result);
    }

    [TestMethod]
    public void IsPalindromic_ReturnsTrueForPalindromicNumber()
    {
        // Arrange
        BigInteger n = 123454321;

        // Act
        var result = n.IsPalindromic();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsPalindromic_ReturnsFalseForNonPalindromicNumber()
    {
        // Arrange
        BigInteger n = 123456789;

        // Act
        var result = n.IsPalindromic();

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void DigitSum_ReturnsCorrectValue()
    {
        // Arrange
        BigInteger n = 123456789;

        // Act
        var result = n.DigitSum();

        // Assert
        Assert.AreEqual(45, result);
    }

    [TestMethod]
    public void NumDigits_ReturnsCorrectValue()
    {
        // Arrange
        BigInteger n = 123456789;

        // Act
        var result = n.NumDigits();

        // Assert
        Assert.AreEqual(9, result);
    }

    [TestMethod]
    public void ToUnsigned_ReturnsCorrectValueForPositiveNumber()
    {
        // Arrange
        BigInteger n = 123456789;

        // Act
        var result = n.ToUnsigned();

        // Assert
        Assert.AreEqual(123456789, result);
    }

    [TestMethod]
    public void ToUnsigned_ReturnsCorrectValueForNegativeNumber()
    {
        // Arrange
        BigInteger n = -123456789;

        // Act
        var result = n.ToUnsigned();

        // Assert
        Assert.AreEqual(306039647, result);
    }

    [TestMethod]
    public void LeastCommonMultiple_ReturnsCorrectValue()
    {
        // Arrange
        BigInteger a = 4;
        BigInteger b = 6;

        // Act
        var result = XBigInteger.LeastCommonMultiple(a, b);

        // Assert
        Assert.AreEqual(12, result);
    }

    [TestMethod]
    public void GreatestCommonDivisor_ReturnsCorrectValue()
    {
        // Arrange
        BigInteger a = 123456789;
        BigInteger b = 987654321;

        // Act
        var result = XBigInteger.GreatestCommonDivisor(a, b);

        // Assert
        Assert.AreEqual(9, result);
    }

    [TestMethod]
    public void Sum_ReturnsCorrectValue()
    {
        // Arrange
        List<BigInteger> nums = new () { 1, 2, 3, 4, 5 };

        // Act
        var result = nums.Sum();

        // Assert
        Assert.AreEqual(15, result);
    }

    [TestMethod]
    public void Sum_WithTransform_ReturnsCorrectValue()
    {
        // Arrange
        List<BigInteger> nums = new () { 1, 2, 3, 4, 5 };

        // Act
        var result = nums.Sum(n => n * n);

        // Assert
        Assert.AreEqual(55, result);
    }
}
