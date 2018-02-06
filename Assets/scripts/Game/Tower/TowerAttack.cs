using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    public int Damage;
    public float Cooldown;

    private TowerAggro aggro;
    private float currentInterval;
    private Vector3 projectileSpawnPosition;

	// Use this for initialization
	public void Start () {
        aggro = GetComponent<TowerAggro>();
        projectileSpawnPosition = transform.parent.GetComponentInChildren<ProjectileSpawn>().transform.position;
	}
	
	// Update is called once per frame
	public void Update ()
    {
        currentInterval += Time.deltaTime;

        if (aggro.Targets.Count > 0 && ProjectilePool.Instance != null)
        {
            if (currentInterval > Cooldown)
            {
                currentInterval = 0;

                // Shoot minion in the face
                Minion target = aggro.GetTarget();

                Projectile projectile = ProjectilePool.Instance.GetProjectile();
                projectile.Fire(projectileSpawnPosition, target, Damage);
            }
        }
	}
}
