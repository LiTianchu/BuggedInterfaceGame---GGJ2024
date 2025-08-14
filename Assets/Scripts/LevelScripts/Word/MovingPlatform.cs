using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private int durability = -1; // -1 means infinite durability, otherwise it will be destroyed after this many hits
    [SerializeField] private LayerMask playerLayerMask; // Layer mask to check for player collision
    [SerializeField] private bool powerJump = false;
    [SerializeField] private float powerMultiplier = 1.5f; // Multiplier for the force applied to the player
    [Header("Events")]
    [SerializeField] private UnityEvent onPlayerTouchedPlatform; // Event to trigger when the player touches the platform
    [SerializeField] private int maxNumOfEventTrigger = 1;

    private Vector3 _startPosition; // Starting position of the platform
    private bool _movingForward = true; // Tracks the movement direction
    private RectTransform _rectTransform; // Reference to the RectTransform component
    private float _randomOffset; // Random offset for starting position
    private TMP_Text _text;
    private SpriteRenderer _platformSpriteRenderer;
    private PlatformEffector2D _platformEffector;
    private int _eventTriggerCount = 0; // Counter for the number of times the event has been triggered

    public bool PowerJump { get => powerJump; }
    public float PowerMultiplier { get => powerMultiplier; }
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

        _text = GetComponentInChildren<TMP_Text>();
        _platformSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _platformEffector = GetComponent<PlatformEffector2D>();

        if (durability > 0) // if the platform has durability, change the color and style
        {
            _text.color = new Color(116 / 255f, 122 / 255f, 50 / 255f);
            _platformSpriteRenderer.color = new Color(116 / 255f, 122 / 255f, 50 / 255f);
            _text.fontStyle = FontStyles.Strikethrough;
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

    public void OnPlayerTouchedPlatform()
    {
        if (durability > 0)
        {
            durability--;
            if (durability <= 0)
            {
                DestroyPlatform();
            }
        }

        if(_eventTriggerCount<maxNumOfEventTrigger)
        {
            _eventTriggerCount++;
            onPlayerTouchedPlatform.Invoke(); // Trigger the event
        }
    }

    private void DestroyPlatform()
    {
        transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => Destroy(gameObject));
    }

}

