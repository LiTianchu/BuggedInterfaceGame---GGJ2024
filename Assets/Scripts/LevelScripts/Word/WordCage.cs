using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WordCage : MonoBehaviour
{
    [SerializeField] private int maxHp = 20;
    [SerializeField] private LayerMask worldLevelPlayerLayer;
    [SerializeField] private ShakeAnim shakeAnim;
    [SerializeField] private GameObject cageParts;
    [SerializeField] private List<CagePartBatch> cagePartsBreakableBatches;

    private EdgeCollider2D _edgeCollider;
    private int _currentHp;
    public int CurrentHp { get => _currentHp; }
    private List<Rigidbody2D> _cageParts = new List<Rigidbody2D>();

    public event System.Action OnCageBroken;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in cageParts.transform)
        {
            Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                _cageParts.Add(rb);
            }
        }
        _edgeCollider = GetComponent<EdgeCollider2D>();
        _currentHp = maxHp; // Initialize current HP to max HP
    }

    public void TakeDamage(int damage)
    {
        _currentHp -= damage;

        CagePartBatch batchToRemove = null;

        foreach (CagePartBatch batch in cagePartsBreakableBatches)
        {
            if ((_currentHp / (float)maxHp) <= batch.hpThreshold)
            {
                foreach (Rigidbody2D rb in batch.cageParts)
                {
                    if (_cageParts.Contains(rb)) // remove if it is in the list to prevent error
                    {
                        _cageParts.Remove(rb);
                    }

                    BreakPart(rb); // Break the part
                }
                batchToRemove = batch;
                break;
            }
        }

        if (batchToRemove != null)
        {
            cagePartsBreakableBatches.Remove(batchToRemove);
        }

        if (_currentHp <= 0)
        {
            OnCageBroken?.Invoke();
            BreakCage();
        }
    }

    public void BreakCage()
    {
        foreach (Rigidbody2D rb in _cageParts)
        {
            BreakPart(rb);
        }
        _edgeCollider.enabled = false; // Disable the edge collider
    }

    public void BreakPart(Rigidbody2D rb)
    {
        rb.bodyType = RigidbodyType2D.Dynamic; // Change to dynamic to allow physics interaction
        rb.isKinematic = false; // Ensure the Rigidbody is not kinematic
        rb.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 5f, ForceMode2D.Impulse);
        //rb.GetComponent<Collider2D>().enabled = false;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if ((1 << collision.gameObject.layer & worldLevelPlayerLayer) != 0)
        {
            // Handle collision with player
            TakeDamage(1);
            shakeAnim.PlayOneShot(0.1f);
        }
    }

    [System.Serializable]
    public class CagePartBatch
    {
        public List<Rigidbody2D> cageParts;
        public float hpThreshold;

        public CagePartBatch(List<Rigidbody2D> cageParts, float hpThreshold)
        {
            this.cageParts = cageParts;
            this.hpThreshold = hpThreshold;
        }
    }
}
