using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents anything that is attackable by a tower.
/// </summary>
public class Attackable : MonoBehaviour
{
    public int HP;      // How much damage they can take.

    private Dictionary<int, TowerAggro> aggroTowers;
    private MinionPool owningPool;
    private MovementController movementController;

	// Use this for initialization
	public void Start () {
        aggroTowers = new Dictionary<int, TowerAggro>();
        movementController = transform.parent.GetComponent<MovementController>();
        gameObject.SetActive(false);
	}
	
	/// <summary>
    /// Check for death state and handle it if necessary.
    /// </summary>
	public void Update () {
		if (HP <= 0)
        {
            gameObject.SetActive(false);
        }
	}

    public void Activate(Node startNode)
    {
        movementController.Activate(startNode);
        gameObject.SetActive(true);
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
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Add a tower to the Tower tracking for this Attackable.
    /// </summary>
    /// <param name="tower"></param>
    public void TagTower(TowerAggro tower)
    {
        aggroTowers.Add(tower.ID, tower);
    }

    /// <summary>
    /// Remove this Attackable from the passed Tower.
    /// </summary>
    /// <param name="tower"></param>
    public void UnTagTower(TowerAggro tower)
    {
        aggroTowers.Remove(tower.ID);
    }

    /// <summary>
    /// Set this minion's pool owner.
    /// Can only be called once.
    /// </summary>
    /// <param name="pool"></param>
    public void SetPoolOwner(MinionPool pool)
    {
        if (owningPool == null) owningPool = pool;
    }

    public void OnEnable()
    {
        
    }

    public void OnDisable()
    {
        CancelInvoke();
        foreach (TowerAggro tower in aggroTowers.Values)
            tower.Targets.Remove(this);
        owningPool.SetInactive(this);
        movementController.Deactivate();
    }
}
