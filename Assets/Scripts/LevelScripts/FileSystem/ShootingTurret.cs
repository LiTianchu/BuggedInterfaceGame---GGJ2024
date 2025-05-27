using System;
using UnityEngine;

public class ShootingTurret : TurretFile
{
    [Header("Turret Settings")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float range = 5f;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private TargetPreferenceEnum targetPreference = TargetPreferenceEnum.FirstOrDefault;

    [Header("Bullet Settings")]
    [SerializeField] private Sprite bulletSprite;


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

        FileSystemLevelManager.Instance.BulletSpawner.SpawnNormalBullet(bulletSprite, firePoint.position, Quaternion.identity,
            damage, bulletSpeed, target);
    }

    private Transform GetTarget()
    {
        // find the closest target within range
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, range, targetLayer);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (!hitCollider.GetComponent<Zerg>().IsAlive())
            { continue; }

            switch (targetPreference)
            {
                case TargetPreferenceEnum.FirstOrDefault:
                    return hitCollider.transform;
                case TargetPreferenceEnum.Closest:
                    float closestDistance = Mathf.Infinity;
                    Transform closestTarget = null;
                    foreach (Collider2D collider in hitColliders)
                    {
                        float distance = Vector2.Distance(transform.position, collider.transform.position);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestTarget = collider.transform;
                        }
                    }
                    return closestTarget;
                case TargetPreferenceEnum.HighestHealth:
                    float highestHealth = 0;
                    Transform highestHealthTarget = null;
                    foreach (Collider2D collider in hitColliders)
                    {
                        Zerg zerg = collider.GetComponent<Zerg>();
                        if (zerg.ZergMaxHp > highestHealth)
                        {
                            highestHealth = zerg.ZergMaxHp;
                            highestHealthTarget = collider.transform;
                        }
                    }
                    return highestHealthTarget;
                case TargetPreferenceEnum.LowestHealth:
                    float lowestHealth = Mathf.Infinity;
                    Transform lowestHealthTarget = null;
                    foreach (Collider2D collider in hitColliders)
                    {
                        Zerg zerg = collider.GetComponent<Zerg>();
                        if (zerg.ZergMaxHp < lowestHealth)
                        {
                            lowestHealth = zerg.ZergMaxHp;
                            lowestHealthTarget = collider.transform;
                        }
                    }
                    return lowestHealthTarget;
            }
        }
        return null;
    }

    public override void OnDeploy() { }

    public override void OnDestroy() { }
}

public enum TargetPreferenceEnum
{
    FirstOrDefault,
    Closest,
    HighestHealth,
    LowestHealth,

}