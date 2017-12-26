using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestVector2Int_PathUtilExtensions
{
    public Vector2Int zero = Vector2Int.zero;

    [Test]
    public void Test_GetUpLocation()
    {
        Vector2Int other = new Vector2Int(0, 1);

        Assert.AreEqual(other, zero.GetUpLocation());
    }

    [Test]
    public void Test_GetDownLocation()
    {
        Vector2Int other = new Vector2Int(0, -1);

        Assert.AreEqual(other, zero.GetDownLocation());
    }

    [Test]
    public void Test_GetLeftLocation()
    {
        Vector2Int other = new Vector2Int(-1, 0);

        Assert.AreEqual(other, zero.GetLeftLocation());
    }

    [Test]
    public void Test_GetRightLocation()
    {
        Vector2Int other = new Vector2Int(1, 0);

        Assert.AreEqual(other, zero.GetRightLocation());
    }

    [Test]
    public void Test_GetNorthLocation()
    {
        Vector2Int other = new Vector2Int(0, 1);

        Assert.AreEqual(other, zero.GetNorthLocation());
    }

    [Test]
    public void Test_GetSouthLocation()
    {
        Vector2Int other = new Vector2Int(0, -1);

        Assert.AreEqual(other, zero.GetSouthLocation());
    }

    [Test]
    public void Test_GetWestLocation()
    {
        Vector2Int other = new Vector2Int(-1, 0);

        Assert.AreEqual(other, zero.GetWestLocation());
    }

    [Test]
    public void Test_GetEastLocation()
    {
        Vector2Int other = new Vector2Int(1, 0);

        Assert.AreEqual(other, zero.GetEastLocation());
    }

    [Test]
    public void Test_ToVector2Int()
    {
        Vector2Int t1 = new Vector2Int(4, 4);
        Vector2 t2 = t1;

        Assert.AreEqual(t1, t2.ToVector2Int());
    }
}
