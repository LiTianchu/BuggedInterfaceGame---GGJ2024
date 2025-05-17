using System;
using UnityEngine;

public class AttackableTurret : TurretFile
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float range = 5f;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask targetLayer;
    

    private float nextFireTime = 0f;
    private void Update()
    {
        Fire();   
    }

    public override void Fire()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        Transform target = GetTarget();
        if (target == null) { return; }

        FileSystemLevelManager.Instance.BulletSpawner.SpawnNormalBullet(firePoint.position, Quaternion.identity,
            damage, bulletSpeed, target);
    }
    
    private Transform GetTarget()
    {
        // find the closest target within range
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, range, targetLayer);
        foreach (Collider2D hitCollider in hitColliders)
        {
            return hitCollider.transform;
        }
        return null;
    }

    public override void OnDeploy() { }

    public override void OnDestroy(){}
}