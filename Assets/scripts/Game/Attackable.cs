using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents anything that is attackable by a tower.
/// </summary>
public class Attackable : MonoBehaviour
{
    public int HP;      // How much damage they can take.
    public int MaxHp;   // Their maximum HP

    public Dictionary<int, TowerAggro> AggroTowers = new Dictionary<int, TowerAggro>();

	// Use this for initialization
	public void Start ()
    {

	}
	
	public void Update ()
    {

	}

    /// <summary>
    /// Deal damage to this minion.
    /// </summary>
    /// <param name="damage"></param>
    /// <returns>True if the minion was killed by the damage.</returns>
    public bool DealDamage(int damage)
    {
        HP -= damage;

        return HP <= 0;
    }

    /// <summary>
    /// Add a tower to the Tower tracking for this Attackable.
    /// </summary>
    /// <param name="tower"></param>
    public void TagTower(TowerAggro tower)
    {
        AggroTowers.Add(tower.ID, tower);
    }

    /// <summary>
    /// Remove this Attackable from the passed Tower.
    /// </summary>
    /// <param name="tower"></param>
    public void UnTagTower(TowerAggro tower)
    {
        AggroTowers.Remove(tower.ID);
    }
}
