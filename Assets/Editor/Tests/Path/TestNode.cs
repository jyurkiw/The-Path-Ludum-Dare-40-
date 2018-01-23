using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestNode {

	[Test]
	public void Test_Node_DirectionOf_UP()
    {
        Node A = new Node(2, 2);
        Node B = new Node(2, 3);

        Assert.AreEqual(NODE_DIRECTION.UP, A.DirectionOf(B));
	}

    [Test]
    public void Test_Node_DirectionOf_DOWN()
    {
        Node A = new Node(2, 2);
        Node B = new Node(2, 1);

        Assert.AreEqual(NODE_DIRECTION.DOWN, A.DirectionOf(B));
    }

    [Test]
    public void Test_Node_DirectionOf_LEFT()
    {
        Node A = new Node(2, 2);
        Node B = new Node(1, 2);

        Assert.AreEqual(NODE_DIRECTION.LEFT, A.DirectionOf(B));
    }

    [Test]
    public void Test_Node_DirectionOf_RIGHT()
    {
        Node A = new Node(2, 2);
        Node B = new Node(3, 2);

        Assert.AreEqual(NODE_DIRECTION.RIGHT, A.DirectionOf(B));
    }

    [Test]
    public void Test_Node_Next_UP()
    {
        Node A = new Node(2, 2);
        NODE_DIRECTION dir = NODE_DIRECTION.UP;
        Vector2Int ex = new Vector2Int(2, 3);

        Assert.AreEqual(ex, A.Next(dir));
    }

    [Test]
    public void Test_Node_Next_DOWN()
    {
        Node A = new Node(2, 2);
        NODE_DIRECTION dir = NODE_DIRECTION.DOWN;
        Vector2Int ex = new Vector2Int(2, 1);

        Assert.AreEqual(ex, A.Next(dir));
    }

    [Test]
    public void Test_Node_Next_LEFT()
    {
        Node A = new Node(2, 2);
        NODE_DIRECTION dir = NODE_DIRECTION.LEFT;
        Vector2Int ex = new Vector2Int(1, 2);

        Assert.AreEqual(ex, A.Next(dir));
    }

    [Test]
    public void Test_Node_Next_RIGHT()
    {
        Node A = new Node(2, 2);
        NODE_DIRECTION dir = NODE_DIRECTION.RIGHT;
        Vector2Int ex = new Vector2Int(3, 2);

        Assert.AreEqual(ex, A.Next(dir));
    }

    [Test]
    public void Test_Node_NextException_NONE()
    {
        Node A = new Node(2, 2);
        NODE_DIRECTION dir = NODE_DIRECTION.NONE;

        Assert.Throws(typeof(DirectionException), () => A.Next(dir));
    }

    [Test]
    public void Test_Node_NextException_MULTI()
    {
        Node A = new Node(2, 2);
        NODE_DIRECTION dir = NODE_DIRECTION.UP_DOWN_LEFT_RIGHT;

        Assert.Throws(typeof(DirectionException), () => A.Next(dir));
    }
}
