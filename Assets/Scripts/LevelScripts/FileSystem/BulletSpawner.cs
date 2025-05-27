using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private TurretBullet bulletPrefab;

    private ObjectPool<TurretBullet> _normalBulletPool;
    private ObjectPool<TurretBullet> _blastingBulletPool;

    private void Start()
    {
        _normalBulletPool = new ObjectPool<TurretBullet>(CreateNormalBullet,
                                                        OnTakeBulletFromPool,
                                                        OnReturnBulletToPool,
                                                        OnDestroyBullet,
                                                        false, 10, 10000);
        _blastingBulletPool = new ObjectPool<TurretBullet>(CreateNormalBullet,
                                                          OnTakeBulletFromPool,
                                                          OnReturnBulletToPool,
                                                          OnDestroyBullet,
                                                          false, 10, 10000);
    }


    private TurretBullet CreateNormalBullet()
    {
        TurretBullet bullet = Instantiate(bulletPrefab);
        return bullet;
    }

    private void OnTakeBulletFromPool(TurretBullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReturnBulletToPool(TurretBullet bullet)
    {
        bullet.ResetState();
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(TurretBullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    public TurretBullet SpawnNormalBullet(Sprite bulletSprite, Vector3 position, Quaternion rotation,
                                        int damage, float speed, Transform target)
    {
        TurretBullet bullet = _normalBulletPool.Get();
        bullet.GetComponent<SpriteRenderer>().sprite = bulletSprite;
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.Initialize(damage, speed, target, _normalBulletPool);
        bullet.transform.SetParent(transform);
        return bullet;
    }

    public TurretBullet SpawnBlastingBullet(Sprite bulletSprite, Animator blastAnimatorPrefab, Vector3 position, Quaternion rotation,
                                            int damage, float speed, float blastRadius, Transform target)
    {
        TurretBullet bullet = _blastingBulletPool.Get();
        bullet.GetComponent<SpriteRenderer>().sprite = bulletSprite;
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.InitializeBlasting(damage, speed, target, _blastingBulletPool,
                                    blastAnimatorPrefab, blastRadius);
        bullet.transform.SetParent(transform);
        return bullet;
    }



}