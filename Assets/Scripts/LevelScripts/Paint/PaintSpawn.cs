using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintSpawn : MonoBehaviour
{

    public GameObject stickmanPrefab; // Reference to the Stickman prefab
    public RectTransform enemyPowerContainer;
    public RectTransform playerPowerContainer;
    public Canvas mainCanvas; // Reference to the main canvas

    private Stickman _spawnedStickman; // Reference to the spawned Stickman

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // This method is called when the object becomes enabled and active
    void OnEnable()
    {
        SpawnStickman();
    }

    void OnDisable()
    {
        if (_spawnedStickman != null)
        {
            RemoveSpawnedEnemyPower(); // Remove all spawned objects in the enemy power container
            RemoveSpawnedPlayerPower(); // Remove all spawned objects in the player power container
            Destroy(_spawnedStickman.gameObject);
            _spawnedStickman.OnLevelFailed -= HandleLevelFailed; // Unsubscribe from the level failed event
            _spawnedStickman = null;
        }
    }

    // Method to spawn the Stickman object
    void SpawnStickman()
    {
        if (_spawnedStickman == null && stickmanPrefab != null && mainCanvas != null)
        {
            GameObject stickman = Instantiate(stickmanPrefab, mainCanvas.transform);
            stickman.transform.localPosition = Vector3.zero; // Adjust position as needed
            _spawnedStickman = stickman.GetComponent<Stickman>();
            _spawnedStickman.Initialize(mainCanvas, enemyPowerContainer, playerPowerContainer); // Initialize the Stickman with the main canvas
            _spawnedStickman.OnLevelFailed += HandleLevelFailed; // Subscribe to the level failed event
        }
    }

    private void RemoveSpawnedEnemyPower()
    {
        foreach (Transform child in enemyPowerContainer)
        {
            Destroy(child.gameObject); // Destroy all child objects in the bullet container
        }
    }
    private void RemoveSpawnedPlayerPower()
    {
        foreach (Transform child in playerPowerContainer)
        {
            Destroy(child.gameObject); // Destroy all child objects in the bullet container
        }
    }
    private void HandleLevelFailed()
    {
        gameObject.SetActive(false); // Deactivate the PaintSpawn object when the level fails
    }
}
