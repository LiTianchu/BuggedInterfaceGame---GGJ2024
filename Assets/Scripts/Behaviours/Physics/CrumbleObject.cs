using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CrumbleObject : MonoBehaviour
{
    [SerializeField] private float crumbleForce = 1f;
    [SerializeField] private float gravityScale = 2f;
    [SerializeField] private bool crumbleOnStart = false;
    [SerializeField] private bool turnOffCollisionWhenCrumbling = true;
    private Rigidbody2D _rb;
    private Collider2D _collider;

    public event System.Action OnOffScreen;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        if (crumbleOnStart)
        {
            Crumble();
        }
    }


    public void Crumble()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        if (turnOffCollisionWhenCrumbling && _collider != null)
        {
            _collider.enabled = false;
        }

        transform.DOShakePosition(5f, 30f, 40, 90, false, true).OnComplete(() =>
        {

            if (_rb == null)
            {
                AddTempRigidbody();
            }

            _rb.simulated = true;
            Vector2 force = VectorUtils.GetRandomForce2D(-crumbleForce, crumbleForce);
            _rb.gravityScale = gravityScale;
            _rb.AddForce(force, ForceMode2D.Impulse);
        });
    }

    public void AddTempRigidbody()
    {
        if (_rb == null)
        {
            _rb = gameObject.AddComponent<Rigidbody2D>();
        }
    }
}
