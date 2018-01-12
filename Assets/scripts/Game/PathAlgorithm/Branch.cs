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
    /// The node this branch leads out towards the origin.
    /// OutNode is not a part of this branch.
    /// </summary>
    public Node OutNode { get; set; }

    /// <summary>
    /// The first node in the branch. This node IS a part of this branch.
    /// </summary>
    public Node InNode { get; set; }

    /// <summary>
    /// Is this branch open to adding more nodes?
    /// </summary>
    public bool Open { get; private set; }

    /// <summary>
    /// The first node of a branch that is built (InNode is the first node of minion traversal. _outNode is the first node built by the path algorithm).
    /// _outnode is a part of this branch.
    /// </summary>
    protected Node _buildStartNode;

    /// <summary>
    /// The current node of the build process.
    /// </summary>
    protected Node currentNode;

    public Branch(int id, Node outNode, NODE_DIRECTION direction)
    {
        ID = id;
        OutNode = outNode;

        Vector2Int initLoc = outNode.Next(direction);
        _buildStartNode = new Node(initLoc.x, initLoc.y);

        currentNode = _buildStartNode;
        Open = true;
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
                InNode = currentNode;

                return direction.GetDirections().ToArray();
            }
            else
            {
                Vector2Int newLocation = currentNode.Next(direction);
                Node nextNode = new Node(newLocation.x, newLocation.y);
                currentNode.InNode = nextNode;
                nextNode.OutNode = currentNode;
                currentNode = nextNode;

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
    /// <summary>
    /// The first node of a branch that is built (InNode is the first node of minion traversal. _outNode is the first node built by the path algorithm).
    /// _outnode is a part of this branch.
    /// </summary>
    public new Node _buildStartNode
    {
        get
        {
            return base._buildStartNode;
        }

        set
        {
            base._buildStartNode = value;
        }
    }

    /// <summary>
    /// The current node of the build process.
    /// </summary>
    public new Node currentNode
    {
        get
        {
            return base.currentNode;
        }

        set
        {
            base.currentNode = value;
        }
    }

    public Branch_Test(int id, Node outNode, NODE_DIRECTION direction) : base(id, outNode, direction) { }
}