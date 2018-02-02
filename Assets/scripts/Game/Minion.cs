using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface script that sits on the minion gimble and bridges the parent-child relationship to the Attackable script.
/// </summary>
public class Minion : MonoBehaviour
{
    public int ID = 0;
    public Attackable AttackableInterface { get; private set; }
    public MovementController MovementInterface { get; private set; }

    private MinionPool owningPool;

    public bool Alive { get { return gameObject.activeSelf; } }
    
    /// <summary>
    /// Initialize the Attackable and Movement interfaces.
    /// Also set the MinionPool owner.
    /// </summary>
    /// <param name="owner"></param>
    public void InitInterfaces(MinionPool owner)
    {
        AttackableInterface = GetComponentInChildren<Attackable>();
        MovementInterface = GetComponent<MovementController>();
        owningPool = owner;
    }

    /// <summary>
    /// Activate this minion at the passed node.
    /// </summary>
    /// <param name="startNode"></param>
    public void Activate(Node startNode)
    {
        MovementInterface.Activate(startNode);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Deactivate this minion (they stay where they currently are.
    /// </summary>
    public void Deactivate()
    {
        gameObject.SetActive(false);
        owningPool.SetInactive(this);

        // Remove this minion from all aggro towers
        foreach (TowerAggro tower in AttackableInterface.AggroTowers.Values)
            tower.Targets.Remove(this);
    }

    /// <summary>
    /// Deal damage to this minion and deactivate them if they die.
    /// </summary>
    /// <param name="damage"></param>
    public void DealDamage(int damage)
    {
        if (AttackableInterface.DealDamage(damage))
        {
            Deactivate();
        }
    }
}
