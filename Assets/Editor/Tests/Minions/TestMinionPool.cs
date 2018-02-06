using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestMinionPool {

	[Test]
	public void Test_MinionPool_CreateNew()
    {
        // Set KeepOnLoad to false so we don't crash the testing framework
        MinionPool.KeepOnLoad = false;

        // Create a local MinionPool
        MinionPool pool = new MinionPool(1);

        Assert.That(pool.Minions.Count, Is.EqualTo(1));

        Minion minion = pool.GetMinion();

        Assert.That(pool.Minions.Count, Is.EqualTo(1));

        minion.Deactivate();

        Assert.That(pool.Minions.Count, Is.EqualTo(2));
	}

    [Test]
    public void Test_MinionPool_GetSameMinion()
    {
        // Set KeepOnLoad to false so we don't crash the testing framework
        MinionPool.KeepOnLoad = false;

        // Create a local MinionPool
        MinionPool pool = new MinionPool(2);

        Minion minion1 = pool.GetMinion();
        
        // Deactivate 
        minion1.Deactivate();

        // GetMinion should return the same minion
        Minion minion2 = pool.GetMinion();

        Assert.That(minion1.ID, Is.EqualTo(minion2.ID));
    }

    [Test]
    public void Test_MinionPool_ResetDamageOnRespawn()
    {
        // Set KeepOnLoad to false so we don't crash the testing framework
        MinionPool.KeepOnLoad = false;

        // Create a local MinionPool
        MinionPool pool = new MinionPool(2);

        Minion minion = pool.GetMinion();

        int originalHP = minion.AttackableInterface.HP;

        // Deal damage
        minion.DealDamage(4);

        // Deactivate 
        minion.Deactivate();

        minion = pool.GetMinion(); // This should be the same minion

        Assert.That(minion.AttackableInterface.HP, Is.EqualTo(originalHP));
    }
}
