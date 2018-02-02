using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class TestMinionMovement
{

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator TestMinionMovement_Straight()
    {
        SceneManager.LoadScene("Test_MinionMovement_Straight");

		yield return null;
        Node origin = new Node(0, 0);
        Path path = new Path(origin, NODE_DIRECTION.RIGHT); //1

        Branch testBranch = path.Branches[path.OuterBranchIDs[0]];
        testBranch.Add(NODE_DIRECTION.RIGHT); //2
        testBranch.Add(NODE_DIRECTION.RIGHT); //3
        testBranch.Add(NODE_DIRECTION.RIGHT); //4

        Node startNode = testBranch.InNode;
        Assert.That(startNode.VectorLocation, Is.EqualTo(new Vector2Int(4, 0)));

        Minion minion = MinionPool.Instance.GetMinion();

        minion.Activate(startNode);

        float travelDuration = minion.MovementInterface.MoveInterval * Node.GetDistanceToOrigin(startNode) + 1f;

        yield return new WaitForSeconds(travelDuration);
        Assert.That(minion.transform.position.RoundToVector2Int(), Is.EqualTo(origin.VectorLocation));
	}

    [UnityTest]
    public IEnumerator TestMinionMovement_Turn_PositionAndRotation()
    {
        SceneManager.LoadScene("Test_MinionMovement_Turn");

        yield return null;
        Node origin = new Node(0, 0);
        Path path = new Path(origin, NODE_DIRECTION.RIGHT); //1

        Branch testBranch = path.Branches[path.OuterBranchIDs[0]];
        testBranch.Add(NODE_DIRECTION.RIGHT);   //2
        testBranch.Add(NODE_DIRECTION.RIGHT);   //3
        testBranch.Add(NODE_DIRECTION.RIGHT);   //4
        testBranch.Add(NODE_DIRECTION.UP);      //5
        testBranch.Add(NODE_DIRECTION.UP);      //6
        testBranch.Add(NODE_DIRECTION.UP);      //7
        testBranch.Add(NODE_DIRECTION.UP);      //8

        Node startNode = testBranch.InNode;
        Assert.That(startNode.VectorLocation, Is.EqualTo(new Vector2Int(4, 4)));

        Minion minion = MinionPool.Instance.GetMinion();
        minion.Activate(startNode);

        float travelDuration = minion.MovementInterface.MoveInterval * Node.GetDistanceToOrigin(startNode) + 1f;

        yield return new WaitForSeconds(travelDuration);
        Assert.That(minion.transform.position.RoundToVector2Int(), Is.EqualTo(origin.VectorLocation));
        Assert.That(minion.transform.rotation, Is.EqualTo(Quaternion.Euler(0, MovementController.GetDirectionRotation(NODE_DIRECTION.LEFT), 0)));
    }
}
