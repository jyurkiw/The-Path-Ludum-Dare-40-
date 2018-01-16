using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Branch
{
    /// <summary>
    /// The branch ID. Should be unique. Used to differentate branches that overlap.
    /// </summary>
    public int ID { get; private set; }

    /// <summary>
    /// The parent Path object.
    /// </summary>
    public Path Parent { get; private set; }

    /// <summary>
    /// The node this branch leads out towards the origin.
    /// OutNode is not a part of this branch.
    /// </summary>
    public Node OutNode { get; set; }

    /// <summary>
    /// The first node in the branch. This node IS a part of this branch.
    /// </summary>
    public Node InNode { get; private set; }

    /// <summary>
    /// Is this branch open to adding more nodes?
    /// </summary>
    public bool Open { get; private set; }

    public Node this[int x, int y]
    {
        get
        {
            Vector2Int key = new Vector2Int(x, y);
            if (_nodeDirectory.ContainsKey(key))
                return _nodeDirectory[key];
            else return null;
        }
    }

    protected Dictionary<Vector2Int, Node> _nodeDirectory;

    public Branch(int id, Node outNode, NODE_DIRECTION direction, Path parent = null)
    {
        ID = id;
        Parent = parent;
        OutNode = outNode;

        Vector2Int initLoc = outNode.Next(direction);
        InNode = new Node(initLoc.x, initLoc.y, this);

        _nodeDirectory = new Dictionary<Vector2Int, Node>();
        _nodeDirectory.Add(InNode.VectorLocation, InNode);

        Open = true;
    }

    /// <summary>
    /// Get the node count of this branch.
    /// </summary>
    public int Count
    {
        get { return _nodeDirectory.Keys.Count; }
    }

    /// <summary>
    /// Add a new node to the build-end of of the branch.
    /// New nodes automatically become the new InNode unless the NODE_DIRECTION
    /// passed is a multi-direction. In this case the branch is "closed" and no
    /// more nodes can be added.
    /// </summary>
    /// <param name="direction"></param>
    public NODE_DIRECTION[] Add(NODE_DIRECTION direction)
    {
        if (Open)
        {
            if (direction.IsMulti())
            {
                // close the branch
                Open = false;
                InNode.InDirection = direction;
                return direction.GetDirections().ToArray();
            }
            else
            {
                Vector2Int newLocation = InNode.Next(direction);
                Node nextNode = new Node(newLocation.x, newLocation.y, this);
                InNode.InNode = nextNode;
                nextNode.OutNode = InNode;
                InNode = nextNode;
                _nodeDirectory.Add(nextNode.VectorLocation, nextNode);

                if (Parent != null)
                    Parent.SetNodeDirectoryData(nextNode);

                return new NODE_DIRECTION[] { direction };
            }
        }
        else
        {
            throw new BranchException(BRANCH_EXCEPTION_TYPE.BRANCH_NOT_OPEN);
        }
    }
}

/// <summary>
/// Various types of branch exceptions.
/// </summary>
public enum BRANCH_EXCEPTION_TYPE
{
    BRANCH_NOT_OPEN
}

/// <summary>
/// Branch exceptions.
/// </summary>
public class BranchException : Exception
{
    private static string GetErrorMessage(BRANCH_EXCEPTION_TYPE type)
    {
        switch (type)
        {
            case BRANCH_EXCEPTION_TYPE.BRANCH_NOT_OPEN:
                return "Tried to add a node to a closed branch.";
            default:
                return string.Empty;
        }
    }

    public BranchException(BRANCH_EXCEPTION_TYPE type) : base(GetErrorMessage(type)) { }
}

public class Branch_Test : Branch
{
    public new Dictionary<Vector2Int, Node> _nodeDirectory
    {
        get
        {
            return base._nodeDirectory;
        }
    }

    public Branch_Test(int id, Node outNode, NODE_DIRECTION direction) : base(id, outNode, direction) { }
}