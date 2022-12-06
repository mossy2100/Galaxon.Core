using AstroMultimedia.Core.Numbers;

namespace AstroMultimedia.Core.Tests;

[TestClass]
public class TestXInt
{
    [TestMethod]
    public void TestSqrt()
    {
        for (int i = 1; i <= 10; i++)
        {
            Assert.AreEqual(i, XInt32.Sqrt(i * i));
        }
    }

    [TestMethod]
    public void TestSqrt2()
    {
        Assert.AreEqual(1, XInt32.Sqrt(1));
        Assert.AreEqual(1, XInt32.Sqrt(2));
        Assert.AreEqual(2, XInt32.Sqrt(3));
        Assert.AreEqual(2, XInt32.Sqrt(4));
        Assert.AreEqual(2, XInt32.Sqrt(5));
        Assert.AreEqual(2, XInt32.Sqrt(6));
        Assert.AreEqual(3, XInt32.Sqrt(7));
        Assert.AreEqual(3, XInt32.Sqrt(8));
        Assert.AreEqual(3, XInt32.Sqrt(9));
        Assert.AreEqual(3, XInt32.Sqrt(10));
    }
}
