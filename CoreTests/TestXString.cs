using System.Diagnostics;
using Galaxon.Core.Strings;

namespace Galaxon.Core.Tests;

[TestClass]
public class TestXString
{
    [TestMethod]
    public void TestMakeSlug()
    {
        string s1;
        string s2;

        // English.
        s1 = "This is a normal, everyday title, written in English.";
        s2 = s1.MakeSlug();
        Trace.WriteLine(s2);
        Assert.AreEqual("this-is-a-normal-everyday-title-written-in-english", s2);

        // Maths.
        s1 = "Einstein's famous equation is e=mc²";
        s2 = s1.MakeSlug();
        Trace.WriteLine(s2);
        Assert.AreEqual("einsteins-famous-equation-is-e-mc2", s2);

        // Spanish.
        // Tests removal of diacritics but also trimming non-alphanumeric characters from the ends
        // of the string.
        s1 = "¡Hola! ¿Cómo estás hoy?";
        s2 = s1.MakeSlug();
        Trace.WriteLine(s2);
        Assert.AreEqual("hola-como-estas-hoy", s2);

        // German.
        s1 = "Der Kurfürstendamm ist eine berühmte Straße in Berlin.";
        s2 = s1.MakeSlug();
        Trace.WriteLine(s2);
        Assert.AreEqual("der-kurfurstendamm-ist-eine-beruhmte-strasse-in-berlin", s2);

        // Polish.
        s1 = "10 rzeczy, które warto wiedzieć o historii Polski.";
        s2 = s1.MakeSlug();
        Trace.WriteLine(s2);
        Assert.AreEqual("10-rzeczy-ktore-warto-wiedziec-o-historii-polski", s2);

        // Japanese. "Hello, and welcome to Tokyo!"
        s1 = "こんにちは、東京へようこそ！";
        s2 = s1.MakeSlug();
        Trace.WriteLine(s2);
        Assert.AreEqual("konnichiha-dongjingheyoukoso", s2);
        // The transliteration is not great for Asian languages.
        // It works better if you Romanize the text first using Google translate, e.g.
        s1 = "Kon'nichiwa, Tōkyō e yōkoso!";
        s2 = s1.MakeSlug();
        Trace.WriteLine(s2);
        Assert.AreEqual("konnichiwa-tokyo-e-yokoso", s2);

        // Chinese. "We hope you enjoy your stay in Shanghai."
        s1 = "我们希望您在上海过得愉快。";
        s2 = s1.MakeSlug();
        Trace.WriteLine(s2);
        Assert.AreEqual("womenxiwangninzaishanghaiguodeyukuai", s2);
        // With initial manual Romanization.
        s1 = "Wǒmen xīwàng nín zài shànghǎiguò dé yúkuài.";
        s2 = s1.MakeSlug();
        Trace.WriteLine(s2);
        Assert.AreEqual("women-xiwang-nin-zai-shanghaiguo-de-yukuai", s2);
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

    [TestMethod]
    public void TestStringToSmallCaps()
    {
        string s1 = "A quick brown fox jumps over the lazy dog.";
        string s2 = s1.ToSmallCaps();
        Assert.AreEqual("A ꞯᴜɪᴄᴋ ʙʀᴏᴡɴ ꜰᴏx ᴊᴜᴍᴘꜱ ᴏᴠᴇʀ ᴛʜᴇ ʟᴀᴢʏ ᴅᴏɢ.", s2);

        // Article title. Example with numbers and symbols.
        s1 = "10 Tips To Become A Good Programmer - C# Corner";
        s2 = s1.ToSmallCaps();
        Assert.AreEqual("10 Tɪᴘꜱ Tᴏ Bᴇᴄᴏᴍᴇ A Gᴏᴏᴅ Pʀᴏɢʀᴀᴍᴍᴇʀ - C# Cᴏʀɴᴇʀ", s2);
    }
}
