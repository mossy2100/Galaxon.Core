using Galaxon.Core.Strings;

namespace Galaxon.Core.Tests;

[TestClass]
public class XStringTests
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

    [TestMethod]
    public void ToProperReturnsEmptyStringGivenEmptyString()
    {
        string expected = "";
        string actual = expected.ToProper();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToProperReturnsWordWithFirstLetterUpperCase()
    {
        string source = "cat";
        string expected = "Cat";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToProperMakesCorrectLettersUpperCase()
    {
        string source = "here is a simple title, all lower-case";
        string expected = "Here Is A Simple Title, All Lower-Case";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToProperMakesCorrectLettersLowerCase()
    {
        string source = "HERE IS A SIMPLE TITLE, ALL UPPER-CASE";
        string expected = "Here Is A Simple Title, All Upper-Case";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// This is an example of how the method is actually broken. It doesn't (yet) know when to
    /// capitalise the letter after an apostrophe.
    /// Words like "can't" should be "Can't" in proper case, but words like "O'Henry" should stay
    /// like that.
    /// Ideally it would know the difference, and work with single quotes as well as true apostrophe
    /// characters.
    /// </summary>
    [TestMethod]
    public void ToProperReturnsContractionsWithEachPartProperCase()
    {
        string source = "I can't believe it's not butter!";
        string expected = "I Can'T Believe It'S Not Butter!";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }
}
