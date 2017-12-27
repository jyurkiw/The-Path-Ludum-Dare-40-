using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Bunch of Classes used for testing.
/// Don't touch these unless you know what you're doing.
/// </summary>

[PathAlgorithm(AlgorithmType.TEST)]
public class TestAlgorithmFailure1
{

}

public class TestAlgorithmFailure2
{

}

[PathAlgorithm(AlgorithmType.TEST)]
public class TestAlgorithmSuccess : IPathAlgorithm
{
    public bool HasNext
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public List<Vector2Int> GetEndPoints()
    {
        throw new NotImplementedException();
    }

    public NodePointPair GetNext()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }

    public void Run()
    {
        throw new System.NotImplementedException();
    }

    public void SetBounds(Node start, Rect bounds)
    {
        throw new NotImplementedException();
    }

    public void SetPoints(Node start, Vector2Int end)
    {
        throw new NotImplementedException();
    }
}

[PathAlgorithm(AlgorithmType.TEST)]
public class TestHighAlgorithmSuccess : IHighLevelPathAlgorithm
{
    public bool HasNext
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public List<Vector2Int> GetEndPoints()
    {
        throw new NotImplementedException();
    }

    public NodePointPair GetNext()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void Run()
    {
        throw new NotImplementedException();
    }

    public void SetBounds(Node start, Rect bounds)
    {
        throw new NotImplementedException();
    }
}

[PathAlgorithm(AlgorithmType.TEST)]
public class TestLowAlgorithmSuccess : ILowLevelPathAlgorithm
{
    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void Run()
    {
        throw new NotImplementedException();
    }

    public void SetPoints(Node start, Vector2Int end)
    {
        throw new NotImplementedException();
    }
}

[PathAlgorithm(AlgorithmType.TEST, true)]
public class TestAlgorithmIgnore : IPathAlgorithm
{
    public bool HasNext
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public List<Vector2Int> GetEndPoints()
    {
        throw new NotImplementedException();
    }

    public NodePointPair GetNext()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void Run()
    {
        throw new NotImplementedException();
    }

    public void SetBounds(Node start, Rect bounds)
    {
        throw new NotImplementedException();
    }

    public void SetPoints(Node start, Vector2Int end)
    {
        throw new NotImplementedException();
    }
}

/*
 * Test Algorithms
 * 
 * The following algorithms are ment for basic testing, and should have their ignore attributes set to true
 * once basic algorithm system development is complete.
 */

[PathAlgorithm(AlgorithmType.HIGH_LEVEL, ignore: true)]
public class StraightLineHighLevelAlgorithm : IHighLevelPathAlgorithm
{
    private Rect Bounds { get; set; }
    private Node WellNode { get; set; }

    private Stack<NodePointPair> nodePoints = new Stack<NodePointPair>();
    private List<Vector2Int> endPoints;

    public bool HasNext
    {
        get
        {
            return nodePoints.Count > 0;
        }
    }

    public void Reset()
    {
        Bounds = new Rect();
        WellNode = null;
        nodePoints = new Stack<NodePointPair>();
        endPoints = null;
    }

    /// <summary>
    /// Create points for low-level feeding in a straight line out to the edge of the bounds in all four cardinal directions.
    /// </summary>
    public void Run()
    {
        // North
        nodePoints.Push(new NodePointPair(WellNode, WellNode.Location.x, (int)Bounds.yMax));

        // South
        nodePoints.Push(new NodePointPair(WellNode, WellNode.Location.x, (int)Bounds.yMin));

        // East
        nodePoints.Push(new NodePointPair(WellNode, (int)Bounds.xMax, WellNode.Location.y));

        // West
        nodePoints.Push(new NodePointPair(WellNode, (int)Bounds.xMin, WellNode.Location.y));

        endPoints = nodePoints.Select(x => x.Point).ToList();
    }

    public void SetBounds(Node start, Rect bounds)
    {
        WellNode = start;
        Bounds = bounds;
    }

    public void SetPoints(Node start, Vector2Int end)
    {
        throw new NotImplementedException();
    }

    public NodePointPair GetNext()
    {
        return nodePoints.Pop();
    }

    public List<Vector2Int> GetEndPoints()
    {
        return endPoints;
    }
}

[PathAlgorithm(AlgorithmType.LOW_LEVEL, ignore: true)]
public class StraightLineLowLevelAlgorithm : ILowLevelPathAlgorithm
{
    private Node StartNode { get; set; }
    private Node CurrNode;
    private Vector2Int Destination { get; set; }

    private int xMod, yMod;

    public bool HasNext
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public NodePointPair GetNext()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        StartNode = null;
        CurrNode = null;
        Destination = Vector2Int.zero;
    }

    public void Run()
    {
        while(CurrNode.Location != Destination)
        {
            int nX = CurrNode.Location.x + xMod;
            int nY = CurrNode.Location.y + yMod;

            Node newNode = new Node(new Vector2Int(nX, nY), CurrNode);
            CurrNode = newNode;
        }
    }

    public void SetBounds(Node start, Rect bounds)
    {
        throw new NotImplementedException();
    }

    public void SetPoints(Node start, Vector2Int end)
    {
        Reset();

        StartNode = start;
        CurrNode = start;
        Destination = end;

        // Set xmod and ymod values
        if (StartNode.Location.x == Destination.x)
        {
            xMod = 0;
            if (StartNode.Location.y < Destination.y)
            {
                yMod = 1;
            }
            else
            {
                yMod = -1;
            }
        }
        else
        {
            yMod = 0;
            if (StartNode.Location.x < Destination.x)
            {
                xMod = 1;
            }
            else
            {
                xMod = -1;
            }
        }
    }

    public List<Vector2Int> GetEndPoints()
    {
        throw new NotImplementedException();
    }
}