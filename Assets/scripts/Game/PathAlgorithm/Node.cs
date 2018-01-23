using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Path Node class.
/// Tracks X and Y location. Can convert to a Vector2Int through the VectorLocation property.
/// Tracks Node In and Out directions.
/// </summary>
public class Node
{
    public int XLocation;
    public int YLocation;

    public Branch Parent { get; private set; }

    /// <summary>
    /// Turn the node's X and Y location into a Vector2Int.
    /// </summary>
    public Vector2Int VectorLocation
    {
        get
        {
            return new Vector2Int(XLocation, YLocation);
        }
    }

    private NODE_DIRECTION _inDirection;

    /// <summary>
    /// The direction that leads into this node.
    /// All nodes lead towards the path origin. You come into the node from farther away, and go out towards the origin.
    /// 
    /// If InDirection is set directly check for NONE, UP, DOWN, LEFT, or RIGHT. If the direction set is not one of those,
    /// it means this node is the end of the branch. Set the direction to the passed multi-direction and clear any existing
    /// in-node. Branching node in connections are handled by the branch object.
    /// </summary>
    public NODE_DIRECTION InDirection
    {
        get
        {
            return _inDirection;
        }

        set
        {
            if (value == NODE_DIRECTION.NONE || value == NODE_DIRECTION.UP ||
                value == NODE_DIRECTION.DOWN || value == NODE_DIRECTION.LEFT ||
                value == NODE_DIRECTION.RIGHT)
            {
                _inDirection = value;
            }
            else
            {
                _inDirection = value;
                _inNode = null;
            }
        }
    }

    /// <summary>
    /// The direction that leads out of this node.
    /// All nodes lead towards the path origin. You come into the node from farther away, and go out towards the origin.
    /// </summary>
    public NODE_DIRECTION OutDirection { get; private set; }

    private Node _inNode;

    /// <summary>
    /// The node that leads into this node.
    /// All nodes lead towards the path origin. You come into the node from farther away, and go out towards the origin.
    /// </summary>
    public Node InNode
    {
        get
        {
            return _inNode;
        }

        set
        {
            _inNode = value;
            InDirection = DirectionOf(value);
        }
    }

    private Node _outNode;

    /// <summary>
    /// The node this node exits out into.
    /// All nodes lead towards the path origin. You come into the node from farther away, and go out towards the origin.
    /// Nodes only lead to one out-node. Multi-node branching happen on the In-Node.
    /// </summary>
    public Node OutNode
    {
        get
        {
            return _outNode;
        }

        set
        {
            _outNode = value;
            OutDirection = DirectionOf(value);
        }
    }

    public Node()
    {
        InDirection = NODE_DIRECTION.NONE;
        OutDirection = NODE_DIRECTION.NONE;

        InNode = null;
        OutNode = null;
    }

    public Node(int X, int Y)
    {
        XLocation = X;
        YLocation = Y;

        InDirection = NODE_DIRECTION.NONE;
        OutDirection = NODE_DIRECTION.NONE;
    }

    public Node(int X, int Y, Branch Parent)
    {
        XLocation = X;
        YLocation = Y;
        this.Parent = Parent;

        InDirection = NODE_DIRECTION.NONE;
        OutDirection = NODE_DIRECTION.NONE;
    }

    /// <summary>
    /// Compare another node to this node and get the UP, DOWN, LEFT, RIGHT direction from this node to that node.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public NODE_DIRECTION DirectionOf(Node other)
    {
        if (XLocation == other.XLocation)
        {
            //return YLocation > o.YLocation ? NODE_DIRECTION.DOWN : NODE_DIRECTION.UP;
            NODE_DIRECTION outd = YLocation > other.YLocation ? NODE_DIRECTION.DOWN : NODE_DIRECTION.UP;
            return outd;
        }
        else if (YLocation == other.YLocation)
        {
            return (NODE_DIRECTION)(6 - ((XLocation - other.XLocation) * 2));
        }
        else
        {
            return NODE_DIRECTION.NONE;
        }
    }

    /// <summary>
    /// Get the location of a neighboring node by direction.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public Vector2Int Next(NODE_DIRECTION direction)
    {
        Vector2Int location = VectorLocation;

        switch (direction)
        {
            case NODE_DIRECTION.UP:
                location.y++;
                break;
            case NODE_DIRECTION.DOWN:
                location.y--;
                break;
            case NODE_DIRECTION.LEFT:
                location.x--;
                break;
            case NODE_DIRECTION.RIGHT:
                location.x++;
                break;
            default:
                throw new DirectionException(DIRECTION_EXCEPTION_TYPE.MULTI_RESTRICTED);
        }

        return location;
    }
}

