using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Path
{
    /// <summary>
    /// The origin node. This is the location of the player's well.
    /// </summary>
    public Node Origin { get; private set; }

    /// <summary>
    /// The branches that make up the level.
    /// </summary>
    public Dictionary<int, Branch> Branches { get; private set; }

    public List<int> OuterBranchIDs { get; private set; }

    private int _branchId;
    private Dictionary<Vector2Int, List<Node>> _nodeDirectory { get; set; }

    public Path(Node origin, NODE_DIRECTION initialDirections)
    {
        Origin = origin;
        Branches = new Dictionary<int, Branch>();
        OuterBranchIDs = new List<int>();

        _branchId = 1;
        _nodeDirectory = new Dictionary<Vector2Int, List<Node>>();
        SetNodeDirectoryData(Origin);

        // Create initial outer branches
        foreach (NODE_DIRECTION direction in initialDirections.GetDirections())
        {
            Branch outerBranch = new Branch(_branchId++, origin, direction, this);
            Add(outerBranch);
        }
    }

    /// <summary>
    /// The branch count in this path.
    /// </summary>
    public int Count
    {
        get { return Branches.Count; }
    }

    /// <summary>
    /// Checks the node directiory for a key at the given position.
    /// </summary>
    /// <param name="X"></param>
    /// <param name="Y"></param>
    /// <returns></returns>
    public bool IsNodeAt(int X, int Y)
    {
        return _nodeDirectory.ContainsKey(new Vector2Int(X, Y));
    }

    /// <summary>
    /// Gets all nodes at the given location.
    /// </summary>
    /// <param name="X"></param>
    /// <param name="Y"></param>
    /// <returns></returns>
    public Node[] GetNodesAt(int X, int Y)
    {
        Vector2Int key = new Vector2Int(X, Y);
        return _nodeDirectory.ContainsKey(key) ? _nodeDirectory[key].ToArray() : new Node[] { };
    }

    /// <summary>
    /// Get the node from the given branch at the given location.
    /// </summary>
    /// <param name="X"></param>
    /// <param name="Y"></param>
    /// <param name="BranchId"></param>
    /// <returns></returns>
    public Node GetNodeAt(int X, int Y, int BranchId)
    {
        return Branches[BranchId][X, Y];
    }

    /// <summary>
    /// Add a node to the nodeDirectory.
    /// </summary>
    /// <param name="loc"></param>
    /// <param name="node"></param>
    public void SetNodeDirectoryData(Node node)
    {
        Vector2Int loc = node.VectorLocation;
        if (!_nodeDirectory.ContainsKey(loc))
            _nodeDirectory.Add(loc, new List<Node>());
        _nodeDirectory[loc].Add(node);
    }

    /// <summary>
    /// Add a new branch to the path.
    /// New branches are assumed to be brand-new, consisting of nothing more than their default initial node.
    /// </summary>
    /// <param name="branch"></param>
    private void Add(Branch branch)
    {
        if (branch.Count > 1)
            throw new PathException(PATH_EXCEPTION_TYPE.BRANCH_POPULATED);

        Branches.Add(branch.ID, branch);
        OuterBranchIDs.Add(branch.ID);
        SetNodeDirectoryData(branch.InNode);
    }

    /// <summary>
    /// Branch the passed branch.
    /// Remove the branch from the OuterBranchIds collection and add new branches based on it's InNode InDirection (the outer-most node in a branch).
    /// Throw an exception if the branch is open.
    /// </summary>
    /// <param name="branchingBranch"></param>
    public void Branch(int branchId)
    {
        Branch branchingBranch = Branches[branchId];

        if (branchingBranch.Open)
            throw new PathException(PATH_EXCEPTION_TYPE.BRANCH_OPEN);

        OuterBranchIDs.Remove(branchingBranch.ID);

        foreach (NODE_DIRECTION direction in branchingBranch.InNode.InDirection.GetDirections())
        {
            Branch outerBranch = new Branch(_branchId++, branchingBranch.InNode, direction, this);
            Add(outerBranch);
        }
    }
}

public enum PATH_EXCEPTION_TYPE
{
    BRANCH_OPEN,
    BRANCH_POPULATED
}

public class PathException : Exception
{
    private static string GetMessage(PATH_EXCEPTION_TYPE type)
    {
        switch(type)
        {
            case PATH_EXCEPTION_TYPE.BRANCH_OPEN:
                return "Branch object must be closed at this point.";
            case PATH_EXCEPTION_TYPE.BRANCH_POPULATED:
                return "Branch object must consist of no more than the initial node at this point.";
            default:
                return "An error was detected.";
        }
    }

    public PathException(PATH_EXCEPTION_TYPE type) : base(PathException.GetMessage(type)) { }
}
