using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class ShootingTurret : TurretFile
{
    [TitleGroup("Turret Settings")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float range = 5f;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private int targetCountPerShot = 1;

    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private TargetPreferenceEnum targetPreference = TargetPreferenceEnum.FirstOrDefault;

    [TitleGroup("Bullet Settings")]
    [Required]
    [SerializeField] private Sprite bulletSprite;

    [TitleGroup("Blast Effect")]
    [SerializeField] private bool hasBlastEffect = false;
    [SerializeField] private Animator blastAnimator;
    [SerializeField] private float blastRadius = 1f;


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
        List<Zerg> targets = GetTargets();

        if (targets == null || targets.Count == 0) { return; }

        // Shoot at each target
        foreach (Zerg target in targets)
        {
            if (hasBlastEffect)
            {
                FileSystemLevelManager.Instance.BulletSpawner.SpawnBlastingBullet(
                    bulletSprite, blastAnimator, firePoint.position, Quaternion.identity,
                    damage, bulletSpeed, blastRadius, target);
            }
            else
            {
                FileSystemLevelManager.Instance.BulletSpawner.SpawnNormalBullet(
                    bulletSprite, firePoint.position, Quaternion.identity,
                    damage, bulletSpeed, target);
            }
        }
    }

    private List<Zerg> GetTargets()
    {
        // Find all valid targets within range
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, range, targetLayer);
        List<Zerg> validTargets = new List<Zerg>();
        
        // Filter valid targets first
        foreach (Collider2D hitCollider in hitColliders)
        {
            Zerg zerg = hitCollider.GetComponent<Zerg>();
            if (zerg != null && zerg.IsAlive() && zerg.CanBeTargeted)
            {
                validTargets.Add(zerg);
            }
        }
        
        if (validTargets.Count == 0)
            return new List<Zerg>();

        List<Zerg> finalTargets = new List<Zerg>();

        switch (targetPreference)
        {
            case TargetPreferenceEnum.FirstOrDefault:
                // Take the first N targets up to targetCountPerShot
                int count = Mathf.Min(targetCountPerShot, validTargets.Count);
                for (int i = 0; i < count; i++)
                {
                    finalTargets.Add(validTargets[i]);
                }
                break;

            case TargetPreferenceEnum.Closest:
                // Sort by distance and take the closest N targets
                validTargets.Sort((a, b) => 
                {
                    float distanceA = Vector2.Distance(transform.position, a.transform.position);
                    float distanceB = Vector2.Distance(transform.position, b.transform.position);
                    return distanceA.CompareTo(distanceB);
                });
                
                count = Mathf.Min(targetCountPerShot, validTargets.Count);
                for (int i = 0; i < count; i++)
                {
                    finalTargets.Add(validTargets[i]);
                }
                break;

            case TargetPreferenceEnum.HighestHealth:
                // Sort by health (descending) and take the N highest health targets
                validTargets.Sort((a, b) => b.ZergHp.CompareTo(a.ZergHp));
                
                count = Mathf.Min(targetCountPerShot, validTargets.Count);
                for (int i = 0; i < count; i++)
                {
                    finalTargets.Add(validTargets[i]);
                }
                break;

            case TargetPreferenceEnum.LowestHealth:
                // Sort by health (ascending) and take the N lowest health targets
                validTargets.Sort((a, b) => a.ZergHp.CompareTo(b.ZergHp));
                
                count = Mathf.Min(targetCountPerShot, validTargets.Count);
                for (int i = 0; i < count; i++)
                {
                    finalTargets.Add(validTargets[i]);
                }
                break;
        }

        return finalTargets;
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