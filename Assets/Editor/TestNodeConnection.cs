using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestNodeConnection
{
    private Node testNode;
    private NodeConnection testConnection;

    [TearDown]
    public void TearDown()
    {
        testNode = null;
        testConnection = null;
    }

	[Test]
	public void TestNodeConnection_DefaultConstructor()
    {
        testConnection = new NodeConnection();

        Assert.IsNull(testConnection.Connection);
        Assert.AreEqual(NodeConnection.ConnectionType.INCOMMING, testConnection.Type);
	}

    [Test]
    public void TestNodeConnection_NodeAndTypeConstructor()
    {
        testNode = new Node(new Vector2Int(0, 0));
        testConnection = new NodeConnection(testNode, NodeConnection.ConnectionType.INCOMMING);

        Assert.AreEqual(new Vector2Int(0, 0), testConnection.Connection.Location);
        Assert.AreEqual(NodeConnection.ConnectionType.INCOMMING, testConnection.Type);
    }

    [Test]
    public void TestNodeConnection_GetOppositeConnectionType()
    {
        Assert.AreEqual(NodeConnection.ConnectionType.INCOMMING, NodeConnection.GetOppositeConnectionType(NodeConnection.ConnectionType.OUTGOING));
        Assert.AreEqual(NodeConnection.ConnectionType.OUTGOING, NodeConnection.GetOppositeConnectionType(NodeConnection.ConnectionType.INCOMMING));
    }
}
