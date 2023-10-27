using System.Diagnostics;
using Galaxon.Core.Numbers;

namespace Galaxon.Core.Tests;

[TestClass]
public class TestXFloatingPoint
{
    [TestMethod]
    public void TestHalfMinMaxPosNormalSubnormalValues()
    {
        var minPosSubnormal = XFloatingPoint.GetMinPosSubnormalValue<Half>();
        Trace.WriteLine($"XHalf.MinPosSubnormalValue = {minPosSubnormal:E10}");
        var maxPosSubnormal = XFloatingPoint.GetMaxPosSubnormalValue<Half>();
        Trace.WriteLine($"XHalf.MaxPosSubnormalValue = {maxPosSubnormal:E10}");
        var minPosNormal = XFloatingPoint.GetMinPosNormalValue<Half>();
        Trace.WriteLine($"XHalf.MinPosNormalValue = {minPosNormal:E10}");
        var maxPosNormal = XFloatingPoint.GetMaxPosNormalValue<Half>();
        Trace.WriteLine($"XHalf.MaxPosNormalValue = {maxPosNormal:E10}");
    }

    [TestMethod]
    public void TestFloatMinMaxPosNormalSubnormalValues()
    {
        var minPosSubnormal = XFloatingPoint.GetMinPosSubnormalValue<float>();
        Trace.WriteLine($"XFloat.MinPosSubnormalValue = {minPosSubnormal:E10}");
        var maxPosSubnormal = XFloatingPoint.GetMaxPosSubnormalValue<float>();
        Trace.WriteLine($"XFloat.MaxPosSubnormalValue = {maxPosSubnormal:E10}");
        var minPosNormal = XFloatingPoint.GetMinPosNormalValue<float>();
        Trace.WriteLine($"XFloat.MinPosNormalValue = {minPosNormal:E10}");
        var maxPosNormal = XFloatingPoint.GetMaxPosNormalValue<float>();
        Trace.WriteLine($"XFloat.MaxPosNormalValue = {maxPosNormal:E10}");
    }

    [TestMethod]
    public void TestDoubleMinMaxPosNormalSubnormalValues()
    {
        var minPosSubnormal = XFloatingPoint.GetMinPosSubnormalValue<double>();
        Trace.WriteLine($"XDouble.MinPosSubnormalValue = {minPosSubnormal:E10}");
        var maxPosSubnormal = XFloatingPoint.GetMaxPosSubnormalValue<double>();
        Trace.WriteLine($"XDouble.MaxPosSubnormalValue = {maxPosSubnormal:E10}");
        var minPosNormal = XFloatingPoint.GetMinPosNormalValue<double>();
        Trace.WriteLine($"XDouble.MinPosNormalValue = {minPosNormal:E10}");
        var maxPosNormal = XFloatingPoint.GetMaxPosNormalValue<double>();
        Trace.WriteLine($"XDouble.MaxPosNormalValue = {maxPosNormal:E10}");
    }
}
