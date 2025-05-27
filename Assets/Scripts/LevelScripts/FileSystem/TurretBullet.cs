using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class TurretBullet : MonoBehaviour
{
    private int _damage;
    private float _speed;
    private Transform _target;
    private ObjectPool<TurretBullet> _pool;

    private bool _hasBlastingEffect = false;
    private float _blastingRadius = 0f;
    private Animator _blastAnimatorPrefab;



    public void Initialize(int damage, float speed, Transform target, ObjectPool<TurretBullet> pool)
    {
        _pool = pool;
        _damage = damage;
        _speed = speed;
        _target = target;
        _hasBlastingEffect = false;
        _blastAnimatorPrefab = null;
        _blastingRadius = 0;
    }

    public void InitializeBlasting(int damage, float speed, Transform target, ObjectPool<TurretBullet> pool,
                                   Animator blastAnimatorPrefab, float blastingRadius)
    {
        _pool = pool;
        _damage = damage;
        _speed = speed;
        _target = target;
        _hasBlastingEffect = true;
        _blastAnimatorPrefab = blastAnimatorPrefab;
        _blastingRadius = blastingRadius;
    }

    private void Update()
    {
        if (_target != null)
        {
            Vector3 direction = (_target.position - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.position += direction * _speed * Time.deltaTime;
            transform.rotation = rotation;

            if (Vector3.Distance(transform.position, _target.position) < 0.1f)
            {
                if (_hasBlastingEffect)
                {
                    // Apply damage to all targets in the blast radius
                    Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _blastingRadius);
                    foreach (Collider2D hitCollider in hitColliders)
                    {
                        Zerg zerg = hitCollider.GetComponent<Zerg>();
                        if (zerg != null && zerg.IsAlive())
                        {
                            zerg.TakeDamage(_damage);
                        }
                    }
                    Instantiate(_blastAnimatorPrefab, transform.position, Quaternion.identity);
                    DestroyBullet();
                }
                else
                {
                    _target.GetComponent<Zerg>().TakeDamage(_damage);
                    DestroyBullet();
                }
            }
        }
        else
        {
            DestroyBullet();
        }
    }




    private void DestroyBullet()
    {
        if (_pool != null)
        {
            _pool.Release(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetState()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        _target = null;
    }
}