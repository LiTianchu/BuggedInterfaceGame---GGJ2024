using Sirenix.OdinInspector;
using UnityEngine;

public class DraggableWorldSpace : MonoBehaviour
{

    [SerializeField] private LayerMask dropAreaLayer;
    [Required]
    [SerializeField] private GridSystem draggableAreaObj;
    [SerializeField] private bool isDraggable = true;

    private Vector3 _offset;
    private Collider2D _collider2d;
    private Camera _mainCamera;
    private IDraggableArea _draggableArea;
    private DropArea[] _dropAreas;
    private DropArea _currentDropArea;
    private bool _isInitialized = false;

    public DropArea CurrentDropArea { get => _currentDropArea; }
    public GridSystem DraggableAreaObj { get => draggableAreaObj; }
    void Awake()
    {
        _collider2d = GetComponent<Collider2D>();
        _mainCamera = Camera.main;

    }

    private void Start()
    {
        _draggableArea = draggableAreaObj.GetComponent<IDraggableArea>();
        _dropAreas = draggableAreaObj.GetComponentsInChildren<DropArea>();
        if (_draggableArea == null)
        {
            throw new System.Exception("DraggableAreaObj must have a component that implements IDraggableArea interface");
        }
        
    }
    private void Update()
    {
        if(!_isInitialized)
        {
            DropObject();
        }
        //Debug.Log(CurrentDropArea?.CurrentDraggable == null);
        if (_currentDropArea == null)
        {
            Collider2D[] hitInfo = GetSurroundingDropArea();
            Collider2D nearestDropArea = DetectNearestDropArea(hitInfo);
            DropArea nearestDropAreaComponent = null;
            if (nearestDropArea != null)
            {
                nearestDropAreaComponent = nearestDropArea.GetComponent<DropArea>();
                if (draggableAreaObj != null)
                {
                    draggableAreaObj.HighlightDropArea(nearestDropAreaComponent);
                }
            }
        }
    }
    private void OnMouseDown()
    {
        if (!isDraggable)
        {
            return;
        }
        _offset = transform.position - MouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        if(!isDraggable)
        {
            return;
        }
        GetComponent<SpriteRenderer>().sortingOrder = 99;
        UnbindDropArea();

        Vector2 newPos = MouseWorldPosition() + _offset;
        if (draggableAreaObj != null)
        {

            Vector2[] boundPoints = _draggableArea.GetBoundPoints();
            float halfWidth = _collider2d.bounds.size.x / 2;
            float halfHeight = _collider2d.bounds.size.y / 2;

            newPos.x = Mathf.Clamp(newPos.x, boundPoints[0].x + halfWidth, boundPoints[1].x - halfWidth);
            newPos.y = Mathf.Clamp(newPos.y, boundPoints[0].y + halfWidth, boundPoints[1].y - halfHeight);
        }
        transform.position = newPos;
    }
    private void OnMouseUp()
    {
        if (!isDraggable)
        {
            return;
        }
        GetComponent<SpriteRenderer>().sortingOrder = 1;
        DropObject();   
    }

    public void DropObject(){
        Collider2D nearestDropArea = DetectNearestDropArea();
        if (nearestDropArea != null)
        {
            _isInitialized = true;
            transform.position = nearestDropArea.transform.position + new Vector3(0, 0, -1f);
            BindDropArea(nearestDropArea.GetComponent<DropArea>());
        }
    }

    private Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = _mainCamera.WorldToScreenPoint(transform.position).z;
        return _mainCamera.ScreenToWorldPoint(mouseScreenPos);
    }

    private Collider2D DetectNearestDropArea(Collider2D[] hitInfo = null)
    {
        if (hitInfo == null)
        {
            hitInfo = GetSurroundingDropArea();
        }

        if (hitInfo.Length > 0)
        {
            Collider2D nearestDropArea = null;
            for (int i = 0; i < hitInfo.Length; i++)
            {
                //Debug.Log(i + ":" + hitInfo[i].GetComponent<DropArea>().CurrentDraggable?.name);
                if (hitInfo[i].GetComponent<DropArea>().CurrentDraggable != null)
                {
                    continue;
                }

                if (nearestDropArea == null)
                {
                    nearestDropArea = hitInfo[i];
                }
                else if (Vector2.Distance(hitInfo[i].transform.position, transform.position) <
                Vector2.Distance(nearestDropArea.transform.position, transform.position))
                {
                    nearestDropArea = hitInfo[i];
                }
            }
            return nearestDropArea;
            // DraggableWorldSpace currentDragObj = nearestDropArea.GetComponent<DropArea>().CurrentDraggable;
            // return currentDragObj == null || currentDragObj == this ? nearestDropArea : null;
        }
        return null;

    }

    private Collider2D[] GetSurroundingDropArea()
    {
        Collider2D[] hitInfo = Physics2D.OverlapBoxAll(transform.position, _collider2d.bounds.size, 0f, dropAreaLayer.value);

        return hitInfo;

    }

    private void BindDropArea(DropArea dropArea)
    {
        _currentDropArea = dropArea;
        _currentDropArea.CurrentDraggable = this;

    }

    private void UnbindDropArea()
    {
        if (_currentDropArea != null)
        {
            _currentDropArea.CurrentDraggable = null;
        }
        _currentDropArea = null;
    }

    public void SetDraggableAreaObj(GridSystem draggableAreaObj)
    {
        this.draggableAreaObj = draggableAreaObj;
    }

}
