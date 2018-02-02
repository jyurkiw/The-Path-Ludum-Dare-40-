using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents anything that is attackable by a tower.
/// </summary>
public class Attackable : MonoBehaviour
{
    public int HP;      // How much damage they can take.

    public bool Alive { get { return HP > 0; } }

    private Dictionary<int, TowerAggro> aggroTowers;

	// Use this for initialization
	public void Start () {
        aggroTowers = new Dictionary<int, TowerAggro>();
	}
	
	/// <summary>
    /// Check for death state and handle it if necessary.
    /// </summary>
	public void Update () {
		if (HP <= 0)
        {
            Destroy(this.transform.parent.gameObject);
        }
	}

    /// <summary>
    /// Deal damage to this minion and destroy if HP <= 0.
    /// </summary>
    /// <param name="damage"></param>
    public void DealDamage(int damage)
    {
        HP -= damage;

        if (HP <= 0)
        {
            foreach (TowerAggro tower in aggroTowers.Values) tower.Targets.Remove(this);
            Destroy(this.transform.parent.gameObject);
        }
    }

    public void TagTower(TowerAggro tower)
    {
        aggroTowers.Add(tower.ID, tower);
    }

    public void UnTagTower(TowerAggro tower)
    {
        aggroTowers.Remove(tower.ID);
    }
}
