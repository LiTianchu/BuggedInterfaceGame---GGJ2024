using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityController2D : MonoBehaviour
{
    [SerializeField]
    private float intialGravityScale;
    private Rigidbody2D _rigidbody;
    private float _gravityScale;
    private void OnEnable() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _gravityScale = intialGravityScale;
        UseGravity();
        //ApplyForce(50000*Vector2.left);
    }
    public void SetGravity(float gravityScale)
    {
        _gravityScale = gravityScale;
    }

    public void UseGravity()
    {
        _rigidbody.gravityScale = _gravityScale;
    }

    public void DisableSimulation()
    {
        _rigidbody.simulated = false;
    }

    public void EnableSimulation()
    {
        _rigidbody.simulated = true;
    }

    public void Freeze()
    {
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Unfreeze()
    {
        _rigidbody.constraints = RigidbodyConstraints2D.None;
    }

    public void Push(Vector2 force)
    {
        //Debug.Log("Applying force: " + force);
        _rigidbody.AddForce(force, ForceMode2D.Impulse);
    }
}
