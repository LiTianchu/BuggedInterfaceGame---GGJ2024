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
    [SerializeField]
    private Canvas canvas;
    private RectTransform _rectTransform;
    private Camera _mainCamera;
    private RectTransform _canvasRect;

    public event Action OnDragBegin;
    public event Action OnDragEnd;
    public void Awake()
    {
        _mainCamera = Camera.main;
        _rectTransform = GetComponent<RectTransform>();
        _canvasRect = canvas.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //call begin drag procedure
        HandleBeginDrag();
        OnDragBegin?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // check if pointer is in the viewport
        if (RectTransformUtility.RectangleContainsScreenPoint(_canvasRect, Input.mousePosition, _mainCamera))
            _rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        else
        {
            // if pointer is not in the viewport, animate the object to the nearest edge
            Vector2 pos = _rectTransform.anchoredPosition;
            Vector2 canvasSize = _canvasRect.sizeDelta;
            Vector2 newPos = new(pos.x, pos.y);
            if (pos.x < -canvasSize.x / 2) // if object at left
                newPos.x = 0;
            if (pos.x > canvasSize.x / 2) // if object at right
                newPos.x = 0;
            if (pos.y < -canvasSize.y / 2) // if object at bottom
                newPos.y = 0;
            if (pos.y > canvasSize.y / 2) // if object at top
                newPos.y = 0;
            _rectTransform.DOAnchorPos(newPos, 0.5f);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //call end drag procedure
        HandleEndDrag();
        OnDragEnd?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //set the order in the hierarchy to the highest
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }

    public void SetCanvas(Canvas canvas)
    {
        this.canvas = canvas;
    }

    public Canvas GetCanvas()
    {
        return this.canvas;
    }

    public RectTransform GetRectTransform()
    {
        return _rectTransform;
    }

    //to be used by children objects to define extra operations when dragging
    protected virtual void HandleBeginDrag() { }
    protected virtual void HandleEndDrag() { }
}
