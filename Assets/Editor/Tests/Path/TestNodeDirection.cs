using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TestNodeDirection {

	[Test]
	public void Test_DirectionBinaryOperations_SanityCheckNone()
    {
        Assert.AreEqual(NODE_DIRECTION.NONE, NODE_DIRECTION.NONE);
	}

    [Test]
    public void Test_DirectionBinaryOperations_CheckNones()
    {
        Assert.True(NODE_DIRECTION.NONE.CheckFlag(NODE_DIRECTION.NONE));
    }

    [Test]
    public void Test_DirectionBinaryOperations_CheckNonNones()
    {
        Assert.True(NODE_DIRECTION.UP_DOWN_LEFT_RIGHT.CheckFlag(NODE_DIRECTION.UP_DOWN_LEFT_RIGHT));
    }

    [Test]
    public void Test_DirectionBinaryOperations_CheckInexact()
    {
        Assert.True(NODE_DIRECTION.UP_DOWN_LEFT_RIGHT.CheckFlag(NODE_DIRECTION.UP));
    }

    [Test]
    public void Test_DirectionBinaryOperations_CheckInexact_False()
    {
        Assert.False(NODE_DIRECTION.UP_DOWN.CheckFlag(NODE_DIRECTION.LEFT));
    }

    [Test]
    public void Test_PathEntrancesAndExits_Node()
    {
        Node A = new Node(2, 2);
        Node B = new Node(2, 3);
        Node C = new Node(2, 1);

        A.InNode = B;
        A.OutNode = C;

        Assert.AreEqual(NODE_DIRECTION.UP_DOWN, A.GetPathEntrancesAndExits());
    }

    [Test]
    public void Test_PathEntrancesAndExits_NodeList()
    {
        Node A = new Node(2, 2);
        Node B = new Node(2, 3);
        Node C = new Node(2, 1);
        A.InNode = B;
        A.OutNode = C;

        Node D = new Node(2, 2);
        Node E = new Node(2, 3);
        Node F = new Node(1, 2);
        D.InNode = E;
        D.OutNode = F;

        Node G = new Node(2, 2);
        Node H = new Node(2, 1);
        Node I = new Node(2, 3);
        G.InNode = H;
        G.OutNode = I;

        List<Node> nl = new List<Node>() { A, D, G };

        Assert.AreEqual(NODE_DIRECTION.UP_DOWN_LEFT, nl.GetPathEntrancesAndExits());
    }

    [Test]
    public void Test_GetDirections()
    {
        NODE_DIRECTION[] da = NODE_DIRECTION.DOWN_LEFT_RIGHT.GetDirections().ToArray();
        NODE_DIRECTION[] ex = { NODE_DIRECTION.DOWN, NODE_DIRECTION.LEFT, NODE_DIRECTION.RIGHT };

        Assert.AreEqual(ex, da);
    }

    [Test]
    public void Test_IsMulti_True()
    {
        Assert.IsTrue(NODE_DIRECTION.DOWN_LEFT_RIGHT.IsMulti());
    }

    [Test]
    public void Test_IsMulti_False()
    {
        Assert.IsFalse(NODE_DIRECTION.DOWN.IsMulti());
    }
}
