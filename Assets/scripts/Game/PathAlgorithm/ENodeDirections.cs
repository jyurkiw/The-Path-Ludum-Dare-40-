using System;
using System.Collections.Generic;

/// <summary>
/// Node directions are always in relation to the node itself.
/// InNode = NODE_DIRECTION.UP means that the node entrance direction is from the up direction.
/// OutNode = NODE_DIRECTION.DOWN means that enemies will exit this node to the down direction.
/// </summary>
[Flags]
public enum NODE_DIRECTION
{
    NONE                = 0,

    UP                  = 1,
    UP_DOWN             = 1 | 2,
    UP_DOWN_LEFT        = 1 | 2 | 4,
    UP_DOWN_RIGHT       = 1 | 2 | 8,
    UP_DOWN_LEFT_RIGHT  = 1 | 2 | 4 | 8,
    UP_LEFT             = 1 | 4,
    UP_LEFT_RIGHT       = 1 | 4 | 8,
    UP_RIGHT            = 1 | 8,

    DOWN                = 2,
    DOWN_LEFT           = 2 | 4,
    DOWN_RIGHT          = 2 | 8,
    DOWN_LEFT_RIGHT     = 2 | 4 | 8,

    LEFT                = 4,
    LEFT_RIGHT          = 4 | 8,

    RIGHT               = 8
};

public static class NodeOps
{
    /// <summary>
    /// Check the passed flag against this flag.
    /// This is not an exactness check. This is a binary check.
    /// That means UP_DOWN.CheckFlag(UP) will return true.
    /// For an exact check, use ==.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool CheckFlag(this NODE_DIRECTION node, NODE_DIRECTION other)
    {
        return node == NODE_DIRECTION.NONE && other == NODE_DIRECTION.NONE || (node & other) != NODE_DIRECTION.NONE;
    }

    /// <summary>
    /// Assemble a node's entrances and exits into a single NODE_DIRECTION value.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static NODE_DIRECTION GetPathEntrancesAndExits(this Node node)
    {
        return node.In | node.Out;
    }

    /// <summary>
    /// Compile entrances and exits for all nodes in this list.
    /// </summary>
    /// <param name="nodeList"></param>
    /// <returns></returns>
    public static NODE_DIRECTION GetPathEntrancesAndExits(this List<Node> nodeList)
    {
        NODE_DIRECTION entrancesAndExits = NODE_DIRECTION.NONE;
        foreach (Node node in nodeList)
            entrancesAndExits |= node.GetPathEntrancesAndExits();

        return entrancesAndExits;
    }
}