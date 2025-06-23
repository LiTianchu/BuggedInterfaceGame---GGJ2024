using UnityEngine;
using UnityEngine.InputSystem;

public class DoodleController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private LayerMask platformLayer;

    private Rigidbody2D _rb;
    private Collider2D _collider;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Keyboard.current.aKey.wasPressedThisFrame)
        // { // move left
        //     MoveLeft();
        // }
        // else if (Keyboard.current.dKey.wasPressedThisFrame)
        // { // move right
        //     MoveRight();
        // }
     
    }

    public void MoveLeft()
    {
        _rb.velocity = new Vector2(-moveSpeed, _rb.velocity.y);
    }

    public void MoveRight()
    {
        _rb.velocity = new Vector2(moveSpeed, _rb.velocity.y);
    }

    public void PushLeft(float force)
    {
        _rb.AddForce(new Vector2(-force, 0), ForceMode2D.Impulse);
    }

    public void PushRight(float force)
    {
        _rb.AddForce(new Vector2(force, 0), ForceMode2D.Impulse);
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
                }
            }
        }
    }
}
