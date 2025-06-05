using UnityEngine;

[System.Serializable]
public class ClusterSpawnState : AbstractSpawnState
{
    [SerializeField] private ClusterSpawnEvent clusterSpawnPrefab;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxSpawns = 3;
    
    private float timeSinceLastSpawn = 0f;
    private int spawnCount = 0;
    
    public override void Enter(FileSystemLevelIntermediate level)
    {
        base.Enter(level);
        timeSinceLastSpawn = 0f;
        spawnCount = 0;
    }
    
    public override void Update(FileSystemLevelIntermediate level)
    {
        base.Update(level);
        timeSinceLastSpawn += Time.deltaTime;
        
        if (timeSinceLastSpawn >= spawnInterval && spawnCount < maxSpawns)
        {
            SpawnCluster(level);
            timeSinceLastSpawn = 0f;
            spawnCount++;
        }
    }
    
    private void SpawnCluster(FileSystemLevelIntermediate level)
    {
        Vector3 spawnPos = GetRandomSpawnPosition(level);
        level.CreateSpawnEvent(clusterSpawnPrefab, spawnPos);
    }
    
    private Vector3 GetRandomSpawnPosition(FileSystemLevelIntermediate level)
    {
        Vector2 randomCircle = Random.insideUnitCircle * 10f;
        return level.transform.position + new Vector3(randomCircle.x, randomCircle.y, 0);
    }
}

// filepath: e:\Projects\Unity\GGJ2024\Assets\Scripts\LevelScripts\FileSystem\SpawnStates\RingSpawnState.cs
[System.Serializable]
public class RingSpawnState : AbstractSpawnState
{
    [SerializeField] private RingSpawnEvent ringSpawnPrefab;
    [SerializeField] private float spawnInterval = 8f;
    
    private float timeSinceLastSpawn = 0f;
    
    public override void Enter(FileSystemLevelIntermediate level)
    {
        base.Enter(level);
        timeSinceLastSpawn = 0f;
    }
    
    public override void Update(FileSystemLevelIntermediate level)
    {
        base.Update(level);
        timeSinceLastSpawn += Time.deltaTime;
        
        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnRing(level);
            timeSinceLastSpawn = 0f;
        }
    }
    
    private void SpawnRing(FileSystemLevelIntermediate level)
    {
        Vector3 spawnPos = GetRandomSpawnPosition(level);
        level.CreateSpawnEvent(ringSpawnPrefab, spawnPos);
    }
    
    private Vector3 GetRandomSpawnPosition(FileSystemLevelIntermediate level)
    {
        Vector2 randomCircle = Random.insideUnitCircle * 8f;
        return level.transform.position + new Vector3(randomCircle.x, randomCircle.y, 0);
    }
}

// filepath: e:\Projects\Unity\GGJ2024\Assets\Scripts\LevelScripts\FileSystem\SpawnStates\IdleSpawnState.cs
[System.Serializable]
public class IdleSpawnState : AbstractSpawnState
{
    public override void Update(FileSystemLevelIntermediate level)
    {
        base.Update(level);
        // Do nothing, just wait
    }
}

// filepath: e:\Projects\Unity\GGJ2024\Assets\Scripts\LevelScripts\FileSystem\SpawnStates\MixedSpawnState.cs
[System.Serializable]
public class MixedSpawnState : AbstractSpawnState
{
    [SerializeField] private ClusterSpawnEvent clusterSpawnPrefab;
    [SerializeField] private RingSpawnEvent ringSpawnPrefab;
    [SerializeField] private float spawnInterval = 3f;
    
    private float timeSinceLastSpawn = 0f;
    
    public override void Update(FileSystemLevelIntermediate level)
    {
        base.Update(level);
        timeSinceLastSpawn += Time.deltaTime;
        
        if (timeSinceLastSpawn >= spawnInterval)
        {
            if (Random.value > 0.5f)
                SpawnCluster(level);
            else
                SpawnRing(level);
                
            timeSinceLastSpawn = 0f;
        }
    }
    
    private void SpawnCluster(FileSystemLevelIntermediate level)
    {
        Vector3 spawnPos = GetRandomSpawnPosition(level);
        level.CreateSpawnEvent(clusterSpawnPrefab, spawnPos);
    }
    
    private void SpawnRing(FileSystemLevelIntermediate level)
    {
        Vector3 spawnPos = GetRandomSpawnPosition(level);
        level.CreateSpawnEvent(ringSpawnPrefab, spawnPos);
    }
    
    private Vector3 GetRandomSpawnPosition(FileSystemLevelIntermediate level)
    {
        Vector2 randomCircle = Random.insideUnitCircle * 12f;
        return level.transform.position + new Vector3(randomCircle.x, randomCircle.y, 0);
    }
}