using AstroMultimedia.Core.Collections;

namespace AstroMultimedia.Core.Tests;

[TestClass]
public class TestXEnumerable
{
    [TestMethod]
    public void ConvertArrayOfIntsToDictionary()
    {
        int[] numbers = { 1, 6, 9, 12, 4, 19191 };
        Dictionary<int, int> result = numbers.ToDictionary();
        Assert.AreEqual(numbers.Length, result.Count);
        foreach ((int index, int item) in result)
        {
            Assert.AreEqual(numbers[index], item);
        }
    }

    [TestMethod]
    public void ConvertListOfStringsToDictionary()
    {
        List<string> strings = new() { "cat", "dog", "Fox", "$1.25", "Mossy rules" };
        Dictionary<int, string> result = strings.ToDictionary();
        Assert.AreEqual(strings.Count, result.Count);
        int i = 0;
        foreach (string str in strings)
        {
            Assert.AreEqual(str, result[i++]);
        }
    }
}
