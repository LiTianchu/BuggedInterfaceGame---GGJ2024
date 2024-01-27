using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

//This is a class controls the dragging behaviour of UI object
//Attach this to a UI object and assign the UI's canvas to the canvas field
public class Draggable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [SerializeField]
    [Tooltip("The canvas that this UI component belongs to")]
    private Canvas canvas;
    private RectTransform _rectTransform;
    //private Transform _parent;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //call begin drag procedure
        HandleBeginDrag();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //call end drag procedure
        HandleEndDrag();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //set the order in the hierarchy to the highest
        transform.SetSiblingIndex(transform.parent.childCount-1);
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
