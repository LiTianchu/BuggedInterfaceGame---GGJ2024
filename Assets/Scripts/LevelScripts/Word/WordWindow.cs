using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using UnityEngine;

[RequireComponent(typeof(Draggable))]
public class WordWindow : MonoBehaviour
{
    [SerializeField] private Collectible collectiblePrefab;
    [SerializeField] private RectTransform collectibleSpawnParent;
    [SerializeField] private DoodleController doodleController;
    [SerializeField] private WordCage wordCage; // Reference to the WordCage component
    [SerializeField] private GameObject level;
    [SerializeField] private WordLevelGoal wordLevelGoal; // Reference to the WordLevelGoal component
    [SerializeField] private CameraFollower levelCamera; // Reference to the CameraFollower component
    private Draggable _draggable; // Reference to the Draggable component
    private RectTransform _rectTransform; // Reference to the RectTransform component
    private float _forceMultiplier = 0.05f; // Multiplier for the force applied based on drag delta
    private bool _levelStarted = false; // Flag to check if the level has started
    // Start is called before the first frame update
    void Start()
    {
        _draggable = GetComponent<Draggable>();
        _rectTransform = GetComponent<RectTransform>();
        if (_draggable != null)
        {
            _draggable.OnDragUpdate += HandleDragUpdate;
        }
        wordLevelGoal.OnGoalCollected += HanleGoalCollected; // Subscribe to the goal collected event
        wordCage.OnCageBroken += HandleCageBroken; // Subscribe to the cage broken event
        doodleController.OnLandOnGround += HandleLevelStart;
    }



    void OnDestroy()
    {
        if (_draggable != null)
        {
            _draggable.OnDragUpdate -= HandleDragUpdate; // Unsubscribe from the event to avoid memory leaks
        }
        wordLevelGoal.OnGoalCollected -= HanleGoalCollected; // Unsubscribe from the goal collected event
        wordCage.OnCageBroken -= HandleCageBroken; // Unsubscribe from the
        doodleController.OnLandOnGround -= HandleLevelStart; // Unsubscribe from the level start event
    }

    private void HandleCageBroken()
    {
        _forceMultiplier = 0.02f;
        doodleController.UpDownMovement = false; // Disable up-down movement
        doodleController.LeftRightMovement = true; // Enable left-right movement
        levelCamera.Following = true;
    }

    private void HandleDragUpdate(Vector2 dragDelta)
    {
        if (dragDelta.x < 0)
        {
            doodleController.PushLeft(Mathf.Abs(dragDelta.x) * _forceMultiplier);
            //doodleController.MoveLeft();
        }
        else if (dragDelta.x > 0)
        {
            doodleController.PushRight(Mathf.Abs(dragDelta.x) * _forceMultiplier);
            //doodleController.MoveRight();
        }

        if (dragDelta.y < 0)
        {
            doodleController.PushDown(Mathf.Abs(dragDelta.y) * _forceMultiplier);
            //doodleController.MoveDown();
        }
        else if (dragDelta.y > 0)
        {
            doodleController.PushUp(Mathf.Abs(dragDelta.y) * _forceMultiplier);
            //doodleController.MoveUp();
        }
    }

    private void HanleGoalCollected()
    {
        wordLevelGoal.OnGoalCollected -= HanleGoalCollected; // Unsubscribe to avoid memory leaks
        float spawnAnchorX = Mathf.Clamp(_rectTransform.anchoredPosition.x, -350f, 350f); // Clamp the spawn position within the level bounds
        float spawnAnchorY = _rectTransform.anchoredPosition.y + 500f;

        Collectible newCollectible = Instantiate(collectiblePrefab, collectibleSpawnParent);
        newCollectible.GetComponent<RectTransform>().anchoredPosition = new Vector2(spawnAnchorX, spawnAnchorY);
    }

    private void HandleLevelStart()
    {
        if (_levelStarted) { return; }

        DialogueManager.StopAllConversations(); // Stop any ongoing conversations
        DialogueManager.StartConversation("Started Word Level");
        
        _levelStarted = true; // Set the level started flag to true
        level.transform.DOLocalMoveX(0f, 1f).SetEase(Ease.OutBack); // Move the level to its starting position
    }

}
