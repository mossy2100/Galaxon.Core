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
    /// Test handling of ASCII apostrophe.
    /// </summary>
    [TestMethod]
    public void ToProperHandlesAsciiApostrophe()
    {
        string source = "I can't believe it's not Java!";
        string expected = "I Can't Believe It's Not Java!";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Test handling of Unicode apostrophe.
    /// </summary>
    [TestMethod]
    public void ToProperHandlesUnicodeApostrophe()
    {
        string source = "let’s cook bill’s and your friends’ meals.";
        string expected = "Let’s Cook Bill’s And Your Friends’ Meals.";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Test handling of words beginning with or preceded by apostrophes.
    /// </summary>
    [TestMethod]
    public void ToProperHandlesApostrophesAtStartOfWords()
    {
        string source = "'don’t worry ’bout a thing,' she said.";
        string expected = "'Don’t Worry ’Bout A Thing,' She Said.";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Show how the method doesn't properly handle proper nouns containing apostrophes.
    /// This could be fixed by detecting and preserving such words in the source string.
    /// </summary>
    [TestMethod]
    public void ToProperDoesNotCorrectlyHandleProperNounsWithApostrophes()
    {
        string source = "Seamus O'Henry loves pretending he's T'Challa.";
        string expected = "Seamus O'henry Loves Pretending He's T'challa.";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Show how the method doesn't properly handle camel case.
    /// This could be fixed by detecting and preserving camel-case words in the source string.
    /// </summary>
    [TestMethod]
    public void ToProperDoesNotCorrectlyHandleCamelCase()
    {
        string source = "The JavaScript method getElementById() can be useful.";
        string expected = "The Javascript Method Getelementbyid() Can Be Useful.";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Show how the method doesn't properly handle acronyms.
    /// This could be fixed by detecting and preserving acronyms in the source string.
    /// </summary>
    [TestMethod]
    public void ToProperDoesNotCorrectlyHandleAcronyms()
    {
        string source = "She works for NASA on UAVs and HLVs.";
        string expected = "She Works For Nasa On Uavs And Hlvs.";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }
}
