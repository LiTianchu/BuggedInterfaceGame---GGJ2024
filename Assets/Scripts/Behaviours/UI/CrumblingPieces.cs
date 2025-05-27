
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPieces : MonoBehaviour
{

    [Tooltip("The list of pieces that will be crumbled")]
    [SerializeField]
    private List<GravityController2D> pieces;
    [SerializeField]
    private List<PuzzleSlot> puzzleSlots;
    [SerializeField]
    private float crumbleGravityScale = 100f;
    [SerializeField]
    private float crumbleTime = 2f;
    [SerializeField]
    private AudioClip crumbleSound;
    [SerializeField]
    private float minForce = -100f;
    [SerializeField]
    private float maxForce = 100f;

    private float _individualCrumbleDelay;
    private Dictionary<PuzzlePiece,bool> _rightPlaceMap; //maps puzzle pieces to whether or not they are in the right place

    public event System.Action OnPuzzlePieceRight;
    
    // Start is called before the first frame update
    void Start()
    {
          foreach (PuzzleSlot puzzleSlot in puzzleSlots)
        {
            puzzleSlot.OnPuzzlePieceRight += (puzzlePiece) =>
            {
                SetRightPlaceFlag(puzzlePiece, true);
            };

            puzzleSlot.OnPuzzlePieceLeave += (puzzlePiece) =>
            {
                SetRightPlaceFlag(puzzlePiece, false);
            };
        }
    }

    void OnEnable()
    {
        _individualCrumbleDelay = crumbleTime / pieces.Count;
        StartCoroutine(Crumble());
        Random.InitState((int)Time.time); //set the random seed to the current time

        _rightPlaceMap = new Dictionary<PuzzlePiece, bool>();
        foreach (GravityController2D p in pieces)
        {
            PuzzlePiece puzzlePiece = p.GetComponent<PuzzlePiece>();
            if (puzzlePiece != null)
            {
                _rightPlaceMap[puzzlePiece]= false;
            }
        }
    }

    public IEnumerator Crumble()
    {
        foreach (GravityController2D piece in pieces)
        {
            yield return new WaitForSeconds(_individualCrumbleDelay);
            piece.SetGravity(crumbleGravityScale);
            piece.UseGravity();
            piece.Push(VectorUtils.GetRandomForce2D(minForce, maxForce));
            piece.GetComponent<RectTransform>().Rotate(VectorUtils.GetRandomRotationAlongZ());

            if (crumbleSound != null)
            {
                AudioManager.Instance.PlaySFX(crumbleSound);
            }
        }
    }



    public void SetRightPlaceFlag(PuzzlePiece puzzlePiece, bool isRightPlace)
    {
        if(puzzlePiece == null)
        {
            return;
        }
        
        if (_rightPlaceMap.ContainsKey(puzzlePiece))
        {
            _rightPlaceMap[puzzlePiece] = isRightPlace;
        }

        if(isRightPlace)
        {
            CheckForRightPlace();
        }
    }

    private bool CheckForRightPlace()
    {
        foreach (KeyValuePair<PuzzlePiece, bool> entry in _rightPlaceMap)
        {
            if (!entry.Value)
            {
                return false;
            }
        }

        OnPuzzlePieceRight?.Invoke();
        return true;
    }
}
