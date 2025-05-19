using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private TurretBullet normalBulletPrefab;
    private ObjectPool<TurretBullet> _turretBulletPool;

    private void Start()
    {
        _turretBulletPool = new ObjectPool<TurretBullet>(CreateNormalBullet,
                                                        OnTakeBulletFromPool,
                                                        OnReturnBulletToPool,
                                                        OnDestroyBullet,
                                                        false,10,10000);
    }
    private TurretBullet CreateNormalBullet()
    {
        TurretBullet bullet = Instantiate(normalBulletPrefab);
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
        TurretBullet bullet = _turretBulletPool.Get();
        bullet.GetComponent<SpriteRenderer>().sprite = bulletSprite;
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.Initialize(damage, speed, target, _turretBulletPool);
        bullet.transform.SetParent(transform);
        return bullet;
    }

}