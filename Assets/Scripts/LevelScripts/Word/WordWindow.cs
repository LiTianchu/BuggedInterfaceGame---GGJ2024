using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Draggable))]
public class WordWindow : MonoBehaviour
{
    [SerializeField] private Collectible collectiblePrefab;
    [SerializeField] private RectTransform collectibleSpawnParent;
    [SerializeField] private DoodleController doodleController;
    [SerializeField] private WordLevelGoal wordLevelGoal; // Reference to the WordLevelGoal component
    private Draggable _draggable; // Reference to the Draggable component
    private RectTransform _rectTransform; // Reference to the RectTransform component

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
    }


    void OnDestroy()
    {
        if (_draggable != null)
        {
            _draggable.OnDragUpdate -= HandleDragUpdate; // Unsubscribe from the event to avoid memory leaks
        }
    }

    private void HandleDragUpdate(Vector2 dragDelta)
    {
        if (dragDelta.x < 0)
        {
            doodleController.PushLeft(Mathf.Abs(dragDelta.x) * 0.02f);
            //doodleController.MoveLeft();
        }
        else if (dragDelta.x > 0)
        {
            doodleController.PushRight(Mathf.Abs(dragDelta.x) * 0.02f);
            //doodleController.MoveRight();
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

}
