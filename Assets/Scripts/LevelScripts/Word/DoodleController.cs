using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoodleController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private bool upDownMovement = true;
    [SerializeField] private bool leftRightMovement = true;

    public event Action OnLandOnGround;
    private Rigidbody2D _rb;
    private Collider2D _collider;
    public bool LeftRightMovement
    {
        get => leftRightMovement;
        set => leftRightMovement = value;
    }
    public bool UpDownMovement
    {
        get => upDownMovement;
        set => upDownMovement = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }
    public void PushUp(float force)
    {
        if (upDownMovement)
        {
            _rb.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
        }
    }
    public void PushDown(float force)
    {
        if (upDownMovement)
        {
            _rb.AddForce(new Vector2(0, -force), ForceMode2D.Impulse);
        }
    }
    public void PushLeft(float force)
    {
        if (leftRightMovement)
        {
            _rb.AddForce(new Vector2(-force, 0), ForceMode2D.Impulse);
        }
    }

    public void PushRight(float force)
    {
        if (leftRightMovement)
        {
            _rb.AddForce(new Vector2(force, 0), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (((1 << other.gameObject.layer) & platformLayer) != 0) // check if collided with platform layer
        {
            PlatformEffector2D platformEffector = other.gameObject.GetComponent<PlatformEffector2D>();
            if (platformEffector != null)
            {
                // check if the collision is from above
                // and apply jump force if so
                Vector2 contactNormal = other.contacts[0].normal;
                if (contactNormal.y > 0.5f)
                {
                    _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    MovingPlatform movingPlatform = other.gameObject.GetComponent<MovingPlatform>();
                    if (movingPlatform != null)
                    {
                        movingPlatform.OnPlayerTouchedPlatform();
                    }
                }
            }
        }

          if (other.gameObject.tag == "WordLevelGround")
        {
            OnLandOnGround?.Invoke();
        }
    }
}
