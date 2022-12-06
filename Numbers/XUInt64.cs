namespace AstroMultimedia.Core.Numbers;

public static class XUInt64
{
    public static ulong Sum(this IEnumerable<ulong> items) =>
        items.Aggregate(0UL, (sum, num) => sum + num);
}
