using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Enum to define the movement direction
    public enum MoveDirection
    {
        NotMoving,
        UpDown,
        LeftRight,
        ForwardDiagonal,
        BackwardDiagonal

    }

    [Header("Movement Settings")]
    [SerializeField] private MoveDirection moveDirection = MoveDirection.UpDown; // Default: Up-Down
    [SerializeField] private float speed = 5f; // Speed of movement
    [SerializeField] private float distance = 10f; // Distance to move

    private Vector3 _startPosition; // Starting position of the platform
    private bool _movingForward = true; // Tracks the movement direction
    private RectTransform _rectTransform; // Reference to the RectTransform component

    void Start()
    {
        // Save the initial position of the platform
        _rectTransform = GetComponent<RectTransform>();
        if (_rectTransform == null)
        {
            _startPosition = transform.position; // Fallback for non-UI elements
        }
        else
        {
            _startPosition = _rectTransform.anchoredPosition;
        }
    }

    void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        // Determine the direction of movement
        Vector3 direction = Vector3.zero;
        switch (moveDirection)
        {
            case MoveDirection.UpDown:
                direction = Vector3.up;
                break;
            case MoveDirection.LeftRight:
                direction = Vector3.right;
                break;
            case MoveDirection.ForwardDiagonal:
                direction = new Vector3(1, 1, 0).normalized; // Diagonal up-right
                break;
            case MoveDirection.BackwardDiagonal:
                direction = new Vector3(-1, 1, 0).normalized; // Diagonal up-left
                break;
        }

        // Calculate the new position
        float displacement = Mathf.PingPong(Time.time * speed, distance);
        Vector3 finalDisplacement = direction * (_movingForward ? displacement : -displacement);

        if (_rectTransform == null)
        {
            // For non-UI elements, use transform.position
            transform.position = _startPosition + finalDisplacement;
        }
        else
        {
            // For UI elements, use RectTransform.anchoredPosition
            _rectTransform.anchoredPosition = _startPosition + finalDisplacement;
        }

        // // Flip movement direction at the extremes (optional for dynamic control)
        // if (displacement >= distance || displacement <= 0)
        // {
        //     _movingForward = !_movingForward;
        // }
    }
}

