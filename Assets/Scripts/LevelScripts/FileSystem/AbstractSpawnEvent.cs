using UnityEngine;
using UnityEngine.Pool;

public abstract class AbstractSpawnEvent : MonoBehaviour
{
    public abstract void Initialize(FileSystemLevel currentLevel,ObjectPool<Zerg> zergPool, Transform zergContainer);
    
    public abstract void Spawn();
}