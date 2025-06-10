using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShockwaveTurret : TurretFile
{
    [Header("Turret Settings")]
    [SerializeField] private int damage = 3;
    [SerializeField] private float fireRate = 3f;
    [SerializeField] private float range = 2.5f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask targetLayer;

    [Header("Bullet Settings")]
    [SerializeField] private Animator shockwaveAnimator;
    
    private float _nextFireTime = 0f;

    private void Update()
    {
        Fire();
    }
    
    public override void Fire()
    {
        if (Time.time >= _nextFireTime)
        {
            _nextFireTime = Time.time + fireRate;
            LaunchShockwave();
        }
    }

    private void LaunchShockwave()
    {
        List<Zerg> targets = GetTargetsInRange();
        if (targets.Count == 0) { return; }

        // Play shockwave animation
        Instantiate(shockwaveAnimator, firePoint.position, Quaternion.identity);


        foreach (Zerg target in targets)
        {
            // Apply damage to each target
            target.TakeDamage(damage);
        }
    }
    
    private List<Zerg> GetTargetsInRange()
    {
        // find all targets within range
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, range, targetLayer);
        List<Zerg> targets = new List<Zerg>();
        
        foreach (Collider2D hitCollider in hitColliders)
        {
            Zerg zerg = hitCollider.GetComponent<Zerg>();
            if (zerg != null && zerg.IsAlive() && zerg.CanBeTargeted)
            {
                targets.Add(zerg);
            }
        }
        
        return targets;
    }

   



    public override void OnDeploy()
    {

    }

    public override void OnDestroy()
    {
        
    }
}