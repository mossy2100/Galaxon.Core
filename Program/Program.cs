using System.Text;
using Galaxon.Core.Strings;

Console.WriteLine("Testing...");

// ProcessText.ProcessLigaturesHtml();

var ligMap = Transliterate.AsciiSubstitutionMap.OrderBy(kvp => kvp.Value).ThenBy(kvp => kvp.Key);

StringBuilder sb = new ();
foreach ((char c, string str) in ligMap)
{
    if (!str.IsAscii())
    {
        Console.WriteLine("Not ascii!");
    }

    // string line = $"{{ '{c}', \"{str}\" }},".PadRight(17)
    //     + $"// \\u{((ushort)c).ToHex(true).ZeroPad(4)}";
    // Console.WriteLine(line);
    // sb.AppendLine(line);
}

File.WriteAllText(
    "/Users/shaun/Documents/Web & software development/C#/Projects/Galaxon/Core/Program/CombinedMap.txt",
    sb.ToString());
