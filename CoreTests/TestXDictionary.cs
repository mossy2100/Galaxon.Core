using Galaxon.Core.Collections;
using Galaxon.Core.Exceptions;

namespace Galaxon.Core.Tests;

[TestClass]
public class TestXDictionary
{
    [TestMethod]
    public void TestFlip()
    {
        Dictionary<int, string> dict = new ()
        {
            { 1, "One" },
            { 2, "Two" },
            { 3, "Three" }
        };
        Dictionary<string, int> flipped = dict.Flip();

        Assert.AreEqual(1, flipped["One"]);
        Assert.AreEqual(2, flipped["Two"]);
        Assert.AreEqual(3, flipped["Three"]);
    }

    [TestMethod]
    public void TestFlipFail()
    {
        Assert.ThrowsException<ArgumentInvalidException>(() =>
        {
            Dictionary<int, string> dict = new ()
            {
                { 1, "cat" },
                { 2, "cat" },
                { 3, "dog" }
            };
            Dictionary<string, int> flipped = dict.Flip();
        });
    }
}
