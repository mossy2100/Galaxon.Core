using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Galaxon.Core.Numbers;
using Galaxon.Core.Strings;

namespace Galaxon.Core.Program;

public class ProcessText
{
    public static void ProcessDiacriticsHtml()
    {
        string diacriticsHtml =
            File.ReadAllText(
                "/Users/shaun/Documents/Web & software development/C#/Projects/Galaxon/Core/Program/data/diacritics.html");

        Regex rx = new (@"<tr>\s*"
            + @"<td>(?<unicode>[0-9a-f]{4})</td>\s*"
            + @"<td>(?<htmlEntity>[^<]+)</td>\s*"
            + @"<td>(?<name>[^<]+)</td>\s*"
            + @"<td>(?<description>[^<]+)</td>\s*"
            + @"</tr>", RegexOptions.IgnoreCase);

        Regex upper = new ("capital (ligature )?(?<uppercaseLetters>[a-z]+)",
            RegexOptions.IgnoreCase);
        Regex lower = new ("(lowercase|small letter) (?<lowercaseLetters>[a-z]+)",
            RegexOptions.IgnoreCase);

        StringBuilder sbDiacriticMap = new ();
        StringBuilder sbLigatureMap = new ();

        MatchCollection matches = rx.Matches(diacriticsHtml);
        foreach (Match match in matches)
        {
            string unicode = match.Groups["unicode"].Value;
            char c = (char)ConvertBase.FromHex<ushort>(unicode);
            // string name = match.Groups["name"].Value;
            string description = match.Groups["description"].Value;

            // string item = unicode.PadRight(10) + c.ToString().PadRight(10) + name.PadRight(20) + description;
            // Console.WriteLine(item);

            // Fix the case in the description.
            string[] words = description.Split(' ');
            string[] words2 = new string[words.Length];
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length <= 2)
                {
                    words2[i] = words[i].ToUpper();
                }
                else
                {
                    words2[i] = words[i].ToLower();
                }
            }
            string description2 = string.Join(' ', words2);

            // Look for upper and lower case letters.
            string replacement = "";
            var upperMatch = upper.Match(description);
            if (upperMatch.Success)
            {
                string uppercaseLetters = upperMatch.Groups["uppercaseLetters"].Value;
                if (uppercaseLetters.Length <= 2)
                {
                    replacement += uppercaseLetters.ToUpper();
                }
            }
            var lowerMatch = lower.Match(description);
            if (lowerMatch.Success)
            {
                string lowercaseLetters = lowerMatch.Groups["lowercaseLetters"].Value;
                if (lowercaseLetters.Length <= 2)
                {
                    replacement += lowercaseLetters.ToLower();
                }
            }
            string listItem = $"{{ '{c}', \"{replacement}\" }},".PadRight(17)
                + $"// \\u{unicode} ({description2})\n";
            Console.Write(listItem);
            if (replacement.Length < 2)
            {
                sbDiacriticMap.Append(listItem);
            }
            else
            {
                sbLigatureMap.Append(listItem);
            }
        }

        File.WriteAllText(
            "/Users/shaun/Documents/Web & software development/C#/Projects/Galaxon/Core/Program/data/DiacriticMap.txt",
            sbDiacriticMap.ToString());
        File.WriteAllText(
            "/Users/shaun/Documents/Web & software development/C#/Projects/Galaxon/Core/Program/data/LigatureMap.txt",
            sbLigatureMap.ToString());
    }

    public static void ProcessLigaturesHtml()
    {
        string path =
            "/Users/shaun/Documents/Web & software development/C#/Projects/Galaxon/Core/Program/data/ligatures.html";

        XmlDocument doc = new ();
        doc.PreserveWhitespace = false;
        doc.Load(path);

        StringBuilder sbLigatureMap = new ();

        Regex rxUnicode = new ("U+[0-9A-F]{4,5}", RegexOptions.IgnoreCase);
        Regex rxReplacement = new ("[a-z]{2,3}", RegexOptions.IgnoreCase);

        XmlNode? tbody = doc.ChildNodes.Item(0)?.ChildNodes.Item(1);
        foreach (XmlNode tr in tbody!)
        {
            if (tr.Name != "tr")
            {
                continue;
            }

            string? replacements = tr.ChildNodes[0]?.InnerText.Trim();
            string? ligatures = tr.ChildNodes[1]?.InnerText.Trim();
            string? unicodes = tr.ChildNodes[2]?.InnerText.Trim();
            string? htmlEntities = tr.ChildNodes[3]?.InnerText.Trim();
            Console.WriteLine($"{replacements} | {ligatures} | {unicodes} | {htmlEntities}");

            // Strip any brackets and their contents from replacements and unicodes strings.
            // Also strip whitespace and zero-width non-joiners.
            replacements = replacements!.StripBrackets().StripWhitespace();
            replacements = Regex.Replace(replacements, @"(‌|&zwnj;)", "");
            unicodes = unicodes!.StripBrackets().StripWhitespace();
            unicodes = Regex.Replace(unicodes, @"(\u200C|&zwnj;)", "");

            Console.WriteLine($"{unicodes} => {replacements}");

            // Extract the replacement strings.
            string[] replacementStrings = replacements.Split(',');

            // Extract the unicode strings.
            string[] unicodeStrings = unicodes.Split(',');

            if (replacementStrings.Length != unicodeStrings.Length)
            {
                throw new Exception("Mismatch between ");
            }

            for (int i = 0; i < replacementStrings.Length; i++)
            {
                string replacement = replacementStrings[i];
                string unicode = unicodeStrings[i][2..];
                if (unicode.Length > 4)
                {
                    continue;
                }
                Console.WriteLine($"{unicode} => {replacement}");
                char c = (char)ConvertBase.FromHex<ushort>(unicode);
                string listItem = $"{{ '{c}', \"{replacement}\" }},".PadRight(17) +
                    $"// \\u{unicode}\n";
                Console.Write(listItem);
                sbLigatureMap.Append(listItem);
            }

            Console.WriteLine();
        }

        File.WriteAllText(
            "/Users/shaun/Documents/Web & software development/C#/Projects/Galaxon/Core/Program/data/WikipediaLigatureMap.txt",
            sbLigatureMap.ToString());
    }
}
