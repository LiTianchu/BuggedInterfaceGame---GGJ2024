using UnityEngine;
[System.Serializable]
public class ScatterSpawnState : AbstractSpawnState
{
    [SerializeField] private int maxZergCount = 50;
    [SerializeField] private float smallZergSpawnRate = 1.0f;
    [SerializeField] private float bigZergSpawnRate = 5.0f;
    [SerializeField] private bool transitOnAllZergKilled= true;

    private float _smallZergSpawnTimeElapsed;
    private float _bigZergSpawnTimeElapsed;

    public override void Enter(FileSystemLevel level)
    {
        base.Enter(level);
        _smallZergSpawnTimeElapsed = smallZergSpawnRate;
        _bigZergSpawnTimeElapsed = bigZergSpawnRate;
    }

    public override void Update(FileSystemLevel level)
    {
        base.Update(level);
        _smallZergSpawnTimeElapsed += Time.deltaTime;
        _bigZergSpawnTimeElapsed += Time.deltaTime;

        // spawn small zergs
        if (_smallZergSpawnTimeElapsed >= smallZergSpawnRate && level.ZergCount < maxZergCount)
        {
            SpawnZerg(level);
            _smallZergSpawnTimeElapsed = 0.0f;
        }

        // spawn big zergs
        if (_bigZergSpawnTimeElapsed >= bigZergSpawnRate && level.ZergCount < maxZergCount)
        {
            SpawnZerg(level, ZergTypeEnum.Big);
            _bigZergSpawnTimeElapsed = 0.0f;
        }

        if(transitOnAllZergKilled && level.ZergDestroyedCount >= maxZergCount)
        {
            TransitNow();
        }
    }

    public void SpawnZerg(FileSystemLevel level, ZergTypeEnum zergType = ZergTypeEnum.Small)
    {
        GridSystem referenceGridSystem = level.GridSystem;
        // get a random point outside the grid
        Vector2 spawnPoint = VectorUtils.GetRandomPointOutsideBox(referenceGridSystem.GridLowerLeft, referenceGridSystem.GridUpperRight, 5f, 10f);
        Zerg zerg = null;
        switch (zergType)
        {
            case ZergTypeEnum.Small:
                zerg = FileSystemLevelManager.Instance.SmallZergPool.Get();
                zerg.Initialize(FileSystemLevelManager.Instance.SmallZergPool);
                break;
            case ZergTypeEnum.Big:
                zerg = FileSystemLevelManager.Instance.BigZergPool.Get();
                zerg.Initialize(FileSystemLevelManager.Instance.BigZergPool);
                break;
        }

        zerg.transform.SetPositionAndRotation(spawnPoint, Quaternion.identity);
        zerg.transform.SetParent(level.ZergContainer);
        level.ZergCount++;
        zerg.gameObject.name = $"{zergType}-{level.ZergCount}";
        //Debug.Log($"Spawned {zergType} zerg with index {level.ZergCount}");
    }
}


[System.Serializable]
public class ClusterSpawnState : AbstractSpawnState
{
    [SerializeField] private ClusterSpawnEvent clusterSpawnPrefab;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxSpawns = 3;

    private float timeSinceLastSpawn = 0f;
    private int spawnCount = 0;

    public override void Enter(FileSystemLevel level)
    {
        base.Enter(level);
        timeSinceLastSpawn = 0f;
        spawnCount = 0;
    }

    public override void Update(FileSystemLevel level)
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

    private void SpawnCluster(FileSystemLevel level)
    {
        Vector3 spawnPos = GetRandomSpawnPosition(level);
        level.CreateSpawnEvent(clusterSpawnPrefab, spawnPos);
    }

    private Vector3 GetRandomSpawnPosition(FileSystemLevel level)
    {
        Vector2 randomCircle = Random.insideUnitCircle * 10f;
        return level.transform.position + new Vector3(randomCircle.x, randomCircle.y, 0);
    }
}

// filepath: e:\Projects\Unity\GGJ2024\Assets\Scripts\LevelScripts\FileSystem\SpawnStates\RingSpawnState.cs
[System.Serializable]
public class RingSpawnState : AbstractSpawnState
{
    [SerializeField] private bool onlySpawnAtCenter = true;
    [SerializeField] private RingSpawnEvent ringSpawnPrefab;
    [SerializeField] private float spawnInterval = 8f;

    private float timeSinceLastSpawn = 0f;

    public override void Enter(FileSystemLevel level)
    {
        base.Enter(level);
        timeSinceLastSpawn = 0f;
    }

    public override void Update(FileSystemLevel level)
    {
        base.Update(level);
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnRing(level);
            timeSinceLastSpawn = 0f;
        }
    }

    private void SpawnRing(FileSystemLevel level)
    {
        Vector3 spawnPos = onlySpawnAtCenter ? new Vector3(0, 0, 0) : GetRandomSpawnPosition(level);
        level.CreateSpawnEvent(ringSpawnPrefab, spawnPos);
    }

    private Vector3 GetRandomSpawnPosition(FileSystemLevel level)
    {
        Vector2 randomCircle = Random.insideUnitCircle * 8f;
        return level.transform.position + new Vector3(randomCircle.x, randomCircle.y, 0);
    }
}

// filepath: e:\Projects\Unity\GGJ2024\Assets\Scripts\LevelScripts\FileSystem\SpawnStates\IdleSpawnState.cs
[System.Serializable]
public class WinState : AbstractSpawnState
{
    public override void Enter(FileSystemLevel level)
    {
        base.Enter(level);
        level.PublishWin(); // Notify the level manager that the level is won
    }
}

// filepath: e:\Projects\Unity\GGJ2024\Assets\Scripts\LevelScripts\FileSystem\SpawnStates\MixedSpawnState.cs
[System.Serializable]
public class MixedSpawnState : AbstractSpawnState
{
    [SerializeField] private ClusterSpawnEvent clusterSpawnPrefab;
    [SerializeField] private RingSpawnEvent ringSpawnPrefab;
    [SerializeField] private bool ringOnlySpawnAtCenter = true;
    [SerializeField] private float clusterSpawnInterval = 3f;
    [SerializeField] private float ringSpawnInterval = 6f;

    private float timeSinceLastRingSpawn = 0f;
    private float timeSinceLastClusterSpawn = 0f;

    public override void Update(FileSystemLevel level)
    {
        base.Update(level);
        timeSinceLastRingSpawn += Time.deltaTime;
        timeSinceLastClusterSpawn += Time.deltaTime;

        if (timeSinceLastRingSpawn >= ringSpawnInterval)
        {
            SpawnCluster(level);

            timeSinceLastRingSpawn = 0f;
        }

        if (timeSinceLastClusterSpawn >= clusterSpawnInterval)
        {
            SpawnRing(level);
            timeSinceLastClusterSpawn = 0f;
        }
    }

    private void SpawnCluster(FileSystemLevel level)
    {
        Vector3 spawnPos = GetRandomSpawnPosition(level);
        level.CreateSpawnEvent(clusterSpawnPrefab, spawnPos);
    }

    private void SpawnRing(FileSystemLevel level)
    {

        Vector3 spawnPos = ringOnlySpawnAtCenter ? new Vector3(0, 0, 0) : GetRandomSpawnPosition(level);
        level.CreateSpawnEvent(ringSpawnPrefab, spawnPos);
    }

    private Vector3 GetRandomSpawnPosition(FileSystemLevel level)
    {
        Vector2 randomCircle = Random.insideUnitCircle * 12f;
        return level.transform.position + new Vector3(randomCircle.x, randomCircle.y, 0);
    }
}