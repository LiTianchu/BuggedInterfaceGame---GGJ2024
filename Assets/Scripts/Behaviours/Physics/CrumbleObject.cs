using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CrumbleObject : MonoBehaviour
{
    [SerializeField] private float crumbleForce = 1f;
    [SerializeField] private float gravityScale = 2f;
    [SerializeField] private bool crumbleOnStart = false;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (crumbleOnStart)
        {
            Crumble();
        }
        else
        {
            _rb.simulated = false;
        }
    }

    public void Crumble()
    {
        if(!gameObject.activeInHierarchy)
        {
            return;
        }

        transform.DOShakePosition(0.5f, 30f, 40, 90, false, true).OnComplete(() =>
        {

            if (_rb != null)
            {
                _rb.simulated = true;
                Vector2 force = VectorUtils.GetRandomForce2D(-crumbleForce, crumbleForce);
                _rb.gravityScale = gravityScale;
                _rb.AddForce(force, ForceMode2D.Impulse);
            }
        });
    }
}
