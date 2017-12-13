using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Path is a node-based map that simply tells minions where to go next.
/// Nodes are bi-directional and contain two to four total connections to other nodes.
///     A Node must have at least one incoming connection.
///     A Node must have at least one outgoing connection.
///     A Node cannot have more than four total connections.
/// </summary>
public class Node {
    public Vector2Int Location { get; set; }
    public List<NodeConnection> Connections { get; set; }

    private bool incommingConn = false, outgoingConn = false;

    /// <summary>
    /// Create a new node with the given location.
    /// </summary>
    /// <param name="location"></param>
    public Node(Vector2Int location)
    {
        Location = location;
        Connections = new List<NodeConnection>();
    }

    /// <summary>
    /// Create a new node with the passed node as an outgoing connection at the given location.
    /// </summary>
    /// <param name="location">The location of this node.</param>
    /// <param name="connectingNode">The node to connect to.</param>
    public Node(Vector2Int location, Node connectingNode)
    {
        Location = location;
        Connections = new List<NodeConnection>();
        AddConnection(connectingNode, NodeConnection.ConnectionType.OUTGOING);
    }

    /// <summary>
    /// Add a connection to this node of the passed type.
    /// Also addes this node to the connecting node as the opposite type.
    /// </summary>
    /// <param name="connection">The node to connect to.</param>
    /// <param name="type">The type of connection (INCOMMING or OUTGOING)</param>
    public void AddConnection(Node connection, NodeConnection.ConnectionType type)
    {
        if (Connections.Count >= 3 && (!incommingConn && type == NodeConnection.ConnectionType.OUTGOING || !outgoingConn && type == NodeConnection.ConnectionType.INCOMMING))
            throw new System.Exception(GameGlobals.NODE_CONNECTION_REQUIREMENTS);
        else if (Connections.Count >= 4)
            throw new System.Exception(GameGlobals.NODE_CONNECTION_LIMIT_EXCEEDED);
        else
        {
            Connections.Add(new NodeConnection(connection, type));
            connection.Connections.Add(new NodeConnection(this, NodeConnection.GetOppositeConnectionType(type)));

            if (type == NodeConnection.ConnectionType.INCOMMING) incommingConn = true;
            else if (type == NodeConnection.ConnectionType.OUTGOING) outgoingConn = true;
        }
    }
}

/// <summary>
/// A node connection is a reference to another node object (default null for end-of-line)
/// and an incomming/outgoing demarkation.
/// </summary>
public class NodeConnection
{
    public enum ConnectionType { INCOMMING, OUTGOING };
    public Node Connection { get; set; }
    public ConnectionType Type { get; set; }

    public NodeConnection()
    {
        Connection = null;
        Type = ConnectionType.INCOMMING;
    }

    public NodeConnection(Node connection, ConnectionType type)
    {
        Connection = connection;
        Type = type;
    }

    public static ConnectionType GetOppositeConnectionType(ConnectionType type)
    {
        return type == ConnectionType.INCOMMING ? ConnectionType.OUTGOING : ConnectionType.INCOMMING;
    }
}