using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TestStraightLineTestAlgorithms {

	[Test]
	public void TestStraightLine_HighLevel()
    {
        IHighLevelPathAlgorithm algo = new StraightLineHighLevelAlgorithm();

        Node wellNode = new Node(Vector2Int.zero);
        Rect boundingRect = new Rect(-4, -4, 8, 8);

        Vector2Int testPointNorth = new Vector2Int(0, 4);
        Vector2Int testPointSouth = new Vector2Int(0, -4);
        Vector2Int testPointEast = new Vector2Int(-4, 0);
        Vector2Int testPointWest = new Vector2Int(4, 0);

        algo.SetBounds(wellNode, boundingRect);
        algo.Run();

        List<Vector2Int> pointList = new List<Vector2Int>();
        while (algo.HasNext) pointList.Add(algo.GetNext().Point);

        Assert.AreEqual(4, pointList.Count);
        Assert.Contains(testPointNorth, pointList);
        Assert.Contains(testPointSouth, pointList);
        Assert.Contains(testPointEast, pointList);
        Assert.Contains(testPointWest, pointList);
    }

    [Test]
    public void TestStraightLine_LowLevel()
    {
        ILowLevelPathAlgorithm algo = new StraightLineLowLevelAlgorithm();
        IHighLevelPathAlgorithm hlAlgo = new StraightLineHighLevelAlgorithm();

        Node wellNode = new Node(Vector2Int.zero);
        Rect boundingRect = new Rect(-4, -4, 8, 8);
        hlAlgo.SetBounds(wellNode, boundingRect);
        hlAlgo.Run();

        while (hlAlgo.HasNext)
        {
            NodePointPair next = hlAlgo.GetNext();


            algo.SetPoints(next.Node, next.Point);
            algo.Run();
        }

        Assert.AreEqual(4, wellNode.Connections.Count);

        foreach(NodeConnection immediateNeighborConnection in wellNode.Connections)
        {
            Vector2Int direction = immediateNeighborConnection.Connection.Location;
            Node neighbor1 = immediateNeighborConnection.Connection;
            Assert.AreEqual(2, neighbor1.Connections.Count);

            Node neighbor2 = neighbor1.Connections.Where(x=>x.Type == NodeConnection.ConnectionType.INCOMMING).ToList()[0].Connection;
            Assert.AreEqual(2, neighbor2.Connections.Count);

            Node neighbor3 = neighbor2.Connections.Where(x => x.Type == NodeConnection.ConnectionType.INCOMMING).ToList()[0].Connection;
            Assert.AreEqual(2, neighbor3.Connections.Count);

            Node neighbor4 = neighbor3.Connections.Where(x => x.Type == NodeConnection.ConnectionType.INCOMMING).ToList()[0].Connection;
            Assert.AreEqual(1, neighbor4.Connections.Count);
        }
    }
}
