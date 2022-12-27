using System.Diagnostics;
using Galaxon.Core.Numbers;

namespace Galaxon.Core.Tests;

[TestClass]
public class TestXUlong
{
    [TestMethod]
    public void TestHexString()
    {
        ulong x = 123456ul;
        string hex = x.ToHexString();
        Trace.WriteLine(hex);
        ulong x2 = XUlong.FromHexString(hex);
        Assert.AreEqual(x, x2);
    }

    [TestMethod]
    public void TestBinString()
    {
        ulong x = 123456ul;
        string bin = x.ToBinString();
        Trace.WriteLine(bin);
        ulong x2 = XUlong.FromBinString(bin);
        Assert.AreEqual(x, x2);
    }
}
