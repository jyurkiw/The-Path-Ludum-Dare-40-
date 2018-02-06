using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float velocity = 3.0f / 60.0f;
    private Minion Target;
    private Vector3 targetPosition;
    private int damage;

    private ProjectilePool owningPool = null;

    /// <summary>
    /// Start projectiles in an inactive state.
    /// TODO: Remove this by deactivating the prefab when you fix the ProjectilePool.
    /// </summary>
    public void Start()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Check our position.
    /// When we reach our destination, deactivate.
    /// </summary>
    public void Update ()
    {
        if (targetPosition != Target.AttackableInterface.transform.position)
        {
            targetPosition = Target.AttackableInterface.transform.position;
        }

        if (transform.position != targetPosition)
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, velocity);
        else gameObject.SetActive(false);
	}

    /// <summary>
    /// Set this projectile's position, target, damage, and activate it.
    /// </summary>
    /// <param name="startingPosition"></param>
    /// <param name="target"></param>
    /// <param name="damage"></param>
    public void Fire(Vector3 startingPosition, Minion target, int damage)
    {
        Target = target;
        transform.position = startingPosition;
        this.damage = damage;
        
        gameObject.SetActive(true);
    }

    public void OnEnable()
    {
        
    }

    /// <summary>
    /// When disabled, make sure we clear any invocations and put it in the Projectile Pool's inactive pool.
    /// </summary>
    public void OnDisable()
    {
        CancelInvoke();
        owningPool.SetInactive(this);
    }

    /// <summary>
    /// When the projectile's trigger box meets something it can hit, check to see if it's a minion.
    /// If it's a minion, deal damage and deactivate.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        Minion target = other.gameObject.GetComponentInParent<Minion>();

        if (target != null)
        {
            target.DealDamage(damage);
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Initialize the projectile.
    /// Set the owning projectile pool.
    /// Can only be called once.
    /// </summary>
    /// <param name="owner"></param>
    public void InitProjectile(ProjectilePool owner)
    {
        if (owningPool == null)
        {
            owningPool = owner;
        }
    }
}
