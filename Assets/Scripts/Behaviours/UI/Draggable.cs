using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

//This is a class controls the dragging behaviour of UI object
//Attach this to a UI object and assign the UI's canvas to the canvas field
//Need to set the canvas to world space overlay
public class Draggable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [Required]
    [Tooltip("The canvas that this UI component belongs to")]
    [SerializeField] private Canvas canvas;
    [SerializeField] protected bool stayInView = true;
    [SerializeField] private bool isDraggable = true;
    [SerializeField] private bool moveToFrontOnDrag = true;
    protected RectTransform _rectTransform;
    private Camera _mainCamera;
    private RectTransform _canvasRect;

    public bool IsDraggable { get => isDraggable; set => isDraggable = value; }
    public event Action OnDragBegin;
    public event Action OnDragEnd;
    public event Action<Vector2> OnDragUpdate;

    public void Awake()
    {
        _mainCamera = Camera.main;
        _rectTransform = GetComponent<RectTransform>();
        _canvasRect = canvas.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDraggable) { return; }
        //call begin drag procedure
        HandleBeginDrag();
        OnDragBegin?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable) { return; }
        Vector2 initialAnchorPos = _rectTransform.anchoredPosition;
        _rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        Vector2 deltaAnchorPos = _rectTransform.anchoredPosition - initialAnchorPos;


        HandleDrag();
        OnDragUpdate?.Invoke(deltaAnchorPos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable) { return; }

        if (stayInView && IsRectTransformCenterOffCanvas())
        {
            Vector2 pos = _rectTransform.anchoredPosition;
            Vector2 canvasSize = _canvasRect.sizeDelta;

            Vector2 clampedPos = new Vector2(
                Mathf.Clamp(pos.x, -canvasSize.x / 2f, canvasSize.x / 2f),
                Mathf.Clamp(pos.y, -canvasSize.y / 2f, canvasSize.y / 2f)
            );

            _rectTransform.DOAnchorPos(clampedPos, 0.25f).SetEase(Ease.OutBack);
        }

        //call end drag procedure
        HandleEndDrag();
        OnDragEnd?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        HandleOnPointerDown();
        if (!isDraggable) { return; }
        if (moveToFrontOnDrag)
        {
            //set the order in the hierarchy to the highest
            transform.SetSiblingIndex(transform.parent.childCount - 1);
        }
    }

    public void SetCanvas(Canvas canvas)
    {
        this.canvas = canvas;
    }

    public Canvas GetCanvas()
    {
        return this.canvas;
    }

    private bool IsRectTransformCenterOffCanvas()
    {
        // Get the center position of the RectTransform relative to the canvas
        Vector2 localCenter = _rectTransform.anchoredPosition;

        // Get canvas bounds (assuming canvas is anchored at center)
        Vector2 canvasSize = _canvasRect.sizeDelta;
        float halfWidth = canvasSize.x / 2f;
        float halfHeight = canvasSize.y / 2f;

        // Check if center is outside canvas bounds
        return localCenter.x < -halfWidth || localCenter.x > halfWidth ||
               localCenter.y < -halfHeight || localCenter.y > halfHeight;
    }

    public RectTransform GetRectTransform()
    {
        return _rectTransform;
    }

    //to be used by children objects to define extra operations when dragging
    protected virtual void HandleBeginDrag() { }
    protected virtual void HandleEndDrag() { }
    protected virtual void HandleDrag() { }
    protected virtual void HandleOnPointerDown() { }
}
