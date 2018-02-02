using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public Stack<Projectile> Projectiles;
    public int InitialPoolSize = 50;

    public static ProjectilePool Instance { get; private set; }

    /// <summary>
    /// Only instance this Projectile Pool if there is not already a Projectile Pool instance.
    /// </summary>
    public void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            Projectiles = new Stack<Projectile>();

            Projectile initialProjectile = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("prefabs/basic_projectile")).GetComponentInChildren<Projectile>();
            initialProjectile.InitProjectile(this);
            Projectiles.Push(initialProjectile);

            while (Projectiles.Count < InitialPoolSize)
            {
                Projectile projectile = Instantiate<Projectile>(initialProjectile);
                projectile.InitProjectile(this);
                Projectiles.Push(projectile);
            }

        }
    }

    /// <summary>
    /// Push the passed projectile into the pool.
    /// </summary>
    /// <param name="projectile"></param>
    public void SetInactive(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        Projectiles.Push(projectile);
    }

    /// <summary>
    /// Pull a projectile from the pool.
    /// Create a new projectile if there are no projectiles left.
    /// </summary>
    /// <returns></returns>
    public Projectile GetProjectile()
    {
        if (Projectiles.Count == 0)
        {
            Projectile newProjectile = GameObject.Instantiate<Projectile>(Resources.Load<Projectile>("prefabs/basic_projectile"));
            newProjectile.InitProjectile(this);
            Projectiles.Push(newProjectile);
        }

        return Projectiles.Pop();
    }
}
