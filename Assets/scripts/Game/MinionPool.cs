using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MinionPool
{
    private static MinionPool _instance;
    public static MinionPool Instance
    {
        get
        {
            if (_instance == null) _instance = new MinionPool();
            return _instance;
        }

        private set
        {
            _instance = value;
        }
    }

    public Stack<Minion> Minions;

    public MinionPool(int initialPoolSize = 50)
    {
        Minions = new Stack<Minion>();

        Minion prototype = GameObject.Instantiate<Minion>(Resources.Load<Minion>("prefabs/enemy/enemy"));
        prototype.InitInterfaces(this);
        Minions.Push(prototype);

        int id = 0;
        while (Minions.Count < initialPoolSize)
        {
            id++;
            Minion minion = GameObject.Instantiate<Minion>(prototype);
            minion.ID = id;
            
            minion.InitInterfaces(this);
            Minions.Push(minion);
        }
    }

    public void SetInactive(Minion minion)
    {
        Minions.Push(minion);
    }

    public Minion GetMinion()
    {
        return Minions.Pop();
    }

    public static void Reset()
    {
        _instance = new MinionPool();
    }
}
