using Galaxon.Core.Numbers;

namespace Galaxon.Core.Tests;

[TestClass]
public class TestXBinaryInteger
{
    [TestMethod]
    public void TestIntToSuperscript()
    {
        int n;
        string s;

        n = 0;
        s = n.ToSuperscript();
        Assert.AreEqual("⁰", s);

        n = 12345;
        s = n.ToSuperscript();
        Assert.AreEqual("¹²³⁴⁵", s);

        n = -12345;
        s = n.ToSuperscript();
        Assert.AreEqual("⁻¹²³⁴⁵", s);
    }

    [TestMethod]
    public void TestIntToSubscript()
    {
        int n;
        string s;

        n = 0;
        s = n.ToSubscript();
        Assert.AreEqual("₀", s);

        n = 12345;
        s = n.ToSubscript();
        Assert.AreEqual("₁₂₃₄₅", s);

        n = -12345;
        s = n.ToSubscript();
        Assert.AreEqual("₋₁₂₃₄₅", s);
    }
}
