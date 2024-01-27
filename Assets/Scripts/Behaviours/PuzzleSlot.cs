using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleSlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private CrumblingPieces crumblingPieces;
    [SerializeField]
    private LayerMask puzzlePieceLayer;
    [SerializeField]
    private PuzzlePiece correctPiece;
    [SerializeField]
    private AudioClip snapSound;

    private PuzzlePiece _puzzlePiece;
    public void OnDrop(PointerEventData eventData)
    {

        //Debug.Log("Dropped on to puzzle slot " + this.name);
        if (_puzzlePiece == null)
        {
            Debug.Log("Puzzle slot is empty, snapping");
            //snap the puzzle to the grid
            _puzzlePiece = eventData.pointerDrag.GetComponent<PuzzlePiece>();
            if (((1 << _puzzlePiece.gameObject.layer) & puzzlePieceLayer.value) == 0)
            {
                Debug.Log("Object is not on the puzzle piece layer");
                return;
            }
            _puzzlePiece.PuzzleSlot = this;
            _puzzlePiece.GravityController.DisableSimulation();
            _puzzlePiece.GetRectTransform().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            if (_puzzlePiece.Equals(correctPiece))
            {
                Debug.Log("Puzzle piece is in the right place");
                crumblingPieces.SetRightPlaceFlag(_puzzlePiece, true);
            }
            if (snapSound != null)
            {
                AudioManager.Instance.PlaySFX(snapSound);
            }
        }
        else
        {
            Debug.Log("Puzzle slot is not empty");
            _puzzlePiece.GravityController.EnableSimulation();
            _puzzlePiece.PuzzleSlot = null;
            return;
        }
    }

    public void OnPuzzleLeave()
    {
        if(_puzzlePiece == null)
        {
            return;
        }
        crumblingPieces.SetRightPlaceFlag(_puzzlePiece, false);
    }

    public void SetPuzzlePiece(PuzzlePiece puzzlePiece)
    {
        _puzzlePiece = puzzlePiece;
    }



}
