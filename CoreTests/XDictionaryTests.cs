using Galaxon.Core.Collections;
using Galaxon.Core.Exceptions;

namespace Galaxon.Core.Tests;

[TestClass]
public class XDictionaryTests
{
    [TestMethod]
    public void Flip_DictionaryWithUniqueValues_ReturnsFlippedDictionary()
    {
        // Arrange
        Dictionary<int, string> dict = new ()
        {
            { 1, "one" },
            { 2, "two" },
            { 3, "three" },
        };

        // Act
        var flippedDict = dict.Flip();

        // Assert
        Assert.AreEqual(1, flippedDict["one"]);
        Assert.AreEqual(2, flippedDict["two"]);
        Assert.AreEqual(3, flippedDict["three"]);
    }

    [TestMethod]
    public void Flip_DictionaryWithDuplicateValues_ThrowsArgumentInvalidException()
    {
        // Arrange
        Dictionary<int, string> dict = new ()
        {
            { 1, "one" },
            { 2, "two" },
            { 3, "one" },
        };

        // Act & Assert
        Assert.ThrowsException<ArgumentInvalidException>(() => dict.Flip());
    }
}
