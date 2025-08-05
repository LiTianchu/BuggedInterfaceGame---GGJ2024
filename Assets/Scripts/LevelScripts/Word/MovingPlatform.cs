using Unity.VisualScripting;
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
    [SerializeField] private float speed = 1f;
    [SerializeField] private float distance = 1f;
    [SerializeField] private bool randomizeStartPosition = false; // Randomize the starting position within the distance

    private Vector3 _startPosition; // Starting position of the platform
    private bool _movingForward = true; // Tracks the movement direction
    private RectTransform _rectTransform; // Reference to the RectTransform component
    private float _randomOffset; // Random offset for starting position

    void Start()
    {
        // Save the initial position of the platform
        _rectTransform = GetComponent<RectTransform>();
        if (_rectTransform == null)
        {
            _startPosition = transform.localPosition; // Fallback for non-UI elements
        }
        else
        {
            _startPosition = _rectTransform.anchoredPosition;
        }

        if (randomizeStartPosition)
        {
            _randomOffset = Random.Range(0f, 1f);
        }
    }

    void Update()
    {
        if (moveDirection != MoveDirection.NotMoving)
        {
            MovePlatform();
        }
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
        float displacement = Mathf.PingPong(Time.time * speed + _randomOffset, distance);

        displacement -= distance / 2; // shift it to center

        Vector3 finalDisplacement = direction * (_movingForward ? displacement : -displacement);


        if (_rectTransform == null)
        {
            transform.localPosition = _startPosition + finalDisplacement;
        }
        else
        {
            _rectTransform.anchoredPosition = _startPosition + finalDisplacement;
        }
    }
}

