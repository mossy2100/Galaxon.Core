using Galaxon.Core.Numbers;
using Galaxon.Core.Strings;

namespace Galaxon.Core.Tests;

[TestClass]
public class TestSuperscriptAndSubscript
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

    [TestMethod]
    public void TestStringToSuperscript()
    {
        string s1;
        string s2;

        s1 = "x2";
        s2 = s1.ToSuperscript();
        Assert.AreEqual("x²", s2);

        s1 = "m/s2";
        s2 = s1.ToSuperscript();
        Assert.AreEqual("m/s²", s2);

        s1 = "23";
        s2 = "6.02 * 10" + s1.ToSuperscript();
        Assert.AreEqual("6.02 * 10²³", s2);
    }

    [TestMethod]
    public void TestStringToSubscript()
    {
        string s1;
        string s2;

        s1 = "CH4";
        s2 = s1.ToSubscript();
        Assert.AreEqual("CH₄", s2);

        s1 = "CH3OH";
        s2 = s1.ToSubscript();
        Assert.AreEqual("CH₃OH", s2);

        s1 = "v0";
        s2 = s1.ToSubscript();
        Assert.AreEqual("v₀", s2);
    }
}
