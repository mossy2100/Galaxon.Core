namespace Galaxon.Core.Numbers;

/// <summary>Extension methods for uint.</summary>
public static class XUint
{
    /// <summary>
    /// Get a random uint.
    /// </summary>
    public static uint GetRandom()
    {
        return (uint)XInt.GetRandom();
    }
}
