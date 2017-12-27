using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// What is the purpose of the Path Builder?
/// The Path Builder produces the path portions of the map strings that the ChunkTexturePainter consumes to paint chunk textures.
/// 
/// The Path Builder uses a number of different path generation algorithms to generate paths.
/// Path algorithms must implement a standard interface, and are detected by the Path Builder at game boot using Reflection.
/// Paths are build without regard to chunk. However, the Path Builder must be able to fetch all path data in a rectangular area
/// for map generation purposes.
/// The underlying node structure produced by the Path Builder will be used by other systems to simulate un-painted chunks for
/// things such as minion-production and movement, and for the control of minions on painted chunks.
/// </summary>
public class PathBuilder
{
    private List<IHighLevelPathAlgorithm> highAlgorithms;
    private List<ILowLevelPathAlgorithm> lowAlgorithms;

    private Dictionary<Vector2Int, Node> pathNodes;
    private Queue<Node> edgeNodes;

    /// <summary>
    /// Path Node X, Y indexer.
    /// </summary>
    /// <param name="x">X loc of the node.</param>
    /// <param name="y">Y loc of the node.</param>
    /// <returns></returns>
    public Node this[int x, int y]
    {
        get
        {
            Vector2Int key = new Vector2Int(x, y);
            if (pathNodes.ContainsKey(key))
                return pathNodes[key];
            else return null;
        }
    }

    /// <summary>
    /// Path Node Vector2Int indexer.
    /// </summary>
    /// <param name="loc"></param>
    /// <returns></returns>
    public Node this[Vector2Int loc]
    {
        get
        {
            if (pathNodes.ContainsKey(loc))
                return pathNodes[loc];
            else return null;
        }
    }

    public PathBuilder()
    {
        highAlgorithms = DiscoverAlgotithmsByType<IHighLevelPathAlgorithm>(AlgorithmType.HIGH_LEVEL);
        lowAlgorithms = DiscoverAlgotithmsByType<ILowLevelPathAlgorithm>(AlgorithmType.LOW_LEVEL);
        pathNodes = new Dictionary<Vector2Int, Node>();
        edgeNodes = new Queue<Node>();
    }

    public PathBuilder(List<IHighLevelPathAlgorithm> highAlgorithms, List<ILowLevelPathAlgorithm> lowAlgorithms)
    {
        this.highAlgorithms = highAlgorithms;
        this.lowAlgorithms = lowAlgorithms;
        pathNodes = new Dictionary<Vector2Int, Node>();
        edgeNodes = new Queue<Node>();
    }

    /// <summary>
    /// Auto-Discover all path algorithm classes in this assembly.
    /// Changes will be needed if we decide to import algorithms from other assemblies for some reason.
    /// </summary>
    /// <param name="type">The algorithm type to return.</param>
    /// <returns>A list of algorithm types that satisfy the IPathAlgorithm contract.</returns>
    public static List<CastType> DiscoverAlgotithmsByType<CastType>(AlgorithmType type) where CastType : IPathAlgorithm
    {
        List<CastType> algos = new List<CastType>();

        foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
        {
            object[] attributes = t.GetCustomAttributes(typeof(PathAlgorithmAttribute), true);

            if (attributes.Length > 0)
            {
                PathAlgorithmAttribute pathAlgo = (PathAlgorithmAttribute)attributes[0];

                if (pathAlgo.Type == type && !pathAlgo.Ignore)
                {
                    object o = Activator.CreateInstance(t);
                    
                    if (o is CastType)
                        algos.Add((CastType)o);
                }
            }
        }

        return algos;
    }
}

/// <summary>
/// Represents the various algorithms that build paths.
/// All algorithms take in a start point and end point, and add new nodes directly to the pathNodes dictionary.
/// </summary>
public interface IPathAlgorithm
{
    void Reset();
    void Run();
}

public interface IHighLevelPathAlgorithm : IPathAlgorithm
{
    bool HasNext { get; }
    NodePointPair GetNext();
    void SetBounds(Node start, Rect bounds);
    List<Vector2Int> GetEndPoints();
}

public interface ILowLevelPathAlgorithm : IPathAlgorithm
{
    void SetPoints(Node start, Vector2Int end);
}

public class NodePointPair
{
    public Node Node { get; set; }
    public Vector2Int Point { get; set; }

    public NodePointPair() { }
    public NodePointPair(Node node, int x, int y)
    {
        Node = node;
        Point = new Vector2Int(x, y);
    }
}

/// <summary>
/// There are two algorithm types: High and Low level.
/// High level algorithms handle point placement and try to prevent blocking paths in.
/// Low level algorithms handle new node placement.
/// </summary>
public enum AlgorithmType { HIGH_LEVEL, LOW_LEVEL, TEST };

[AttributeUsage(AttributeTargets.Class)]
public class PathAlgorithmAttribute : Attribute
{
    public AlgorithmType Type { get; set; }
    public bool Ignore { get; set; }

    public PathAlgorithmAttribute(AlgorithmType type, bool ignore = false)
    {
        Type = type;
        Ignore = ignore;
    }
}

/// <summary>
/// Some simple utility methods used by the path builder.
/// </summary>
public static class Vector2Int_PathUtilExtensions
{
    public static Vector2Int GetUpLocation(this Vector2Int v)
    {
        return new Vector2Int(v.x, v.y + 1);
    }

    public static Vector2Int GetDownLocation(this Vector2Int v)
    {
        return new Vector2Int(v.x, v.y - 1);
    }

    public static Vector2Int GetLeftLocation(this Vector2Int v)
    {
        return new Vector2Int(v.x - 1, v.y);
    }

    public static Vector2Int GetRightLocation(this Vector2Int v)
    {
        return new Vector2Int(v.x + 1, v.y);
    }

    public static Vector2Int GetNorthLocation(this Vector2Int v)
    {
        return new Vector2Int(v.x, v.y + 1);
    }

    public static Vector2Int GetSouthLocation(this Vector2Int v)
    {
        return new Vector2Int(v.x, v.y - 1);
    }

    public static Vector2Int GetWestLocation(this Vector2Int v)
    {
        return new Vector2Int(v.x - 1, v.y);
    }

    public static Vector2Int GetEastLocation(this Vector2Int v)
    {
        return new Vector2Int(v.x + 1, v.y);
    }

    /// <summary>
    /// Lossy vector2 => vector2int conversion. Floats are cast to ints.
    /// Intended to convert vector2 values that were originally vector2int values.
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector2Int ToVector2Int(this Vector2 v)
    {
        return new Vector2Int((int)v.x, (int)v.y);
    }
}

public static class Node_PathUtilExtensions
{
}