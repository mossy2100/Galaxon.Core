using Galaxon.Core.Numbers;

Console.WriteLine("Testing...");

for (int i = 0; i < 1000; i++)
{
    Half n = XHalf.GetRandom();
    Console.WriteLine(n);
}
