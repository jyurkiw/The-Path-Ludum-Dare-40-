using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAggro : MonoBehaviour
{
    public List<Attackable> Targets { get; private set; }
    public int ID { get; private set; }

    private bool Sorted = false;

    private static int id = 0;

	// Use this for initialization
	public void Start ()
    {
        Targets = new List<Attackable>();
        ID = id++;
	}
	
	// Update is called once per frame
	public void Update ()
    {
		
	}

    /// <summary>
    /// When the aggro trigger is entered, add any attackable objects to the target list.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        Attackable target = other.GetComponent<Attackable>();
        if (target != null)
        {
            Targets.Add(target);
            target.TagTower(this);
            Sorted = false;
        }
    }

    /// <summary>
    /// If a game object leaves the aggro trigger, remove it from the targets list.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerExit(Collider other)
    {
        Attackable target = other.GetComponent<Attackable>();
        if (target != null)
        {
            target.UnTagTower(this);
            Targets.Remove(target);
        }
    }
    
    /// <summary>
    /// Get the attack target. Calling this method will sort the target list if necessary.
    /// Since sorting can be expensive, GetTarget should be called as little as possible.
    /// </summary>
    /// <param name="sorter"></param>
    /// <returns></returns>
    public Attackable GetTarget(IComparer<Attackable> sorter = null)
    {
        if (Targets.Count == 1) return Targets[0];

        if (Targets.Count > 0)
        {
            if (!Sorted && sorter != null)
            {
                Targets.Sort(sorter);
            }
            else if (!Sorted)
            {
                Targets.Sort(CompareDistances);
            }
            Sorted = true;

            return Targets[0];
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Distance comparable implementation for List.Sort.
    /// Sorts by distance to this TowerAggro component.
    /// </summary>
    /// <param name="at1"></param>
    /// <param name="at2"></param>
    /// <returns></returns>
    public int CompareDistances(Attackable at1, Attackable at2)
    {
        float compValue = Mathf.Clamp(Vector3.Distance(transform.position, at1.transform.position) - Vector3.Distance(transform.position, at2.transform.position), -1, 1);
        return compValue == 0 ? 0 : compValue > 0 ? 1 : -1;
    }
}
