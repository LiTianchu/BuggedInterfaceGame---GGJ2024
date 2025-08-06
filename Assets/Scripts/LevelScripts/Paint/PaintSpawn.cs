using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.UI;

public class PaintSpawn : MonoBehaviour
{

    public GameObject stickmanPrefab; // Reference to the Stickman prefab
    public RectTransform enemyPowerContainer;
    public RectTransform playerPowerContainer;
    public Canvas mainCanvas; // Reference to the main canvas
    public GameObject desktopGUIOverlay;
    public GameObject levelFailedOverlay; // Reference to the level failed overlay
    public bool spawnImmediately = true; // Flag to determine if the Stickman should spawn immediately
    public PuzzleSlot puzzleSlot; // Reference to the PuzzleSlot component
    public Color levelFailedColor = Color.red; // Color to flash when the level fails

    private Stickman _spawnedStickman; // Reference to the spawned Stickman
    private bool _levelCompleted = false; // Flag to check if the level is completed

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
        if (!_levelCompleted && spawnImmediately) // Check if the level is not completed and if we should spawn immediately
        {
            SpawnStickman();
        }
    }

    void OnDisable()
    {
        if (_spawnedStickman != null)
        {
            RemoveSpawnedEnemyPower(); // Remove all spawned objects in the enemy power container
            RemoveSpawnedPlayerPower(); // Remove all spawned objects in the player power container
            Destroy(_spawnedStickman.gameObject);
            _spawnedStickman.OnLevelFailed -= HandleLevelFailed; // Unsubscribe from the level failed event
            _spawnedStickman.OnLevelCompleted -= HandleLevelCompleted; // Unsubscribe from the level completed event
            _spawnedStickman = null;
            //puzzleSlot.ReleasePuzzlePiece();
        }
        desktopGUIOverlay.SetActive(false); // Deactivate the desktop GUI overlay
    }

    private void HandleLevelCompleted()
    {
        _levelCompleted = true; // Set the level completed flag to true
        desktopGUIOverlay.SetActive(false); // Deactivate the desktop GUI overlay
        DialogueManager.StopAllConversations(); // replace
        DialogueManager.StartConversation("Finished Paint Level"); // Start the conversation for level completion
        RemoveSpawnedEnemyPower(); // Remove all spawned objects in the enemy power container
        RemoveSpawnedPlayerPower(); // Remove all spawned objects in the player power container
        RemovePuzzle(); // Remove the puzzle slot
    }

    // Method to spawn the Stickman object
    public void SpawnStickman()
    {

        if (_spawnedStickman == null && stickmanPrefab != null && mainCanvas != null)
        {
            spawnImmediately = true; // after the first spawn, we can set this to true
            desktopGUIOverlay.SetActive(true); // Activate the desktop GUI overlay
            GameObject stickman = Instantiate(stickmanPrefab, mainCanvas.transform);
            stickman.transform.localPosition = Vector3.zero; // Adjust position as needed
            _spawnedStickman = stickman.GetComponent<Stickman>();
            _spawnedStickman.Initialize(mainCanvas, enemyPowerContainer, playerPowerContainer); // Initialize the Stickman with the main canvas
            _spawnedStickman.OnLevelFailed += HandleLevelFailed; // Subscribe to the level failed event
            _spawnedStickman.OnLevelCompleted += HandleLevelCompleted; // Subscribe to the level completed event
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

    private void RemovePuzzle()
    {
        if (puzzleSlot != null)
        {
            puzzleSlot.gameObject.SetActive(false); // Deactivate the PuzzleSlot object
        }
    }

    private void HandleLevelFailed()
    {
        UnityEngine.UI.Image overlayImage = levelFailedOverlay.GetComponent<UnityEngine.UI.Image>();
        levelFailedOverlay.SetActive(true); // Activate the level failed overlay
        if (overlayImage != null)
        {
            // Flash the image color directly - no material issues
            DOTween.Sequence()
                .Append(overlayImage.DOColor(levelFailedColor, 1f)).SetEase(Ease.OutQuad)
                .Append(overlayImage.DOColor(Color.white, 0.05f))
                .OnComplete(() =>
                {
                    levelFailedOverlay.SetActive(false); // Deactivate the overlay after flashing
                    gameObject.SetActive(false);
                });
        }
       
    }

    
}
