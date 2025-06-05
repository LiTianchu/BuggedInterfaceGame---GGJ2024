using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;

public class ClusterSpawnEvent : AbstractSpawnEvent
{
    [SerializeField] private float clusterRadius = 5f;
    [SerializeField] private float spawnPointDistance = 2;
    [SerializeField] private float spawnDelay = 0.1f;
    [SerializeField] private float spawnAnimationDuration = 0.3f; 
    [SerializeField] private Transform spawnCenter; // if null, uses this transform as center
    [SerializeField] private int numberOfZergs = 10; // Number of Zergs to spawn in the cluster

    public float ClusterRadius { get => clusterRadius; set => clusterRadius = value; }
    public float SpawnPointDistance { get => spawnPointDistance; set => spawnPointDistance = value; }
    public float SpawnDelay { get => spawnDelay; set => spawnDelay = value; }
    public float SpawnAnimationDuration { get => spawnAnimationDuration; set => spawnAnimationDuration = value; }
    public Transform SpawnCenter { get => spawnCenter; set => spawnCenter = value; }
    public int NumberOfZergs { get => numberOfZergs; set => numberOfZergs = value; }

    public ObjectPool<Zerg> ZergPool { get; private set; }
    public Transform ZergContainer { get; private set; }
    public FileSystemLevel CurrentLevel { get; private set; }

    public override void Initialize(FileSystemLevel currentLevel, ObjectPool<Zerg> zergPool, Transform zergContainer)
    {
        ZergPool = zergPool;
        ZergContainer = zergContainer;
        CurrentLevel = currentLevel;
    }

    public override void Spawn()
    {
        if (ZergPool == null || ZergContainer == null || CurrentLevel == null)
        {
            throw new System.Exception("ClusterSpawnEvent is not properly initialized. Please call Initialize() before spawning.");
        }

        StartCoroutine(SpawnAnimated());
    }

    private IEnumerator SpawnAnimated()
    {
        Vector3 centerPosition = spawnCenter != null ? spawnCenter.position : transform.position;
        List<Vector3> spawnPositions = GetClusterSamplePositions(centerPosition, clusterRadius, numberOfZergs, spawnPointDistance);

        foreach (Vector3 position in spawnPositions)
        {
            SpawnZergAtPosition(position);
            yield return new WaitForSeconds(spawnDelay);
        }

        // Wait for the last spawn animation to complete before destroying
        yield return new WaitForSeconds(spawnAnimationDuration);

        // Destroy this spawn event object
        Destroy(gameObject);
    }

    /// <summary>
    /// Generates a list of positions randomly distributed within a cluster area
    /// </summary>
    /// <param name="centerPosition">Center of the cluster</param>
    /// <param name="radius">Maximum radius of the cluster</param>
    /// <param name="count">Number of positions to generate</param>
    /// <param name="minDistance">Minimum distance between positions</param>
    /// <returns>List of positions within the cluster</returns>
    public static List<Vector3> GetClusterSamplePositions(Vector3 centerPosition, float radius, int count, float minDistance)
    {
        List<Vector3> positions = new List<Vector3>();
        int maxAttempts = count * 30; // Prevent infinite loops
        int attempts = 0;

        while (positions.Count < count && attempts < maxAttempts)
        {
            // Generate random position within circle using polar coordinates
            float angle = Random.Range(0f, 2f * Mathf.PI);
            float distance = Random.Range(0f, radius);
            
            // Use square root for uniform distribution
            distance = Mathf.Sqrt(distance / radius) * radius;

            Vector3 candidatePosition = new Vector3(
                centerPosition.x + distance * Mathf.Cos(angle),
                centerPosition.y + distance * Mathf.Sin(angle),
                centerPosition.z
            );

            // Check if position is far enough from existing positions
            bool validPosition = true;
            foreach (Vector3 existingPos in positions)
            {
                if (Vector3.Distance(candidatePosition, existingPos) < minDistance)
                {
                    validPosition = false;
                    break;
                }
            }

            if (validPosition)
            {
                positions.Add(candidatePosition);
            }

            attempts++;
        }

        return positions;
    }

    private void SpawnZergAtPosition(Vector3 position)
    {
        if (ZergPool == null)
        {
            throw new System.Exception("ZergPool is not set. Please assign a ZergPool to the ClusterSpawnEvent.");
        }
        else
        {
            Zerg zerg = ZergPool.Get();
            zerg.Initialize(ZergPool);

            zerg.transform.SetPositionAndRotation(position, Quaternion.identity);
            zerg.transform.SetParent(ZergContainer);

            zerg.gameObject.name = $"zerg-{CurrentLevel.ZergCount}";
            Debug.Log($"Spawned zerg with index {CurrentLevel.ZergCount}");
            CurrentLevel.ZergCount++;

            if (zerg != null)
            {
                AnimationUtils.AnimateZergSpawn(zerg, spawnAnimationDuration);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 centerPosition = spawnCenter != null ? spawnCenter.position : transform.position;
        
        // Draw cluster boundary
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(centerPosition, clusterRadius);
        
        // Draw sample positions
        List<Vector3> positions = GetClusterSamplePositions(centerPosition, clusterRadius, numberOfZergs, spawnPointDistance);
        
        Gizmos.color = Color.red;
        foreach (Vector3 position in positions)
        {
            Gizmos.DrawWireSphere(position, 0.2f);
        }

        // Draw lines connecting positions that are within minimum distance
        Gizmos.color = Color.yellow;
        for (int i = 0; i < positions.Count; i++)
        {
            for (int j = i + 1; j < positions.Count; j++)
            {
                if (Vector3.Distance(positions[i], positions[j]) <= spawnPointDistance * 1.1f)
                {
                    Gizmos.DrawLine(positions[i], positions[j]);
                }
            }
        }
    }
}
