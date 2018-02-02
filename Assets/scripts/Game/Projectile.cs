using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float velocity = 3.0f / 60.0f;
    private Transform Target;
    private int damage;

    private ProjectilePool owningPool = null;

    public void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void Update ()
    {
		if (Target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.position, velocity);
        }
        else if (Target == null)
        {
            gameObject.SetActive(false);
        }
	}

    public void Fire(Vector3 startingPosition, Transform target, int damage)
    {
        Target = target;
        transform.position = startingPosition;
        this.damage = damage;

        gameObject.SetActive(true);
    }

    public void OnEnable()
    {
        
    }

    public void OnDisable()
    {
        CancelInvoke();
        owningPool.SetInactive(this);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Attackable target = collision.gameObject.GetComponentInChildren<Attackable>();

        if (target != null)
        {
            target.DealDamage(damage);
        }
    }

    public void InitProjectile(ProjectilePool owner)
    {
        if (owningPool == null)
        {
            owningPool = owner;
        }
    }
}
