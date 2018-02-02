using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class TestTowerAttacks
{
    [UnityTest]
    public IEnumerator Test_TowerAttacks_MinionsDieCorrectly()
    {
        SceneManager.LoadScene("Test_TowerAttacks_Aggro");
        yield return null;

        GameObject tower = GameObject.Find("test_tower");
        TowerAggro towerAgg = tower.GetComponentInChildren<TowerAggro>();

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

        float secondMinionSpawnDelay = 2.0f;
        yield return new WaitForSeconds(secondMinionSpawnDelay);

        MovementController minion2 = GameObject.Instantiate<MovementController>(Resources.Load<MovementController>("prefabs/enemy/enemy"));
        minion2.Activate(startNode);

        float travelDuration = minion.MoveInterval * Node.GetDistanceToOrigin(startNode) + 1f - secondMinionSpawnDelay;

        yield return new WaitForSeconds(travelDuration);
        Assert.That(towerAgg.Targets.Count, Is.EqualTo(0));
        Assert.That(minion == null);
    }

    [UnityTest]
    public IEnumerator Test_ProjectileBehavior()
    {
        SceneManager.LoadScene("Test_Attacks");
        yield return null;

        GameObject tower = GameObject.Find("test_tower");
        TowerAggro towerAgg= tower.GetComponentInChildren<TowerAggro>();
        Attackable enemy = GameObject.Find("enemy").GetComponentInChildren<Attackable>();

        towerAgg.Targets.Add(enemy);

        yield return new WaitForSeconds(6);

        Projectile[] p = GameObject.FindObjectsOfType<Projectile>();
        Assert.That(p.Length, Is.EqualTo(1));
        Assert.That(enemy.HP, Is.EqualTo(10));

        yield return new WaitForSeconds(3);

        Assert.That(enemy.HP, Is.EqualTo(9));
    }
}
