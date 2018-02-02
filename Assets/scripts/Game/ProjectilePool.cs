using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public Stack<Projectile> Projectiles;
    public int InitialPoolSize = 50;

    public static ProjectilePool Instance { get; private set; }

    public void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            Projectiles = new Stack<Projectile>();

            Projectile initialProjectile = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("prefabs/basic_projectile")).GetComponentInChildren<Projectile>();
            initialProjectile.InitProjectile(this);
            Projectiles.Push(initialProjectile);

            initialProjectile.gameObject.SetActive(false);

            while (Projectiles.Count < InitialPoolSize)
            {
                Projectile projectile = Instantiate<Projectile>(initialProjectile);
                projectile.InitProjectile(this);
                Projectiles.Push(projectile);
            }
        }
    }

    public void SetInactive(Projectile projectile)
    {
        Projectiles.Push(projectile);
    }

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
