using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MinionPool
{
    public static MinionPool Instance { get; private set; }

    public Stack<Attackable> Minions;

    public MinionPool(int initialPoolSize = 50)
    {
        Minions = new Stack<Attackable>();

        Attackable prototype = GameObject.Instantiate<Attackable>(Resources.Load<GameObject>("prefabs/enemy/enemy").GetComponentInChildren<Attackable>());
        prototype.gameObject.SetActive(false);
        prototype.SetPoolOwner(this);
        Minions.Push(prototype);

        while (Minions.Count < initialPoolSize)
        {
            Attackable minion = GameObject.Instantiate<Attackable>(prototype);
            Minions.Push(minion);
        }
    }

    public void SetInactive(Attackable minion)
    {
        Minions.Push(minion);
    }

    public Attackable GetMinion()
    {
        return Minions.Pop();
    }
}
