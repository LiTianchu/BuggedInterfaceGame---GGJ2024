using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleSlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private LayerMask puzzlePieceLayer;
    [SerializeField]
    private PuzzlePiece correctPiece;
    [SerializeField]
    private AudioClip snapSound;
    [SerializeField]
    private bool acceptWrongPiece = true;

    private PuzzlePiece _puzzlePiece;

    public event Action<PuzzlePiece> OnPuzzlePieceRight;
    public event Action<PuzzlePiece> OnPuzzlePieceLeave;

    public void OnDrop(PointerEventData eventData)
    {

        //Debug.Log("Dropped on to puzzle slot " + this.name);
        if (_puzzlePiece == null)
        {
            Debug.Log("Puzzle slot is empty, snapping");
            //snap the puzzle to the grid
            _puzzlePiece = eventData.pointerDrag.GetComponent<PuzzlePiece>();

            if(_puzzlePiece == null){ return; }
            
            if (((1 << _puzzlePiece.gameObject.layer) & puzzlePieceLayer.value) == 0)
            {
                Debug.Log("Object is not on the puzzle piece layer");
                return;
            }
            _puzzlePiece.PuzzleSlot = this;
            _puzzlePiece.GravityController.DisableSimulation();
            _puzzlePiece.GetRectTransform().anchoredPosition = GetComponent<RectTransform>().localPosition;
            if (_puzzlePiece.Equals(correctPiece))
            {
                Debug.Log("Puzzle piece is in the right place");

                OnPuzzlePieceRight?.Invoke(_puzzlePiece);
                //crumblingPieces.SetRightPlaceFlag(_puzzlePiece, true);
            }
            else
            {
                Debug.Log("Puzzle piece is not in the right place");
                if (!acceptWrongPiece)
                {
                    _puzzlePiece.GravityController.EnableSimulation();
                    _puzzlePiece.PuzzleSlot = null;
                    _puzzlePiece = null;
                    return;
                }
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
        if (_puzzlePiece == null)
        {
            return;
        }

        OnPuzzlePieceLeave?.Invoke(_puzzlePiece);
        //_puzzlePiece.GravityController.EnableSimulation();
        //_puzzlePiece.PuzzleSlot = null;
       // _puzzlePiece = null;
        //crumblingPieces.SetRightPlaceFlag(_puzzlePiece, false);
    }

    public void SetPuzzlePiece(PuzzlePiece puzzlePiece)
    {
        _puzzlePiece = puzzlePiece;
    }



}
