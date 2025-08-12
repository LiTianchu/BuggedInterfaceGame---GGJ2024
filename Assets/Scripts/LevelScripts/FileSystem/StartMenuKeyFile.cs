using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuKeyFile : Draggable
{
    [SerializeField] private LayerMask fileSystemLockLayer;
    [SerializeField] private Color usedColor = new Color(1, 1, 1, 0.0f);
    private StartMenu _startMenu;
    private Color _originalMenuColor;
    private Vector3 _originalAnchorPos = new Vector3(999, 999, 999);
    private BiosEntranceFile _linkedBiosEntranceFile;
    private bool _isUsed;
    private Image _image;

    public new void Awake()
    {
        base.Awake();
        _rectTransform = GetComponent<RectTransform>();
        stayInView = false; // deactivate stay in view to use custom logic
        _image = GetComponent<Image>();
        InventoryManager.Instance.OnKeyFileUnlocked += UnlockKeyFile;
    }

    private void UnlockKeyFile()
    {
        gameObject.SetActive(true);
    }

    public void OnDestroy()
    {
        InventoryManager.Instance.OnKeyFileUnlocked -= UnlockKeyFile;
    }


    public void Initialize(StartMenu startMenu)
    {
        _startMenu = startMenu;
        _originalMenuColor = _startMenu.ItemGrid.GetComponent<Image>().color;
    }

    protected override void HandleBeginDrag()
    {
        if (_originalAnchorPos == new Vector3(999, 999, 999))
        {
            _originalAnchorPos = _rectTransform.anchoredPosition;
        }

        _startMenu.CanvasGroup.blocksRaycasts = false;
        _startMenu.CanvasGroup.interactable = false;
        _startMenu.ItemGrid.GetComponent<Image>().color = new Color(_originalMenuColor.r,
                                                                    _originalMenuColor.g,
                                                                    _originalMenuColor.b,
                                                                    0.2f);
    }

    protected override void HandleEndDrag()
    {
        _startMenu.CanvasGroup.blocksRaycasts = true;
        _startMenu.CanvasGroup.interactable = true;
        _startMenu.ItemGrid.GetComponent<Image>().color = _originalMenuColor;
        if (IsOverLock()) // drop the turret file and mark as used
        {
            UseKey();
        }

        _rectTransform.anchoredPosition = _originalAnchorPos;
    }

    protected override void HandleDrag()
    {
        Vector2 worldSpacePos = TransformUtils.GetRectTransformWorldCoordCenter(_rectTransform);

        // raycast to check if the pointer is over the grid
        RaycastHit2D hit = Physics2D.Raycast(worldSpacePos, Vector2.zero, Mathf.Infinity, fileSystemLockLayer);
        if (hit.collider != null)
        {
            BiosEntranceFile file = hit.collider.GetComponent<BiosEntranceFile>();
            if (file != null)
            {
                _linkedBiosEntranceFile = file;
                _linkedBiosEntranceFile.Highlight();
            }
        }
        else if (_linkedBiosEntranceFile != null)
        {
            _linkedBiosEntranceFile.NormalColor();
            _linkedBiosEntranceFile = null;
        }

    }

    private bool IsOverLock()
    {
        return _linkedBiosEntranceFile != null && _linkedBiosEntranceFile.Locked;
    }
    private void UseKey()
    {
        if(_isUsed)
        {
            return;
        }

        _linkedBiosEntranceFile.UnlockBiosEntrance();
        SetUsed();
    }

     private void SetUsed()
    {
        _isUsed = true;
        IsDraggable = false;
        _image.color = usedColor;
    }

}