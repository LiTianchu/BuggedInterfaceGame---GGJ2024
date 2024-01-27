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

    }

}
