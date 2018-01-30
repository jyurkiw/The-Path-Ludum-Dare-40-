using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class TestTowerAttacks
{
    [UnityTest]
    public IEnumerator Test_TowerAttacks_AggroEstablish()
    {
        SceneManager.LoadScene("Test_TowerAttacks_Aggro");

        yield return null;
        Node origin = new Node(0, 0);
        Path path = new Path(origin, NODE_DIRECTION.RIGHT); //1

        Branch testBranch = path.Branches[path.OuterBranchIDs[0]];
        testBranch.Add(NODE_DIRECTION.RIGHT); //2
        testBranch.Add(NODE_DIRECTION.RIGHT); //3
        testBranch.Add(NODE_DIRECTION.RIGHT); //4
        testBranch.Add(NODE_DIRECTION.RIGHT); //5
        testBranch.Add(NODE_DIRECTION.RIGHT); //6
        testBranch.Add(NODE_DIRECTION.RIGHT); //7
        testBranch.Add(NODE_DIRECTION.RIGHT); //8
        testBranch.Add(NODE_DIRECTION.RIGHT); //9
        testBranch.Add(NODE_DIRECTION.RIGHT); //10

        Node startNode = testBranch.InNode;

        MovementController minion = GameObject.Instantiate<MovementController>(Resources.Load<MovementController>("prefabs/enemy/enemy"));
        minion.Activate(startNode);

        float travelDuration = minion.MoveInterval * Node.GetDistanceToOrigin(startNode) + 1f;

        yield return new WaitForSeconds(travelDuration);
        Assert.That(minion.transform.position.RoundToVector2Int(), Is.EqualTo(origin.VectorLocation));
    }

    [UnityTest]
    public IEnumerator Test_TowerAttacks_AggroBreakOnDistance()
    {
        yield return null;
        Assert.Fail();
    }
}
