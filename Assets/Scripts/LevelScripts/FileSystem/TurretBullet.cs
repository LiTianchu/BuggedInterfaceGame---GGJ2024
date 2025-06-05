using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class TurretBullet : MonoBehaviour
{
    private int _damage;
    private float _speed;
    private Zerg _target;
    private ObjectPool<TurretBullet> _pool;

    private bool _hasBlastingEffect = false;
    private float _blastingRadius = 0f;
    private Animator _blastAnimatorPrefab;
    private Vector3 _recordedTargetPosition;
    private bool _isTargetDestroyed = false;



    public void Initialize(int damage, float speed, Zerg target, ObjectPool<TurretBullet> pool)
    {
        _pool = pool;
        _damage = damage;
        _speed = speed;
        _target = target;
        _hasBlastingEffect = false;
        _blastAnimatorPrefab = null;
        _blastingRadius = 0;
        _isTargetDestroyed = false;

        _target.OnZergDestroyed += HandleTargetZergDestroyed;
    }

    public void InitializeBlasting(int damage, float speed, Zerg target, ObjectPool<TurretBullet> pool,
                                   Animator blastAnimatorPrefab, float blastingRadius)
    {
        _pool = pool;
        _damage = damage;
        _speed = speed;
        _target = target;
        _hasBlastingEffect = true;
        _blastAnimatorPrefab = blastAnimatorPrefab;
        _blastingRadius = blastingRadius;
        _isTargetDestroyed = false;

        _target.OnZergDestroyed += HandleTargetZergDestroyed;
    }

    private void Update()
    {
        if (_target != null)
        {
            if (!_isTargetDestroyed) // If the target is not destroyed, update the target position
            {
                _recordedTargetPosition = _target.transform.position;
            }
            
            Vector3 direction = (_recordedTargetPosition - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.position += direction * _speed * Time.deltaTime;
            transform.rotation = rotation;

            if (Vector3.Distance(transform.position, _recordedTargetPosition) < 0.1f)
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


    private void HandleTargetZergDestroyed()
    {
        _isTargetDestroyed = true;
        _target.OnZergDestroyed -= HandleTargetZergDestroyed;
    }

    private void DestroyBullet()
    {
        _target.OnZergDestroyed -= HandleTargetZergDestroyed;
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
        transform.position = new Vector3(999f, 999f, 0f);
        transform.rotation = Quaternion.identity;
        _target = null;
    }
}