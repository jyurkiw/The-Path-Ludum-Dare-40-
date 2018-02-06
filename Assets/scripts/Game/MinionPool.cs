using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Singleton Object pool implementation for Minions.
/// </summary>
public class MinionPool
{
    /// <summary>
    /// If set to true all pooled minions are set to Don't Destroy On Load.
    /// This is necessary for testing.
    /// </summary>
    public static bool KeepOnLoad = true;
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
    private int IDCounter = 0;

    /// <summary>
    /// Create a new minion pool.
    /// </summary>
    /// <param name="initialPoolSize"></param>
    public MinionPool(int initialPoolSize = 50)
    {
        Minions = new Stack<Minion>();

        Minion prototype = GameObject.Instantiate<Minion>(Resources.Load<Minion>("prefabs/enemy/enemy"));
        prototype.InitInterfaces(this);
        if (KeepOnLoad) GameObject.DontDestroyOnLoad(prototype);
        Minions.Push(prototype);
        
        while (Minions.Count < initialPoolSize)
            Minions.Push(NewMinion(prototype));
    }

    private Minion NewMinion(Minion prototype)
    {
        Minion minion = GameObject.Instantiate<Minion>(prototype);
        if (KeepOnLoad) GameObject.DontDestroyOnLoad(minion);
        minion.ID = ++IDCounter;

        minion.InitInterfaces(this);
        return minion;
    }

    /// <summary>
    /// Send the passed minion to the inactive pool.
    /// </summary>
    /// <param name="minion"></param>
    public void SetInactive(Minion minion)
    {
        Minions.Push(minion);
    }

    /// <summary>
    /// Get a deactivated minion.
    /// If we're down to our last minion, clone them and return that minion instead.
    /// </summary>
    /// <returns></returns>
    public Minion GetMinion()
    {
        if (Minions.Count == 1)
            Minions.Push(NewMinion(Minions.Peek()));

        Minion minion = Minions.Pop();
        minion.ID = ++IDCounter;
        minion.AttackableInterface.HP = minion.AttackableInterface.MaxHp;

        return minion;
    }

    /// <summary>
    /// Reset this minion pool.
    /// Needs to be called any time you load a scene if KeepOnLoad is set to false.
    /// Must be called at the beginning of every client test method because
    /// minions are destroyed between method calls for some stupid reason.
    /// </summary>
    public static void Reset()
    {
        _instance = new MinionPool();
    }
}
