using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TestTowerAggro
{
    [Test]
    public void Test_CompareDistance_NegOne()
    {
        MinionPool.KeepOnLoad = false;
        MinionPool.Reset();

        Minion enemy1 = MinionPool.Instance.GetMinion();
        Minion enemy2 = MinionPool.Instance.GetMinion();

        enemy1.gameObject.SetActive(true);
        enemy2.gameObject.SetActive(true);

        enemy1.transform.SetPositionAndRotation(new Vector3(2, 0, 2), Quaternion.identity);
        enemy2.transform.SetPositionAndRotation(new Vector3(4, 0, 4), Quaternion.identity);

        GameObject tower = new GameObject();
        TowerAggro towerAggro = tower.AddComponent<TowerAggro>();

        int distanceSortValue = towerAggro.CompareDistances(enemy1, enemy2);

        Assert.That(distanceSortValue, Is.EqualTo(-1));
    }

    [Test]
    public void Test_CompareDistance_Zero()
    {
        MinionPool.KeepOnLoad = false;
        MinionPool.Reset();

        Minion enemy1 = MinionPool.Instance.GetMinion();
        Minion enemy2 = MinionPool.Instance.GetMinion();

        enemy1.gameObject.SetActive(true);
        enemy2.gameObject.SetActive(true);

        enemy1.transform.SetPositionAndRotation(new Vector3(2, 0, 2), Quaternion.identity);
        enemy2.transform.SetPositionAndRotation(new Vector3(2, 0, 2), Quaternion.identity);

        GameObject tower = new GameObject();
        TowerAggro towerAggro = tower.AddComponent<TowerAggro>();

        int distanceSortValue = towerAggro.CompareDistances(enemy1, enemy2);

        Assert.That(distanceSortValue, Is.EqualTo(0));
    }

    [Test]
    public void Test_CompareDistance_PosOne()
    {
        MinionPool.KeepOnLoad = false;
        MinionPool.Reset();

        Minion enemy1 = MinionPool.Instance.GetMinion();
        Minion enemy2 = MinionPool.Instance.GetMinion();

        enemy1.gameObject.SetActive(true);
        enemy2.gameObject.SetActive(true);

        enemy1.transform.SetPositionAndRotation(new Vector3(8, 0, 8), Quaternion.identity);
        enemy2.transform.SetPositionAndRotation(new Vector3(4, 0, 4), Quaternion.identity);

        GameObject tower = new GameObject();
        TowerAggro towerAggro = tower.AddComponent<TowerAggro>();

        int distanceSortValue = towerAggro.CompareDistances(enemy1, enemy2);

        Assert.That(distanceSortValue, Is.EqualTo(1));
    }

    [Test]
    public void Test_CompareDistance_PosOne2()
    {
        MinionPool.KeepOnLoad = false;
        MinionPool.Reset();

        Minion enemy1 = MinionPool.Instance.GetMinion();
        Minion enemy2 = MinionPool.Instance.GetMinion();

        enemy1.gameObject.SetActive(true);
        enemy2.gameObject.SetActive(true);

        enemy1.transform.SetPositionAndRotation(new Vector3(8, 0, -8), Quaternion.identity);
        enemy2.transform.SetPositionAndRotation(new Vector3(-4, 0, 4), Quaternion.identity);

        GameObject tower = new GameObject();
        TowerAggro towerAggro = tower.AddComponent<TowerAggro>();

        int distanceSortValue = towerAggro.CompareDistances(enemy1, enemy2);

        Assert.That(distanceSortValue, Is.EqualTo(1));
    }

    [Test]
    public void Test_AddTarget()
    {
        MinionPool.KeepOnLoad = false;
        MinionPool.Reset();

        Minion enemy = MinionPool.Instance.GetMinion();
        enemy.gameObject.SetActive(true);
        SphereCollider enemyCollider = enemy.AttackableInterface.GetComponent<SphereCollider>();
            
        enemy.transform.SetPositionAndRotation(new Vector3(8, 0, -8), Quaternion.identity);

        GameObject tower = new GameObject();
        TowerAggro towerAggro = tower.AddComponent<TowerAggro>();
        
        towerAggro.Start();
        towerAggro.OnTriggerEnter(enemyCollider);

        Assert.That(towerAggro.Targets.Count, Is.EqualTo(1));
    }

    [Test]
    public void Test_LoseTarget()
    {
        MinionPool.KeepOnLoad = false;
        MinionPool.Reset();

        Minion enemy = MinionPool.Instance.GetMinion();
        enemy.gameObject.SetActive(true);
        SphereCollider enemyCollider = enemy.AttackableInterface.GetComponent<SphereCollider>();

        enemy.transform.SetPositionAndRotation(new Vector3(8, 0, -8), Quaternion.identity);

        enemyCollider.radius = 0.1f;

        GameObject tower = new GameObject();
        TowerAggro towerAggro = tower.AddComponent<TowerAggro>();
        SphereCollider towerCollider = tower.AddComponent<SphereCollider>();

        towerCollider.radius = 4;
        towerAggro.Start();
        towerAggro.OnTriggerEnter(enemyCollider);

        towerAggro.OnTriggerExit(enemyCollider);

        Assert.That(towerAggro.Targets.Count, Is.EqualTo(0));
    }

    [Test]
    public void Test_GetTarget()
    {
        MinionPool.KeepOnLoad = false;
        MinionPool.Reset();

        Minion enemy = MinionPool.Instance.GetMinion();
        enemy.gameObject.SetActive(true);
        SphereCollider enemyCollider = enemy.AttackableInterface.GetComponent<SphereCollider>();

        enemy.transform.SetPositionAndRotation(new Vector3(8, 0, -8), Quaternion.identity);
        enemyCollider.radius = 0.1f;

        GameObject tower = new GameObject();
        TowerAggro towerAggro = tower.AddComponent<TowerAggro>();
        towerAggro.Start();
        SphereCollider towerCollider = tower.AddComponent<SphereCollider>();

        towerCollider.radius = 4;
        towerAggro.Start();
        towerAggro.OnTriggerEnter(enemyCollider);

        enemy = MinionPool.Instance.GetMinion();
        enemy.gameObject.SetActive(true);
        enemyCollider = enemy.AttackableInterface.GetComponent<SphereCollider>();

        enemy.transform.SetPositionAndRotation(new Vector3(4, 0, 4), Quaternion.identity);
        enemyCollider.radius = 0.1f;
        towerAggro.OnTriggerEnter(enemyCollider);

        enemy = MinionPool.Instance.GetMinion();
        enemy.gameObject.SetActive(true);
        enemyCollider = enemy.AttackableInterface.GetComponent<SphereCollider>();

        enemy.transform.SetPositionAndRotation(new Vector3(2, 0, 4), Quaternion.identity);
        enemyCollider.radius = 0.1f;
        towerAggro.OnTriggerEnter(enemyCollider);

        enemy = MinionPool.Instance.GetMinion();
        enemy.gameObject.SetActive(true);
        enemyCollider = enemy.AttackableInterface.GetComponent<SphereCollider>();

        enemy.transform.SetPositionAndRotation(new Vector3(1, 0, 1), Quaternion.identity);
        enemyCollider.radius = 0.1f;
        towerAggro.OnTriggerEnter(enemyCollider);

        Minion target = towerAggro.GetTarget();

        Assert.That(target.GetInstanceID(), Is.EqualTo(enemy.GetInstanceID()));
    }
}
