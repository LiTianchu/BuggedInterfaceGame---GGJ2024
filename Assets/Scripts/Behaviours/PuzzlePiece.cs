using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(GravityController2D))]
public class PuzzlePiece : Draggable
{
    [SerializeField]
    private LayerMask puzzleGridLayer;
    private GravityController2D _gravityController;
    private CanvasGroup _canvasGroup;
    private PuzzleSlot _puzzleSlot;

    public PuzzleSlot PuzzleSlot { get { return _puzzleSlot; } set { _puzzleSlot = value; } }
    public GravityController2D GravityController { get { return _gravityController; } }
    //HashSet<Collider2D> _nearbySlots = new HashSet<Collider2D>();
    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _gravityController = GetComponent<GravityController2D>();
    }
    protected override void HandleBeginDrag()
    {
        GetRectTransform().rotation = Quaternion.identity;
        _canvasGroup.blocksRaycasts = false;
        _gravityController.DisableSimulation();
        if (_puzzleSlot != null)
        {
            _puzzleSlot.SetPuzzlePiece(null);
        }
    }

    protected override void HandleEndDrag()
    {
        if(_puzzleSlot == null){
            _gravityController.EnableSimulation();
        }
        _canvasGroup.blocksRaycasts = true;
        // if(_nearbySlots.Count == 0){
        //     Debug.Log("Puzzle piece is not over the puzzle grid");
        //     return;
        // }

        // //get the nearest slot
        // Collider2D nearestSlot = _nearbySlots.GetEnumerator().Current;
        // foreach(Collider2D slot in _nearbySlots){
        //     if(Vector2.Distance(slot.transform.position, transform.position) < Vector2.Distance(nearestSlot.transform.position, transform.position)){
        //         nearestSlot = slot;
        //     }
        // }

        // //snap the puzzle to the grid
        // GetRectTransform().anchoredPosition = nearestSlot.GetComponent<RectTransform>().anchoredPosition;
        // RectTransform rect = GetRectTransform();
        // //check if the piece is over the puzzle grid
        // Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, rect.rect.size, 0, puzzleGridLayer);

        // if (colliders.Length == 0)
        // {
        //     return;
        // }

        // Collider2D nearestCollider = null;
        // float nearestDistance = float.MaxValue;
        // foreach (Collider2D coll in colliders)
        // {
        //     Debug.Log("Checking puzzle grid: " + coll.name);
        //     //chech if the puzzle grid is empty
        //     RectTransform gridRect = coll.GetComponent<RectTransform>();
        //     LayerMask thisLayer = 1 << gameObject.layer;

        //     foreach (Collider2D collider in collidersInGrid)
        //     {
        //         Debug.Log("Collider in grid: " + collider.name);
        //     }
        //     //if the puzzle grid has only one collider, it means it is the puzzle piece itself
        //     if (Physics2D.OverlapBoxAll(coll.transform.position,
        //         coll.bounds.size, 0,puzzlePieceLayer).Length<=1)
        //     {
        //         //check if the puzzle piece is closer to the grid than the previous one
        //         float distance = Vector2.Distance(gridRect.anchoredPosition, rect.anchoredPosition);
        //         if (distance < nearestDistance)
        //         {
        //             nearestDistance = distance; //update the nearest distance
        //             nearestCollider = coll; //update the nearest collider
        //         }

        //     }else{
        //         Debug.Log("Puzzle grid is not empty");
        //     }
        // }
        // //snap the puzzle to the grid
        // if(nearestCollider == null){
        //     Debug.Log("No empty puzzle grid found");
        //     return;
        // }
        // rect.anchoredPosition = nearestCollider.GetComponent<RectTransform>().anchoredPosition;
        // _gravityController.DisableSimulation();
        // GetRectTransform().rotation = Quaternion.identity;

    }



    // private void OnTriggerEnter2D(Collider2D other) {
    //     //Debug.Log(other.gameObject.layer + " " + puzzleGridLayer.value);
    //     if(((1<<other.gameObject.layer) & puzzleGridLayer) != 0){
    //         Debug.Log("Puzzle piece is over the puzzle grid");
    //         _nearbySlots.Add(other);
    //     }
    // }

    // private void OnTriggerExit2D(Collider2D other) {
    //     if(((1<<other.gameObject.layer) & puzzleGridLayer) != 0){
    //         Debug.Log("Puzzle piece is no longer over the puzzle grid");
    //         _nearbySlots.Remove(other);
    //     }
    // }

}
