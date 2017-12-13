using System.Collections;
using System.Collections.Generic;
using UnityEngine.TestTools;
using NUnit.Framework;
using UnityEngine;

public class TestNode
{
    private Node testNode;

    private static readonly Vector2Int zerozero = new Vector2Int(0, 0);
    private static readonly Vector2Int zeroone = new Vector2Int(0, 1);
    private static readonly Vector2Int onezero = new Vector2Int(1, 0);
    private static readonly Vector2Int zeronegone = new Vector2Int(0, -1);
    private static readonly Vector2Int negonezero = new Vector2Int(-1, 0);

    [TearDown]
    public void TearDown()
    {
        testNode = null;
    }

    [Test]
    public void TestNode_BasicConstructor()
    {
        testNode = new Node(zerozero);

        Assert.AreEqual(zerozero, testNode.Location);
    }

    [Test]
    public void TestNode_LocationAndNodeConstructor()
    {
        Node otherNode = new Node(zeroone);
        testNode = new Node(zerozero, otherNode);

        Assert.AreEqual(zerozero, testNode.Location);
        Assert.AreEqual(zeroone, otherNode.Location);

        Assert.AreEqual(1, testNode.Connections.Count);
        Assert.AreEqual(1, otherNode.Connections.Count);

        Assert.AreEqual(zeroone, testNode.Connections[0].Connection.Location);
        Assert.AreEqual(NodeConnection.ConnectionType.OUTGOING, testNode.Connections[0].Type);

        Assert.AreEqual(zerozero, otherNode.Connections[0].Connection.Location);
        Assert.AreEqual(NodeConnection.ConnectionType.INCOMMING, otherNode.Connections[0].Type);
    }

    [Test]
    public void TestNode_AddConnection()
    {
        Node otherNode = new Node(zeroone);
        testNode = new Node(zerozero);

        testNode.AddConnection(otherNode, NodeConnection.ConnectionType.OUTGOING);

        Assert.AreEqual(zerozero, testNode.Location);
        Assert.AreEqual(zeroone, otherNode.Location);

        Assert.AreEqual(1, testNode.Connections.Count);
        Assert.AreEqual(1, otherNode.Connections.Count);

        Assert.AreEqual(zeroone, testNode.Connections[0].Connection.Location);
        Assert.AreEqual(NodeConnection.ConnectionType.OUTGOING, testNode.Connections[0].Type);

        Assert.AreEqual(zerozero, otherNode.Connections[0].Connection.Location);
        Assert.AreEqual(NodeConnection.ConnectionType.INCOMMING, otherNode.Connections[0].Type);
    }

    [Test]
    public void TestNode_AddConnectionException_FourIncomming()
    {
        testNode = new Node(zerozero);
        Node zerooneNode = new Node(zeroone);
        Node onezeroNode = new Node(onezero);
        Node zeronegoneNode = new Node(zeronegone);
        Node negonezeroNode = new Node(negonezero);

        testNode.AddConnection(zerooneNode, NodeConnection.ConnectionType.INCOMMING);
        testNode.AddConnection(onezeroNode, NodeConnection.ConnectionType.INCOMMING);
        testNode.AddConnection(zeronegoneNode, NodeConnection.ConnectionType.INCOMMING);

        Assert.Throws<System.Exception>(() => testNode.AddConnection(negonezeroNode, NodeConnection.ConnectionType.INCOMMING));
    }

    [Test]
    public void TestNode_AddConnectionException_FourOutgoing()
    {
        testNode = new Node(zerozero);
        Node zerooneNode = new Node(zeroone);
        Node onezeroNode = new Node(onezero);
        Node zeronegoneNode = new Node(zeronegone);
        Node negonezeroNode = new Node(negonezero);

        testNode.AddConnection(zerooneNode, NodeConnection.ConnectionType.OUTGOING);
        testNode.AddConnection(onezeroNode, NodeConnection.ConnectionType.OUTGOING);
        testNode.AddConnection(zeronegoneNode, NodeConnection.ConnectionType.OUTGOING);

        Assert.Throws<System.Exception>(() => testNode.AddConnection(negonezeroNode, NodeConnection.ConnectionType.OUTGOING));
    }

    [Test]
    public void TestNode_AddConnectionException_AddFourth()
    {
        testNode = new Node(zerozero);
        Node zerooneNode = new Node(zeroone);
        Node onezeroNode = new Node(onezero);
        Node zeronegoneNode = new Node(zeronegone);
        Node negonezeroNode = new Node(negonezero);

        testNode.AddConnection(zerooneNode, NodeConnection.ConnectionType.INCOMMING);
        testNode.AddConnection(onezeroNode, NodeConnection.ConnectionType.INCOMMING);
        testNode.AddConnection(zeronegoneNode, NodeConnection.ConnectionType.INCOMMING);
        testNode.AddConnection(negonezeroNode, NodeConnection.ConnectionType.OUTGOING);

        Assert.Pass();
    }

    [Test]
    public void TestNode_AddConnectionException_AddFifth()
    {
        testNode = new Node(zerozero);
        Node zerooneNode = new Node(zeroone);
        Node onezeroNode = new Node(onezero);
        Node zeronegoneNode = new Node(zeronegone);
        Node negonezeroNode = new Node(negonezero);

        testNode.AddConnection(zerooneNode, NodeConnection.ConnectionType.INCOMMING);
        testNode.AddConnection(onezeroNode, NodeConnection.ConnectionType.INCOMMING);
        testNode.AddConnection(zeronegoneNode, NodeConnection.ConnectionType.INCOMMING);
        testNode.AddConnection(negonezeroNode, NodeConnection.ConnectionType.OUTGOING);

        Assert.Throws<System.Exception>(() => testNode.AddConnection(negonezeroNode, NodeConnection.ConnectionType.INCOMMING));
    }
}
