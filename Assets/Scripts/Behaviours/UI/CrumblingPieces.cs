
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
        _individualCrumbleDelay = crumbleTime / pieces.Count;
        StartCoroutine(Crumble());
        Random.InitState((int)Time.time); //set the random seed to the current time

        _rightPlaceMap = new Dictionary<PuzzlePiece, bool>();
        foreach (GravityController2D p in pieces)
        {
            PuzzlePiece puzzlePiece = p.GetComponent<PuzzlePiece>();
            if (puzzlePiece != null)
            {
                _rightPlaceMap.Add(puzzlePiece, false);
            }
        }

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

    public IEnumerator Crumble()
    {
        foreach (GravityController2D piece in pieces)
        {
            yield return new WaitForSeconds(_individualCrumbleDelay);
            piece.SetGravity(crumbleGravityScale);
            piece.UseGravity();
            piece.Push(GetRandomForce());
            piece.GetComponent<RectTransform>().Rotate(GetRandomRotation());

            if (crumbleSound != null)
            {
                AudioManager.Instance.PlaySFX(crumbleSound);
            }
        }
    }

    private Vector2 GetRandomForce()
    {
        int xDirection = Random.Range(0, 2);
        int yDirection = Random.Range(0, 2);
        Vector2 force = new Vector2
        {
            x = Random.Range(minForce, maxForce) * (xDirection == 0 ? -1 : 1),
            y = Random.Range(minForce, maxForce) * (yDirection == 0 ? -1 : 1)
        };

        return force;

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

        //all pieces are in the right place
        //FindObjectOfType<Money>().CheckAmount();
    }

    private Vector3 GetRandomRotation()
    {
        return new Vector3(0, 0, Random.Range(-360f, 360f));
    }
}
