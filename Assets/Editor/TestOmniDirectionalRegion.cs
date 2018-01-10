using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestOmniDirectionalRegion
{
    private const int size = 4;
    private OmniDirectionRegions algo;

    [SetUp]
    public void Setup()
    {
        algo = new OmniDirectionRegions(size, Vector2Int.zero);
    }

    [Test]
    public void Test_BasicFunction_Size()
    {
        Assert.AreEqual(12, algo.Size);
    }

    [Test]
    public void Test_BasicFunction_OuterRegionsCount()
    {
        Assert.AreEqual(4, algo.OuterRegions.Count);
    }

    [Test]
    public void Test_BasicFunction_InnerArea()
    {
        Assert.AreEqual(new Rect(-2, -2, 4, 4), algo.innerArea);
    }

    [Test]
    public void Test_BasicFunction_TopOuterRegion()
    {
        Assert.AreEqual(new Rect(-6, 2, 8, 4), algo.OuterRegions[0]);
    }

    [Test]
    public void Test_BasicFunction_RightOuterRegion()
    {
        Assert.AreEqual(new Rect(2, -2, 4, 8), algo.OuterRegions[1]);
    }

    [Test]
    public void Test_BasicFunction_BottomOuterRegion()
    {
        Assert.AreEqual(new Rect(-2, -6, 8, 4), algo.OuterRegions[2]);
    }

    [Test]
    public void Test_BasicFunction_LeftOuterRegion()
    {
        Assert.AreEqual(new Rect(-6, -6, 4, 8), algo.OuterRegions[3]);
    }
}
