using Galaxon.Core.Strings;

namespace Galaxon.Core.Tests;

[TestClass]
public class TestXString
{
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

    [TestMethod]
    public void TestStringToSmallCaps()
    {
        var s1 = "A quick brown fox jumps over the lazy dog.";
        var s2 = s1.ToSmallCaps();
        Assert.AreEqual("A ꞯᴜɪᴄᴋ ʙʀᴏᴡɴ ꜰᴏx ᴊᴜᴍᴘꜱ ᴏᴠᴇʀ ᴛʜᴇ ʟᴀᴢʏ ᴅᴏɢ.", s2);

        // Article title. Example with numbers and symbols.
        s1 = "10 Tips To Become A Good Programmer - C# Corner";
        s2 = s1.ToSmallCaps();
        Assert.AreEqual("10 Tɪᴘꜱ Tᴏ Bᴇᴄᴏᴍᴇ A Gᴏᴏᴅ Pʀᴏɢʀᴀᴍᴍᴇʀ - C# Cᴏʀɴᴇʀ", s2);
    }
}
