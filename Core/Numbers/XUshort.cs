namespace Galaxon.Core.Numbers;

/// <summary>Extension methods for ushort.</summary>
public static class XUshort
{
    /// <summary>
    /// Get a random ushort.
    /// </summary>
    public static ushort GetRandom()
    {
        // Get a random non-negative int and cast to ushort.
        // The value will wrap to a ushort within the valid range for the type, without overflow.
        Random rnd = new ();
        return (ushort)rnd.Next();
    }
}
