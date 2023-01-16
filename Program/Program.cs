using Galaxon.Core.Numbers;
using Galaxon.Core.Strings;

Console.WriteLine("0x" + 0b1111111111.ToHex().ZeroPad(8).GroupDigits());
