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

    public Vector2Int VectorLocation
    {
        get
        {
            return new Vector2Int(XLocation, YLocation);
        }
    }

    public NODE_DIRECTION In { get; set; }
    public NODE_DIRECTION Out { get; set; }

    public Node()
    {
        In = NODE_DIRECTION.NONE;
        Out = NODE_DIRECTION.NONE;
    }

    public Node(NODE_DIRECTION In, NODE_DIRECTION Out)
    {
        this.In = In;
        this.Out = Out;
    }
}

