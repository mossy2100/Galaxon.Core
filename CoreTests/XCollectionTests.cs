using System.Collections;
using Galaxon.Core.Collections;

namespace Galaxon.Core.Tests;

[TestClass]
public class XCollectionTests
{
    [TestMethod]
    public void IsEmpty_NullCollection_ReturnsTrue()
    {
        // Arrange
        ICollection? collection = null;

        // Act
        var result = collection.IsEmpty();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsEmpty_EmptyCollection_ReturnsTrue()
    {
        // Arrange
        ICollection collection = new List<int>();

        // Act
        var result = collection.IsEmpty();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsEmpty_NonEmptyCollection_ReturnsFalse()
    {
        // Arrange
        ICollection collection = new List<int> { 1, 2, 3 };

        // Act
        var result = collection.IsEmpty();

        // Assert
        Assert.IsFalse(result);
    }
}
