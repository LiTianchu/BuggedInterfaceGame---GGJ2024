using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Draggable))]
public class WordWindow : MonoBehaviour
{
    [SerializeField] private DoodleController doodleController;
    private Draggable _draggable; // Reference to the Draggable component


    // Start is called before the first frame update
    void Start()
    {
        _draggable = GetComponent<Draggable>();
        if (_draggable != null)
        {
            _draggable.OnDragUpdate += HandleDragUpdate;
        }
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


}
