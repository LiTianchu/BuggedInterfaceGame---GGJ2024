using System.Collections;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class ScatterSpawnState : AbstractSpawnState
{
    [SerializeField] private int maxZergCount = 50;
    [SerializeField] private float smallZergSpawnRate = 1.0f;
    [SerializeField] private float bigZergSpawnRate = 5.0f;
    [SerializeField] private bool transitOnAllZergKilled = true;
    private FileSystemLevelBattle _level;
    private float _smallZergSpawnTimeElapsed;
    private float _bigZergSpawnTimeElapsed;
    private int _zergSpawned;

    public override void Enter(FileSystemLevelBattle level)
    {
        base.Enter(level);
        _level = level;
        _smallZergSpawnTimeElapsed = smallZergSpawnRate;
        _bigZergSpawnTimeElapsed = bigZergSpawnRate;
        _level.ZergDestroyedCount = 0;
        _zergSpawned = 0;
    }

    public override void Update(FileSystemLevelBattle level)
    {
        base.Update(level);
        _smallZergSpawnTimeElapsed += Time.deltaTime;
        _bigZergSpawnTimeElapsed += Time.deltaTime;

        // spawn small zergs
        if (_smallZergSpawnTimeElapsed >= smallZergSpawnRate && _zergSpawned < maxZergCount)
        {
            _zergSpawned++;
            SpawnZerg(level);
            _smallZergSpawnTimeElapsed = 0.0f;
        }

        // spawn big zergs
        if (_bigZergSpawnTimeElapsed >= bigZergSpawnRate && _zergSpawned < maxZergCount)
        {
            _zergSpawned++;
            SpawnZerg(level, ZergTypeEnum.Big);
            _bigZergSpawnTimeElapsed = 0.0f;
        }

        if (transitOnAllZergKilled && level.ZergDestroyedCount >= maxZergCount)
        {
            TransitNow();
        }
    }

    public void SpawnZerg(FileSystemLevelBattle level, ZergTypeEnum zergType = ZergTypeEnum.Small)
    {
        GridSystem referenceGridSystem = level.GridSystem;
        // get a random point outside the grid
        Vector2 spawnPoint = VectorUtils.GetRandomPointOutsideBox(referenceGridSystem.GridLowerLeft, referenceGridSystem.GridUpperRight, 5f, 10f);
        Zerg zerg = null;
        switch (zergType)
        {
            case ZergTypeEnum.Small:
                zerg = FileSystemLevelManager.Instance.SmallZergPool.Get();
                zerg.Initialize(_level.MainCanvas, FileSystemLevelManager.Instance.SmallZergPool);
                break;
            case ZergTypeEnum.Big:
                zerg = FileSystemLevelManager.Instance.BigZergPool.Get();
                zerg.Initialize(_level.MainCanvas, FileSystemLevelManager.Instance.BigZergPool);
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

    public override void Enter(FileSystemLevelBattle level)
    {
        base.Enter(level);
        timeSinceLastSpawn = spawnInterval;
        spawnCount = 0;
    }

    public override void Update(FileSystemLevelBattle level)
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

    private void SpawnCluster(FileSystemLevelBattle level)
    {
        Vector3 spawnPos = GetRandomSpawnPosition(level);
        level.CreateSpawnEvent(clusterSpawnPrefab,
                                FileSystemLevelManager.Instance.SmallZergPool,
                                spawnPos);
    }

    private Vector3 GetRandomSpawnPosition(FileSystemLevelBattle level)
    {
        Vector2 randomCircle = Random.insideUnitCircle * 10f;
        return level.transform.position + new Vector3(randomCircle.x, randomCircle.y, 0);
    }
}

[System.Serializable]
public class RingSpawnState : AbstractSpawnState
{
    [SerializeField] private bool onlySpawnAtCenter = true;
    [SerializeField] private RingSpawnEvent ringSpawnPrefab;
    [SerializeField] private float spawnInterval = 8f;

    private float timeSinceLastSpawn = 0f;

    public override void Enter(FileSystemLevelBattle level)
    {
        base.Enter(level);
        timeSinceLastSpawn = spawnInterval;
    }

    public override void Update(FileSystemLevelBattle level)
    {
        base.Update(level);
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnRing(level);
            timeSinceLastSpawn = 0f;
        }
    }

    private void SpawnRing(FileSystemLevelBattle level)
    {
        Vector3 spawnPos = onlySpawnAtCenter ? new Vector3(0, 0, 0) : GetRandomSpawnPosition(level);
        level.CreateSpawnEvent(ringSpawnPrefab,
                                FileSystemLevelManager.Instance.SmallZergPool,
                                spawnPos);
    }

    private Vector3 GetRandomSpawnPosition(FileSystemLevelBattle level)
    {
        Vector2 randomCircle = Random.insideUnitCircle * 8f;
        return level.transform.position + new Vector3(randomCircle.x, randomCircle.y, 0);
    }
}

[System.Serializable]
public class WinState : AbstractSpawnState
{
    public override void Enter(FileSystemLevelBattle level)
    {
        base.Enter(level);
        level.PublishWin(); // Notify the level manager that the level is won
    }
}

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
    public override void Enter(FileSystemLevelBattle level)
    {
        base.Enter(level);
        timeSinceLastRingSpawn = ringSpawnInterval;
        timeSinceLastClusterSpawn = clusterSpawnInterval;
    }

    public override void Update(FileSystemLevelBattle level)
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

    private void SpawnCluster(FileSystemLevelBattle level)
    {
        Vector3 spawnPos = GetRandomSpawnPosition(level);
        level.CreateSpawnEvent(clusterSpawnPrefab,
                                FileSystemLevelManager.Instance.SmallZergPool,
                                spawnPos);
    }

    private void SpawnRing(FileSystemLevelBattle level)
    {

        Vector3 spawnPos = ringOnlySpawnAtCenter ? new Vector3(0, 0, 0) : GetRandomSpawnPosition(level);
        level.CreateSpawnEvent(ringSpawnPrefab,
                                FileSystemLevelManager.Instance.SmallZergPool,
                                spawnPos);
    }

    private Vector3 GetRandomSpawnPosition(FileSystemLevelBattle level)
    {
        Vector2 randomCircle = Random.insideUnitCircle * 12f;
        return level.transform.position + new Vector3(randomCircle.x, randomCircle.y, 0);
    }
}

