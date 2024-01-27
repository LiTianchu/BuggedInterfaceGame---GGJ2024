using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleSlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private LayerMask puzzlePieceLayer;

    private PuzzlePiece _puzzlePiece;
    public void OnDrop(PointerEventData eventData)
    {
    
        Debug.Log("Dropped on to puzzle slot " + this.name);
        if(_puzzlePiece == null){
            Debug.Log("Puzzle slot is empty, snapping");
            //snap the puzzle to the grid
            _puzzlePiece = eventData.pointerDrag.GetComponent<PuzzlePiece>();
            _puzzlePiece.PuzzleSlot = this;
            _puzzlePiece.GravityController.DisableSimulation();
            _puzzlePiece.GetRectTransform().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }else{
            Debug.Log("Puzzle slot is not empty");
            _puzzlePiece.GravityController.EnableSimulation();
            _puzzlePiece.PuzzleSlot = null;
            return;
    }
    }

    public void SetPuzzlePiece(PuzzlePiece puzzlePiece){
        _puzzlePiece = puzzlePiece;
    }

    

}
