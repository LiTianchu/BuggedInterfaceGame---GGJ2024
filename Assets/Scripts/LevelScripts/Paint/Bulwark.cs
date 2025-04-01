using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bulwark : Heart
{
    public GameObject wallPrefab; // Wall prefab
    public float immunityDuration = 5.0f; // Duration of immunity
    public float wallDuration = 10.0f; // Duration before walls despawn

    protected override void StickmanCollides()
    {
        SpawnWalls();
    }

    protected override void ItemClicked()
    {
        if (stickman != null)
        {
            stickman.SetImmunity(immunityDuration);
        }
    }

    public void SpawnWalls()
    {
        Vector2 minBounds = stickman.GetMinBounds();
        Vector2 maxBounds = stickman.GetMaxBounds();
        List<GameObject> walls = new List<GameObject>();
        float defaultSize = 70;

        // Top wall
        walls.Add(InstantiateWall(new Vector2(0, maxBounds.y), new Vector2(maxBounds.x * 10, defaultSize)));
        // Bottom wall
        walls.Add(InstantiateWall(new Vector2(0, minBounds.y), new Vector2(maxBounds.x * 10, defaultSize)));
        // Left wall
        walls.Add(InstantiateWall(new Vector2(minBounds.x, 0), new Vector2(defaultSize, maxBounds.y * 10)));
        // Right wall
        walls.Add(InstantiateWall(new Vector2(maxBounds.x, 0), new Vector2(defaultSize, maxBounds.y * 10)));

        CoroutineManager.Instance.StartPersistentCoroutine(DespawnWalls(walls, wallDuration));
    }

    private GameObject InstantiateWall(Vector2 position, Vector2 size)
    {
        GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity, transform.parent);
        RectTransform wallRect = wall.GetComponent<RectTransform>();
        wallRect.sizeDelta = size;
        wallRect.anchoredPosition = position;
        return wall;
    }

    private IEnumerator DespawnWalls(List<GameObject> walls, float duration)
    {
        yield return new WaitForSeconds(duration);

        foreach (GameObject wall in walls)
        {
            Destroy(wall);
        }
    }
}