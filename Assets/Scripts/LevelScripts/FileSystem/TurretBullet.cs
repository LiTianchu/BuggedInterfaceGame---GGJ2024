using System;
using UnityEngine;
using UnityEngine.Pool;

public class TurretBullet : MonoBehaviour
{
    private int _damage;
    private float _speed;
    private Transform _target;
    private ObjectPool<TurretBullet> _pool;

    public void Initialize(int damage, float speed, Transform target, ObjectPool<TurretBullet> pool)
    {
        _pool = pool;
        _damage = damage;
        _speed = speed;
        _target = target;
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
                _target.GetComponent<Zerg>().TakeDamage(_damage);
                DestroyBullet();
            }
        }
        else
        {
            DestroyBullet();
        }
    }
    
    private void DestroyBullet()
    {
        if(_pool != null)
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