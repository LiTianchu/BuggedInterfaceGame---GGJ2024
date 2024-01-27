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

        //_rectTransform = GetComponent<RectTransform>();
        //_parent = _rectTransform.parent; //record the parent
        //_rectTransform.parent = canvas.transform; //set the canvas as the parent

        // Debug.Log("OnBeginDrag");

        // //transform screen point to local rect transform point
        // Vector2 pivotPos;
        // RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventData.position, canvas.worldCamera, out pivotPos);

        // Debug.Log("Dragging UI Component Size: " + _rectTransform.rect.size);

        // //set the point as new pivot
        // Vector2 newPivot = _rectTransform.pivot + pivotPos * (Vector2.one / _rectTransform.rect.size); 

        // SetPivotWhileKeepingPos(newPivot); //keep the position of the element
        // Debug.Log("Dragging UI Component Pivot: (" + pivotPos.x + ", " + pivotPos.y + ")");
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Vector2 pos;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform.parent.transform as RectTransform, eventData.position, canvas.worldCamera, out pos);
        _rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        // Debug.Log("OnEndDrag");
        // //_rectTransform.parent = _parent; //set the parent back to the original parent
        // SetPivotWhileKeepingPos(new Vector2(0.5f, 0.5f)); // set pivot back to center
        
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

    // public void SetReferenceRectTransfrom(RectTransform rectTransform)
    // {
    //     _rectTransform = rectTransform;
    // }

    // private void SetPivotWhileKeepingPos(Vector2 newPivot) {
    //     Vector2 size = _rectTransform.rect.size;
    //     Vector2 deltaPivot = _rectTransform.pivot - newPivot; 
    //     Vector2 deltaPosition = new Vector2(deltaPivot.x * size.x, deltaPivot.y * size.y);
    //     _rectTransform.pivot = newPivot;
    //     _rectTransform.anchoredPosition -= deltaPosition;
    // }

    //to be used by children objects to define extra operations when dragging
    protected virtual void HandleBeginDrag() { }
    protected virtual void HandleEndDrag() { }
}
