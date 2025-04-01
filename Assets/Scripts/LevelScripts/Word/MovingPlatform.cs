using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Enum to define the movement direction
    public enum MoveDirection
    {
        UpDown,
        LeftRight
    }

    [Header("Movement Settings")]
    [SerializeField] private MoveDirection moveDirection = MoveDirection.UpDown; // Default: Up-Down
    [SerializeField] private float speed = 2f; // Speed of movement
    [SerializeField] private float distance = 3f; // Distance to move

    private Vector3 startPosition; // Starting position of the platform
    private bool movingForward = true; // Tracks the movement direction

    void Start()
    {
        // Save the initial position of the platform
        startPosition = transform.position;
    }

    void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        // Determine the direction of movement
        Vector3 direction = moveDirection == MoveDirection.UpDown ? Vector3.up : Vector3.right;

        // Calculate the new position
        float displacement = Mathf.PingPong(Time.time * speed, distance);
        transform.position = startPosition + direction * (movingForward ? displacement : -displacement);

        // Flip movement direction at the extremes (optional for dynamic control)
        if (displacement >= distance || displacement <= 0)
        {
            movingForward = !movingForward;
        }
    }
}

