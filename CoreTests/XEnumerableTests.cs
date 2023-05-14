using Galaxon.Core.Collections;

namespace Galaxon.Core.Tests;

[TestClass]
public class XEnumerableTests
{
    [TestMethod]
    public void Diff_RemovesValuesFromList2FromList1()
    {
        // Arrange
        List<int> list1 = new () { 1, 2, 3, 4, 5 };
        List<int> list2 = new () { 2, 4 };

        // Act
        var result = list1.Diff(list2);

        // Assert
        CollectionAssert.AreEqual(new List<int> { 1, 3, 5 }, result.ToList());
    }

    [TestMethod]
    public void Diff_SupportsDuplicates()
    {
        // Arrange
        List<string> list1 = new () { "cat", "dog", "cat", "bird" };
        List<string> list2 = new () { "cat", "bird" };

        // Act
        var result = list1.Diff(list2);

        // Assert
        CollectionAssert.AreEqual(new List<string> { "dog", "cat" }, result.ToList());
    }

    [TestMethod]
    public void ToDictionary_ConvertsIEnumerableToDictionaryWithIndexAsKey()
    {
        // Arrange
        List<string> enumerable = new () { "apple", "banana", "cherry" };

        // Act
        var result = enumerable.ToDictionary();

        // Assert
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual("apple", result[0]);
        Assert.AreEqual("banana", result[1]);
        Assert.AreEqual("cherry", result[2]);
    }
}