[System.Serializable]
public class BossSpawnState : AbstractSpawnState
{
    [Header("Boss Spawn Settings")]
    [SerializeField] private Vector3 bossSpawnPosition;
    [SerializeField] private float delayBeforeSpawn = 2f;
    [SerializeField] private float spawnDelay = 2f;
    [SerializeField] private float delayBeforeMinion = 4f;
    [SerializeField] private bool transitOnBossDefeated = true;


    private float _timeSinceStart = 0f;
    private bool _hasSpawnedBoss = false;
    private bool _canSpawnMinions = false;
    private BossZerg _bossZerg;
    private FileSystemLevelBattle _level;

    public override void Enter(FileSystemLevelBattle level)
    {
        base.Enter(level);
        _level = level;
        _hasSpawnedBoss = false;
        _timeSinceStart = 0f;
        _canSpawnMinions = false;
        _bossZerg = null;
    }

    public override void Update(FileSystemLevelBattle level)
    {
        base.Update(level);
        _timeSinceStart += Time.deltaTime;

        if (_timeSinceStart >= delayBeforeSpawn && !_hasSpawnedBoss)
        {
            SpawnBossAtPosition(level);
        }

        if (_canSpawnMinions)
        {

        }
    }

    private void SpawnBossAtPosition(FileSystemLevelBattle level)
    {
        if (_hasSpawnedBoss) return;

        DialogueManager.StopAllConversations(); // replace
        DialogueManager.StartConversation("When Zerg Boss Appeared"); // Start the conversation for boss spawn

        _hasSpawnedBoss = true;
        _bossZerg = FileSystemLevelManager.Instance.GetBossZergInstance();
        _bossZerg.transform.SetPositionAndRotation(bossSpawnPosition, Quaternion.identity);
        _bossZerg.transform.SetParent(level.ZergContainer);

        _bossZerg.transform.localScale = Vector3.zero;
        _bossZerg.CanBeTargeted = false;
        _bossZerg.transform.DOScale(Vector3.one, spawnDelay).OnComplete(() =>
        {
            _bossZerg.IsBossReady = true;
            _bossZerg.CanBeTargeted = true;
            _bossZerg.OnZergDestroyed += HandleBossDefeated;
            _bossZerg.Initialize(_level.MainCanvas);
            // After the boss zerg is spawned, wait for a while before transitioning to the next state
            DOVirtual.DelayedCall(delayBeforeMinion, () =>
            {
                _canSpawnMinions = true;
            });
        });
    }

    private void HandleBossDefeated()
    {
        _bossZerg.OnZergDestroyed -= HandleBossDefeated;
        if (transitOnBossDefeated)
        {
            Debug.Log("Boss defeated, transitioning to next state.");
            TransitNow();
        }
    }
    

}