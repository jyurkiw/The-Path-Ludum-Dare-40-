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
    DOWN                = 2,
    LEFT                = 4,
    RIGHT               = 8,

    UP_DOWN             = UP | DOWN,
    UP_DOWN_LEFT        = UP | DOWN | LEFT,
    UP_DOWN_RIGHT       = UP | DOWN | RIGHT,
    UP_DOWN_LEFT_RIGHT  = UP | DOWN | LEFT | RIGHT,
    UP_LEFT             = UP | LEFT,
    UP_LEFT_RIGHT       = UP | LEFT | RIGHT,
    UP_RIGHT            = UP | RIGHT,

    DOWN_LEFT           = DOWN | LEFT,
    DOWN_RIGHT          = DOWN | RIGHT,
    DOWN_LEFT_RIGHT     = DOWN | LEFT | RIGHT,

    LEFT_RIGHT          = LEFT | RIGHT
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
        return node == NODE_DIRECTION.NONE && other == NODE_DIRECTION.NONE || (node & other) != 0;
    }

    /// <summary>
    /// Assemble a node's entrances and exits into a single NODE_DIRECTION value.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static NODE_DIRECTION GetPathEntrancesAndExits(this Node node)
    {
        return node.InDirection | node.OutDirection;
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

    /// <summary>
    /// Break a multi-direction into individual NODE_DIRECTIONs.
    /// </summary>
    /// <param name="multiDirection"></param>
    /// <returns></returns>
    public static IEnumerable<NODE_DIRECTION> GetDirections(this NODE_DIRECTION multiDirection)
    {
        NODE_DIRECTION[] basicDirectionSet = { NODE_DIRECTION.UP, NODE_DIRECTION.DOWN, NODE_DIRECTION.LEFT, NODE_DIRECTION.RIGHT };

        foreach(NODE_DIRECTION direction in basicDirectionSet)
        {
            if (multiDirection.CheckFlag(direction))
            {
                yield return direction;
            }
        }

        yield break;
    }

    /// <summary>
    /// Test a direction to see if it is a multi-direction or not.
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    public static bool IsMulti(this NODE_DIRECTION d)
    {
        return !(
            d == NODE_DIRECTION.NONE ||
            d == NODE_DIRECTION.UP ||
            d == NODE_DIRECTION.DOWN ||
            d == NODE_DIRECTION.LEFT ||
            d == NODE_DIRECTION.RIGHT);
    }
}

/// <summary>
/// The type of direction exception.
/// There should only be two.
/// </summary>
public enum DIRECTION_EXCEPTION_TYPE
{
    MULTI_RESTRICTED,
    SINGLE_RESTRICTED
}

/// <summary>
/// An exception based on weather or not we expect or allow multi-direction NODE_DIRECTIONS.
/// </summary>
public class DirectionException : Exception
{
    public DirectionException(DIRECTION_EXCEPTION_TYPE type) : base(
        type == DIRECTION_EXCEPTION_TYPE.MULTI_RESTRICTED ?
        "Multi-direction expected. Found NONE or single-direction instead." :
        "Single-direction expected. Found multi-direction instead."
        )
    { }
}