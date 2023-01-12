using Galaxon.Core.Numbers;

foreach (int b in new [] { -5, 5 })
{
    for (int i = -10; i <= 10; i++)
    {
        (int d, int m) = XNumberBase.DivMod(i, b);
        // int d = (int)XBigInteger.Div(i, b);
        if (m == 0) Console.WriteLine();
        Console.WriteLine(
            $"{i} \\ {b} = {d}          {i} % {b} = {m}          Check: {i} == {b * d + m}");
    }
    Console.WriteLine("-----");
}
