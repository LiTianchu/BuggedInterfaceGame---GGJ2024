using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPieces : MonoBehaviour
{

    [Tooltip("The list of pieces that will be crumbled")]
    [SerializeField]
    private List<GravityController2D> pieces;
    [SerializeField]
    private float crumbleGravityScale = 100f;
    [SerializeField]
    private float crumbleTime = 2f;
    [SerializeField]
    private AudioClip crumbleSound;

    private float _individualCrumbleDelay;
    // Start is called before the first frame update
    void Start()
    {
        _individualCrumbleDelay = crumbleTime / pieces.Count;
        StartCoroutine(Crumble());
        int xDirection = Random.Range(0, 2);
        int yDirection = Random.Range(0, 2);
        Random.InitState((int)Time.time);
        Vector2 force = new Vector2
        {
            x = Random.Range(-1000f, 1000f) * (xDirection == 0 ? -1 : 1),
            y = Random.Range(-1000f, 1000f) * (yDirection == 0 ? -1 : 1)
        };
        

        
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
            x = Random.Range(-1000f, 1000f) * (xDirection == 0 ? -1 : 1),
            y = Random.Range(-1000f, 1000f) * (yDirection == 0 ? -1 : 1)
        };

        return force;

    }

    private Vector3 GetRandomRotation()
    {
        return new Vector3(0, 0, Random.Range(-360f, 360f));
    }
}
