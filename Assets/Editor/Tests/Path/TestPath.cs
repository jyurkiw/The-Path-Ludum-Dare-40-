﻿using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestPath {
    private Node origin = new Node(2, 2);

	[Test]
	public void Test_Path_IsNodeAt_TestOrigin()
    {
        Path path = new Path(origin, NODE_DIRECTION.UP);


        Assert.That(path.IsNodeAt(origin.XLocation, origin.YLocation));
	}

    [Test]
    public void Test_Path_IsNodeAt_TestInitialBranch()
    {
        Path path = new Path(origin, NODE_DIRECTION.UP);

        Assert.That(path.IsNodeAt(2, 3));
    }

    [Test]
    public void Test_Path_Init_BranchCount()
    {
        Path path = new Path(origin, NODE_DIRECTION.UP);

        Assert.That(path.Count, Is.EqualTo(NODE_DIRECTION.UP.GetDirections().ToArray().Length));
    }

    [Test]
    public void Test_Path_Init_BranchCount4()
    {
        Path path = new Path(origin, NODE_DIRECTION.UP_DOWN_LEFT_RIGHT);

        Assert.That(path.Count, Is.EqualTo(NODE_DIRECTION.UP_DOWN_LEFT_RIGHT.GetDirections().ToArray().Length));
    }

    [Test]
    public void Test_Path_Init_OuterBranchIDsCount()
    {
        Path path = new Path(origin, NODE_DIRECTION.UP_DOWN_LEFT_RIGHT);

        Assert.That(path.OuterBranchIDs.Count, Is.EqualTo(NODE_DIRECTION.UP_DOWN_LEFT_RIGHT.GetDirections().ToArray().Length));
    }

    [Test]
    public void Test_Path_Init_GetNodesAt()
    {
        NODE_DIRECTION initDir = NODE_DIRECTION.UP_LEFT_RIGHT;
        Path path = new Path(origin, initDir);

        foreach(int id in path.OuterBranchIDs)
        {
            Branch outerBranch = path.Branches[id];
            switch (origin.DirectionOf(outerBranch.InNode))
            {
                case NODE_DIRECTION.LEFT:
                    outerBranch.Add(NODE_DIRECTION.UP);
                    outerBranch.Add(NODE_DIRECTION.RIGHT);
                    break;
                case NODE_DIRECTION.RIGHT:
                    outerBranch.Add(NODE_DIRECTION.UP);
                    outerBranch.Add(NODE_DIRECTION.LEFT);
                    break;
                default:
                    break;
            }
        }

        Assert.That(path.GetNodesAt(2, 3).Length, Is.EqualTo(initDir.GetDirections().ToArray().Length));
    }

    [Test]
    public void Test_Path_GetNodesAt_NoNodeExists()
    {
        NODE_DIRECTION initDir = NODE_DIRECTION.UP;
        Path path = new Path(origin, initDir);

        Assert.That(path.GetNodesAt(0, 0).Length, Is.EqualTo(0));
    }

    [Test]
    public void Test_Path_Init_GetNodeAt()
    {
        NODE_DIRECTION initDir = NODE_DIRECTION.UP;
        Path path = new Path(origin, initDir);
        Branch outerBranch = path.Branches[path.OuterBranchIDs[0]];

        Vector2Int upLoc = origin.Next(initDir);

        Assert.That(path.GetNodeAt(upLoc.x, upLoc.y, path.OuterBranchIDs[0]).VectorLocation, Is.EqualTo(upLoc));
    }

    [Test]
    public void Test_Path_Branch_OpenException()
    {
        NODE_DIRECTION initDir = NODE_DIRECTION.UP, branchDir = NODE_DIRECTION.UP_LEFT_RIGHT;
        Path path = new Path(origin, initDir);
        int initBranchId = path.OuterBranchIDs[0];

        Assert.Throws<PathException>(() => path.Branch(initBranchId));
    }

    [Test]
    public void Test_Path_Branch_PostCount()
    {
        NODE_DIRECTION initDir = NODE_DIRECTION.UP, branchDir = NODE_DIRECTION.UP_LEFT_RIGHT;
        Path path = new Path(origin, initDir);
        int initBranchId = path.OuterBranchIDs[0];

        Branch initBranch = path.Branches[initBranchId];
        initBranch.Add(branchDir);

        path.Branch(initBranchId);

        Assert.That(path.Count, Is.EqualTo(4));
    }

    [Test]
    public void Test_Path_Branch_PostOuterBranchIDCount()
    {
        NODE_DIRECTION initDir = NODE_DIRECTION.UP, branchDir = NODE_DIRECTION.UP_LEFT_RIGHT;
        Path path = new Path(origin, initDir);
        int initBranchId = path.OuterBranchIDs[0];

        Branch initBranch = path.Branches[initBranchId];
        initBranch.Add(branchDir);

        path.Branch(initBranchId);

        Assert.That(path.OuterBranchIDs.Count, Is.EqualTo(3));
    }

    [Test]
    public void Test_Path_Branch_PostOuterBranchIDGone()
    {
        NODE_DIRECTION initDir = NODE_DIRECTION.UP, branchDir = NODE_DIRECTION.UP_LEFT_RIGHT;
        Path path = new Path(origin, initDir);
        int initBranchId = path.OuterBranchIDs[0];

        Branch initBranch = path.Branches[initBranchId];
        initBranch.Add(branchDir);

        path.Branch(initBranchId);

        Assert.That(path.OuterBranchIDs, !Contains.Item(initBranchId));
    }

    [Test]
    public void Test_Path_Branch_WalkBackThroughNodesOnly()
    {
        // build a short path (one step up)
        Path path = new Path(origin, NODE_DIRECTION.UP);

        // now branch left
        int initBranchId = path.OuterBranchIDs[0];
        Branch initBranch = path.Branches[initBranchId];
        initBranch.Add(NODE_DIRECTION.UP_LEFT);

        path.Branch(initBranchId);

        Node outerNode = path.Branches.Where(x => x.Value.InNode.OutDirection == NODE_DIRECTION.RIGHT).Select(x => x.Value.InNode).First();

        // Should be the node up from the origin.
        Node trackNode = outerNode.OutNode;

        // Should be the origin.
        trackNode = trackNode.OutNode;

        Assert.That(trackNode.VectorLocation, Is.EqualTo(origin.VectorLocation));
    }

    [Test]
    public void Test_Path_OriginNodeShouldBeUDLR()
    {
        Path path = new Path(origin, NODE_DIRECTION.UP_DOWN_LEFT_RIGHT);

        NODE_DIRECTION originDirection = path.GetNodesAt(2, 2).First().InDirection;

        Assert.That(originDirection, Is.EqualTo(NODE_DIRECTION.UP_DOWN_LEFT_RIGHT));
    }
}
